using System;
using System.Collections.Generic;

using Common.Structure.Reporting;

using Effanville.Common.Console.Options;

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
                "[Option number] - Argument '-5' failed in validation.")
                .SetName("SingleOption - Fails - validator says false");

            yield return new TestCaseData(
                new[] { ("number", true, positiveValid) }, 
                Array.Empty<string>(), 
                false, 
                "(Error) - [Validate] - [Option number] - Is required and no value supplied.")
                .SetName("SingleOption - Fails - Required value not supplied");

            yield return new TestCaseData(
                new[] { ("number", false, alwaysValid) }, 
                new[] { "--dog", "4" }, 
                false, 
                "(Error) - [Test.Validate] - Option dog is not a valid option.")
                .SetName("SingleOption - Fails - OptionName not found");

            yield return new TestCaseData(
                new[] { ("number", false, alwaysValid) }, 
                new[] { "--number", "five" }, 
                false, 
                "(Error) - [Validate] - [Option number] - The input string 'five' was not in a correct format.")
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
                new[] { "--number", "4"}, 
                true, 
                "").SetName("TwoOption - Passes - with validator not all supplied");

            yield return new TestCaseData(
                new[] { ("number", true, positiveValid), ("other", true, positiveValid) }, 
                new[] { "--number", "4" }, 
                false, 
                "(Error) - [Validate] - [Option other] - Is required and no value supplied.")
                .SetName("TwoOption - Fails - with validator not all supplied");

            yield return new TestCaseData(
                new[] { ("number", false, positiveValid), ("other", false, alwaysValid) }, 
                new[] { "--number", "-4", "--other", "5" }, 
                false, 
                "(Error) - [Validate] - [Option number] - Argument '-4' failed in validation.")
                .SetName("TwoOption - Fails - First Positive validator");

            yield return new TestCaseData(
                new[] { ("number", false, positiveValid), ("other", false, positiveValid) }, 
                new[] { "--number", "4", "--other", "-5" }, false, 
                "(Error) - [Validate] - [Option other] - Argument '-5' failed in validation.")
                .SetName("TwoOption - Fails - Second Positive validator");

            yield return new TestCaseData(
                new[] { ("number", false, alwaysValid), ("other", false, alwaysValid) }, 
                new[] { "--dog", "4" }, 
                false, 
                "(Error) - [Validate] - Option dog is not a valid option.")
                .SetName("TwoOption - Fails - OptionName not found");
        }

        [TestCaseSource(nameof(SingleOptionTestSource))]
        [TestCaseSource(nameof(TwoOptionTestSource))]
        public void CommandValidatorTester((string, bool, Func<double, bool>)[] options, string[] args, bool expectedOutcome, string expectedError)
        {
            string error = "";

            var consoleInstance = new ConsoleInstance(WriteError, WriteReport);
            var logReporter = new LogReporter(ReportAction);
            var testCommand = new TestCommand();
            foreach (var optionData in options)
            {
                var option = new CommandOption<double>(optionData.Item1, "", optionData.Item2, optionData.Item3);
                testCommand.Options.Add(option);
            }
            bool validated = testCommand.Validate(consoleInstance, logReporter, args);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedOutcome, validated);
                Assert.AreEqual(expectedError, error);
            });
            return;

            void WriteError(string err)
            {
                error = err;
            }
            
            void WriteReport(string rep)
            {
            }
            
            void ReportAction(ReportSeverity severity, ReportType reportType, string location, string text)
            {
                string message = $"({reportType}) - [{location}] - {text}";
                if (reportType == ReportType.Error)
                {
                    WriteError(message);
                }
                else
                {
                    WriteReport(message);
                }
            }
        }
    }
}