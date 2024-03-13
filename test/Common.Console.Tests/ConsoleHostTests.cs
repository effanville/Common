using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;

using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;
using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.Reporting;

using Microsoft.Extensions.Hosting;

using Moq;

using NUnit.Framework;

namespace Effanville.Common.Console.Tests;

public class ConsoleHostTests
{
    private static IEnumerable<TestCaseData> CanRunTests()
    {
        yield return new TestCaseData(new string[] { }, 2,
            "Options specified were not valid.");
        yield return new TestCaseData(new [] { "Test" }, 0, "");
        yield return new TestCaseData(new [] { "Test", "--number", "4" }, 0, "");
    }

    [TestCaseSource(nameof(CanRunTests))]
    public async Task CanRun(string[] args, int expectedExitCode, string errorMessage)
    {
        string error = string.Empty;
        var commandArgs = new ConsoleCommandArgs(args);
        var fileSystem = new MockFileSystem();
        var consoleInstance = new ConsoleInstance(WriteError, WriteReport);
        var logReporter = new LogReporter(ReportAction, new SingleTaskQueue(), saveInternally: true);
        var testCommand = new TestCommand();
        testCommand.Options.Add(new CommandOption<string>("number", ""));
        var applicationLifetime = new Mock<IHostApplicationLifetime>();
        var applicationStartedCts = new CancellationTokenSource();
        applicationLifetime.Setup(x => x.ApplicationStarted).Returns(applicationStartedCts.Token);
        var host = new ConsoleHost(
            commandArgs,
            fileSystem,
            consoleInstance,
            logReporter,
            new List<ICommand>() { testCommand },
            applicationLifetime.Object);

        var cts = new CancellationTokenSource();
        await host.StartAsync(cts.Token);

        applicationStartedCts.Cancel();
        while (host._exitCode == null)
        {
            await Task.Delay(10);
        }

        Assert.AreEqual(expectedExitCode, host._exitCode);
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