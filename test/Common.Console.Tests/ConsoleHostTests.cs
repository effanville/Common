using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

namespace Effanville.Common.Console.Tests;

public class ConsoleHostTests
{
    private static IEnumerable<TestCaseData> CanRunTests()
    {
        yield return new TestCaseData(new string[] { }, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new [] { "Test" }, 0, "");
        yield return new TestCaseData(new [] { "Test", "--number", "4" }, 0, "");
    }

    [TestCaseSource(nameof(CanRunTests))]
    public async Task CanRun(string[] args, int expectedExitCode, string errorMessage)
    {
        var consoleInstance = new ConsoleInstance(null, null);
        var mock = new Mock<ILogger<ConsoleHost>>();
        ILogger<ConsoleHost> logger = mock.Object;
        var consoleContextMock = new Mock<ILogger<ConsoleContext>>();
        ILogger<ConsoleContext> consoleContextLogger = consoleContextMock.Object;       
        var mockLogger = new Mock<ILogger<TestCommand>>();
        var testCommand = new TestCommand(mockLogger.Object);
        testCommand.Options.Add(new CommandOption<string>("number", ""));
        var applicationLifetime = new Mock<IHostApplicationLifetime>();
        var applicationStartedCts = new CancellationTokenSource();
        applicationLifetime.Setup(x => x.ApplicationStarted).Returns(applicationStartedCts.Token);
        var consoleContext = new ConsoleContext(new ConsoleCommandArgs(args), new List<ICommand>() { testCommand },
            consoleInstance, consoleContextLogger);
        var host = new ConsoleHost(
            consoleContext,
            logger,
            applicationLifetime.Object);

        var cts = new CancellationTokenSource();
        await host.StartAsync(cts.Token);

        applicationStartedCts.Cancel();
        while (host.ExitCode == null)
        {
            await Task.Delay(10);
        }

        Assert.AreEqual(expectedExitCode, host.ExitCode);
        Times times = expectedExitCode != 0 ? Times.Once() : Times.Never();
        consoleContextMock.Verify(l =>
                l.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains(errorMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
            times
        );
    }
}