using System;
using System.Collections.Generic;
using Common.Console.Options;
using NUnit.Framework;

namespace Common.Console.Tests
{
    [TestFixture]
    public class CommandValidationTests
    {
        private static IEnumerable<TestCaseData> SingleOptionTestSource()
        {
            Func<double, bool> alwaysValid = x => true;

            Func<double, bool> positiveValid = x => x >= 0;
            yield return new TestCaseData(new[] { ("number", false, alwaysValid) }, new[] { "--number", "4" }, true, "").SetName("SingleOption - Passes - no validator");

            yield return new TestCaseData(new[] { ("number", true, alwaysValid) }, new[] { "--number", "4" }, true, "").SetName("SingleOption - Passes - no validator and required");

            yield return new TestCaseData(new[] { ("number", false, alwaysValid) }, new[] { "--Number", "4" }, true, "").SetName("SingleOption - Passes - case insensitve name");

            yield return new TestCaseData(new[] { ("number", false, positiveValid) }, new[] { "--number", "4" }, true, "").SetName("SingleOption - Passes - with validator");

            yield return new TestCaseData(new[] { ("number", false, positiveValid) }, new[] { "--number", "-5" }, false, "Option number - Failed to validate option.").SetName("SingleOption - Fails - validator says false");

            yield return new TestCaseData(new[] { ("number", true, positiveValid) }, Array.Empty<string>(), false, "Option number - No value supplied for required option.").SetName("SingleOption - Fails - Required value not supplied");

            yield return new TestCaseData(new[] { ("number", false, alwaysValid) }, new[] { "--dog", "4" }, false, "Option dog is not a valid option for the Test command.").SetName("SingleOption - Fails - OptionName not found");

            yield return new TestCaseData(new[] { ("number", false, alwaysValid) }, new[] { "--number", "five" }, false, "Option number - Input string was not in a correct format.").SetName("SingleOption - Fails - value not a double.");
        }

        private static IEnumerable<TestCaseData> TwoOptionTestSource()
        {
            Func<double, bool> alwaysValid = x => true;

            Func<double, bool> positiveValid = x => x >= 0;
            yield return new TestCaseData(new[] { ("number", false, alwaysValid), ("other", false, alwaysValid) }, new[] { "--number", "4", "--other", "5" }, true, "").SetName("TwoOption - Passes - no validator");

            yield return new TestCaseData(new[] { ("number", true, alwaysValid), ("other", true, alwaysValid) }, new[] { "--number", "4", "--other", "5" }, true, "").SetName("TwoOption - Passes - no validator but required");

            yield return new TestCaseData(new[] { ("number", false, positiveValid), ("other", false, positiveValid) }, new[] { "--number", "4", "--other", "5" }, true, "").SetName("TwoOption - Passes - with validator");

            yield return new TestCaseData(new[] { ("number", false, positiveValid), ("other", false, positiveValid) }, new[] { "--number", "4"}, true, "").SetName("TwoOption - Passes - with validator not all supplied");

            yield return new TestCaseData(new[] { ("number", true, positiveValid), ("other", true, positiveValid) }, new[] { "--number", "4" }, false, "Option other - No value supplied for required option.").SetName("TwoOption - Fails - with validator not all supplied");

            yield return new TestCaseData(new[] { ("number", false, positiveValid), ("other", false, alwaysValid) }, new[] { "--number", "-4", "--other", "5" }, false, "Option number - Failed to validate option.").SetName("TwoOption - Fails - First Positive validator");

            yield return new TestCaseData(new[] { ("number", false, positiveValid), ("other", false, positiveValid) }, new[] { "--number", "4", "--other", "-5" }, false, "Option other - Failed to validate option.").SetName("TwoOption - Fails - Second Positive validator");

            yield return new TestCaseData(new[] { ("number", false, alwaysValid), ("other", false, alwaysValid) }, new[] { "--dog", "4" }, false, "Option dog is not a valid option for the Test command.").SetName("SingleOption - Fails - OptionName not found");
        }

        [TestCaseSource(nameof(SingleOptionTestSource))]
        [TestCaseSource(nameof(TwoOptionTestSource))]
        public void CommandValidatorTester((string, bool, Func<double, bool>)[] options, string[] args, bool expectedOutcome, string expectedReport)
        {
            string error = "";
            void writeError(string err)
            {
                error = err;
            }
            string report = "";
            void writeReport(string rep)
            {
                report = rep;
            }

            var consoleInstance = new ConsoleInstance(writeError, writeReport);
            var testCommand = new TestCommand();
            foreach (var optionData in options)
            {
                var option = new CommandOption<double>(optionData.Item1, "", optionData.Item2, optionData.Item3);
                testCommand.Options.Add(option);
            }
            bool validated = testCommand.Validate(consoleInstance, args);

            Assert.Multiple(() =>
            {

                Assert.AreEqual(expectedOutcome, validated);
                Assert.AreEqual(expectedReport, error);
            });
        }
    }
}