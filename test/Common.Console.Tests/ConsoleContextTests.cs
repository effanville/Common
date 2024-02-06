using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

using Common.Structure.DataStructures;
using Common.Structure.Reporting;

using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;

using NUnit.Framework;

namespace Effanville.Common.Console.Tests;

public class ConsoleContextTests
{
    private static IEnumerable<TestCaseData> CanRunTests()
    {
        yield return new TestCaseData(new string[] { }, 2,
            "Options specified were not valid.");
        yield return new TestCaseData(new [] { "Test" }, 0, "");
        yield return new TestCaseData(new [] { "Test", "--number", "4" }, 0, "");
    }

    [TestCaseSource(nameof(CanRunTests))]
    public void CanRun(string[] args, int expectedExitCode, string errorMessage)
    {
        string error = string.Empty;
        var fileSystem = new MockFileSystem();
        var consoleInstance = new ConsoleInstance(WriteError, WriteReport);
        var logReporter = new LogReporter(ReportAction, new SingleTaskQueue(), saveInternally: true);
        var testCommand = new TestCommand();
        testCommand.Options.Add(new CommandOption<string>("number", ""));
        int actualExitCode = ConsoleContext.SetAndExecute(
            args,
            fileSystem,
            consoleInstance,
            logReporter,
            new List<ICommand>() { testCommand });

        Assert.AreEqual(expectedExitCode, actualExitCode);
        Assert.AreEqual(errorMessage, error);
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