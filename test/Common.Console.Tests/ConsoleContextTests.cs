using System;
using System.Collections.Generic;

using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

namespace Effanville.Common.Console.Tests;

public class ConsoleContextTests
{
    private static IEnumerable<TestCaseData> CanRunTests()
    {
        yield return new TestCaseData(new string[] { }, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new [] { "Test" }, 0, "");
        yield return new TestCaseData(new [] { "Test", "--number", "4" }, 0, "");
    }

    [TestCaseSource(nameof(CanRunTests))]
    public void CanRun(string[] args, int expectedExitCode, string errorMessage)
    {
        var consoleInstance = new ConsoleInstance(null, null);
        var mockLogger = new Mock<ILogger<TestCommand>>();
        var testCommand = new TestCommand(mockLogger.Object);
        testCommand.Options.Add(new CommandOption<string>("number", ""));
        var consoleContextMock = new Mock<ILogger<ConsoleContext>>();
        ILogger<ConsoleContext> consoleContextLogger = consoleContextMock.Object;
        var context = new ConsoleContext(
            new ConsoleCommandArgs(args),
            new List<ICommand>() { testCommand },
            consoleInstance,
            consoleContextLogger);
        int actualExitCode = context.ValidateAndExecute();

        Assert.AreEqual(expectedExitCode, actualExitCode);
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