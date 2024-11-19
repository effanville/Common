using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Effanville.Common.Console.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NSubstitute;

using NUnit.Framework;

namespace Effanville.Common.Console.Tests
{
    [TestFixture]
    public class CommandValidationTests
    {
        private static IEnumerable<TestCaseData> SingleOptionTestSource()
        {
            Func<double, bool> alwaysValid = _ => true;

            Func<double, bool> positiveValid = x => x >= 0;
            yield return new TestCaseData(
                new[] { ("number", false, alwaysValid) },
                new[] { "--number", "4" },
                true,
                "").SetName("SingleOption - Passes - no validator");

            yield return new TestCaseData(
                new[] { ("number", true, alwaysValid) },
                new[] { "--number", "4" },
                true,
                "").SetName("SingleOption - Passes - no validator and required");

            yield return new TestCaseData(
                new[] { ("number", false, alwaysValid) },
                new[] { "--Number", "4" },
                true,
                "").SetName("SingleOption - Passes - case insensitive name");

            yield return new TestCaseData(
                new[] { ("number", false, positiveValid) },
                new[] { "--number", "4" },
                true,
                "").SetName("SingleOption - Passes - with validator");

            yield return new TestCaseData(
                new[] { ("number", false, positiveValid) },
                new[] { "--number", "-5" },
                false,
                "Argument '-5' failed in validation.")
                .SetName("SingleOption - Fails - validator says false");

            yield return new TestCaseData(
                new[] { ("number", true, positiveValid) },
                Array.Empty<string>(),
                false,
                "Is required and no value supplied.")
                .SetName("SingleOption - Fails - Required value not supplied");

            yield return new TestCaseData(
                new[] { ("number", false, alwaysValid) },
                new[] { "--dog", "4" },
                true,
                "")
                .SetName("SingleOption - Fails - OptionName not found");

            yield return new TestCaseData(
                new[] { ("number", false, alwaysValid) },
                new[] { "--number", "five" },
                false,
                "The input string 'five' was not in a correct format.")
                .SetName("SingleOption - Fails - value not a double.");
        }

        private static IEnumerable<TestCaseData> TwoOptionTestSource()
        {
            Func<double, bool> alwaysValid = _ => true;

            Func<double, bool> positiveValid = x => x >= 0;
            yield return new TestCaseData(
                new[] { ("number", false, alwaysValid), ("other", false, alwaysValid) },
                new[] { "--number", "4", "--other", "5" },
                true,
                "").SetName("TwoOption - Passes - no validator");

            yield return new TestCaseData(
                new[] { ("number", true, alwaysValid), ("other", true, alwaysValid) },
                new[] { "--number", "4", "--other", "5" },
                true,
                "").SetName("TwoOption - Passes - no validator but required");

            yield return new TestCaseData(
                new[] { ("number", false, positiveValid), ("other", false, positiveValid) },
                new[] { "--number", "4", "--other", "5" },
                true,
                "").SetName("TwoOption - Passes - with validator");

            yield return new TestCaseData(
                new[] { ("number", false, positiveValid), ("other", false, positiveValid) },
                new[] { "--number", "4" },
                true,
                "").SetName("TwoOption - Passes - with validator not all supplied");

            yield return new TestCaseData(
                new[] { ("number", true, positiveValid), ("other", true, positiveValid) },
                new[] { "--number", "4" },
                false,
                "Is required and no value supplied.")
                .SetName("TwoOption - Fails - with validator not all supplied");

            yield return new TestCaseData(
                new[] { ("number", false, positiveValid), ("other", false, alwaysValid) },
                new[] { "--number", "-4", "--other", "5" },
                false,
                "Argument '-4' failed in validation.")
                .SetName("TwoOption - Fails - First Positive validator");

            yield return new TestCaseData(
                new[] { ("number", false, positiveValid), ("other", false, positiveValid) },
                new[] { "--number", "4", "--other", "-5" }, false,
                "Argument '-5' failed in validation.")
                .SetName("TwoOption - Fails - Second Positive validator");

            yield return new TestCaseData(
                new[] { ("number", false, alwaysValid), ("other", false, alwaysValid) },
                new[] { "--dog", "4" },
                true,
                "")
                .SetName("TwoOption - Fails - OptionName not found");
        }

        [TestCaseSource(nameof(SingleOptionTestSource))]
        [TestCaseSource(nameof(TwoOptionTestSource))]
        public void CommandValidatorTester((string, bool, Func<double, bool>)[] options, string[] args, bool expectedOutcome, string expectedError)
        {
            ILogger<TestCommand> mockLogger = Substitute.For<ILogger<TestCommand>>();
            TestCommand testCommand = new TestCommand(mockLogger);
            foreach ((string, bool, Func<double, bool>) optionData in options)
            {
                CommandOption<double> option = new CommandOption<double>(optionData.Item1, "", optionData.Item2, optionData.Item3);
                testCommand.Options.Add(option);
            }
            IConfiguration config = new ConfigurationBuilder()
                .AddCommandLine(new ConsoleCommandArgs(args).GetEffectiveArgs())
                .AddEnvironmentVariables()
                .Build();
            bool validated = testCommand.Validate(config);

            Assert.Multiple(() =>
            {
                Assert.That(validated, Is.EqualTo(expectedOutcome));
                if (!expectedOutcome)
                {
                    Expression<Predicate<object>> predicate = str => str.ToString().Contains(expectedError);
                    mockLogger.Received(1)
                        .Log(Arg.Is(LogLevel.Error),
                            Arg.Any<EventId>(),
                            Arg.Is<Arg.AnyType>(predicate),
                            Arg.Any<Exception>(),
                            Arg.Any<Func<Arg.AnyType, Exception, string>>());
                }
                else
                {
                    mockLogger.DidNotReceive()
                    .Log(Arg.Is(LogLevel.Error),
                        Arg.Any<EventId>(),
                        Arg.Any<Arg.AnyType>(),
                        Arg.Any<Exception>(),
                        Arg.Any<Func<Arg.AnyType, Exception, string>>());
                }
            });
        }
    }
}