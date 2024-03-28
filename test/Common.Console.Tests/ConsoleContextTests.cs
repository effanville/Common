using System;
using System.Collections.Generic;

using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

namespace Effanville.Common.Console.Tests;

public class ConsoleContextTests
{
    private static IEnumerable<TestCaseData> CanRunTests()
    {
        yield return new TestCaseData(new[] { "notTest" }, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new[] { "notTest", "--superman", "mark"}, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new [] { "Test" }, 0, "");
        yield return new TestCaseData(new [] { "Test", "subtest" }, 0, "");
        yield return new TestCaseData(new [] { "Test", "--number", "4" }, 0, "");
        yield return new TestCaseData(new string[] { }, 0, "");
        yield return new TestCaseData(new[] { "help"}, 0, "");
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
        
        IConfiguration config = new ConfigurationBuilder()
            .AddCommandLine(new ConsoleCommandArgs(args).GetEffectiveArgs())
            .AddEnvironmentVariables()
            .Build();
        var context = new ConsoleContext(
            config,
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
    private static IEnumerable<TestCaseData> CanRunSubCommandTests()
    {
        yield return new TestCaseData(new[] { "notTest" }, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new[] { "notTest", "--superman", "mark"}, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new [] { "Test" }, 0, "");
        yield return new TestCaseData(new [] { "Test", "Subtest" }, 0, "");
        yield return new TestCaseData(new [] { "Test", "subtest" }, 0, "");
        yield return new TestCaseData(new [] { "Test", "--number", "4" }, 0, "");
        yield return new TestCaseData(new string[] { }, 0, "");
        yield return new TestCaseData(new[] { "help"}, 0, "");
    }
    
    [TestCaseSource(nameof(CanRunSubCommandTests))]
    public void CanRunSubCommand(string[] args, int expectedExitCode, string errorMessage)
    {
        var consoleInstance = new ConsoleInstance(null, null);
        var mockLogger = new Mock<ILogger<TestCommand>>();
        var testCommand = new TestCommand(mockLogger.Object);
        testCommand.Options.Add(new CommandOption<string>("number", ""));

        var subCommand = new TestCommand(mockLogger.Object) { Name = "Subtest" };
        subCommand.Options.Add(new CommandOption<string>("otherNumber", ""));
        testCommand.SubCommands.Add(subCommand);
        var consoleContextMock = new Mock<ILogger<ConsoleContext>>();
        ILogger<ConsoleContext> consoleContextLogger = consoleContextMock.Object;
        
        IConfiguration config = new ConfigurationBuilder()
            .AddCommandLine(new ConsoleCommandArgs(args).GetEffectiveArgs())
            .AddEnvironmentVariables()
            .Build();
        var context = new ConsoleContext(
            config,
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