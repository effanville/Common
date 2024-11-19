using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NSubstitute;

using NUnit.Framework;

namespace Effanville.Common.Console.Tests;

public class ConsoleContextTests
{
    private static IEnumerable<TestCaseData> CanRunTests()
    {
        yield return new TestCaseData(new[] { "notTest" }, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new[] { "notTest", "--superman", "mark" }, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new[] { "Test" }, 0, "");
        yield return new TestCaseData(new[] { "Test", "subtest" }, 0, "");
        yield return new TestCaseData(new[] { "Test", "--number", "4" }, 0, "");
        yield return new TestCaseData(new string[] { }, 0, "");
        yield return new TestCaseData(new[] { "help" }, 0, "");
    }

    [TestCaseSource(nameof(CanRunTests))]
    public void CanRun(string[] args, int expectedExitCode, string errorMessage)
    {
        var mockLogger = Substitute.For<ILogger<TestCommand>>();
        TestCommand testCommand = new TestCommand(mockLogger);
        testCommand.Options.Add(new CommandOption<string>("number", ""));
        var consoleContextLogger = Substitute.For<ILogger<ConsoleContext>>();

        IConfiguration config = new ConfigurationBuilder()
            .AddCommandLine(new ConsoleCommandArgs(args).GetEffectiveArgs())
            .AddEnvironmentVariables()
            .Build();
        ConsoleContext context = new ConsoleContext(
            config,
            new List<ICommand>() { testCommand },
            consoleContextLogger);
        int actualExitCode = context.ValidateAndExecute();

        Assert.That(actualExitCode, Is.EqualTo(expectedExitCode));

        if (expectedExitCode != 0)
        {
            Expression<Predicate<object>> predicate = str => str.ToString().Contains(errorMessage);
            consoleContextLogger.Received()
                .Log(LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Is<Arg.AnyType>(predicate),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<Arg.AnyType, Exception, string>>());
        }
        else
        {
            consoleContextLogger.DidNotReceive()
                .Log(Arg.Is(LogLevel.Error),
                    Arg.Any<EventId>(),
                    Arg.Any<Arg.AnyType>(),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<Arg.AnyType, Exception, string>>());
        }
    }
    private static IEnumerable<TestCaseData> CanRunSubCommandTests()
    {
        yield return new TestCaseData(new[] { "notTest" }, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new[] { "notTest", "--superman", "mark" }, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new[] { "Test" }, 0, "");
        yield return new TestCaseData(new[] { "Test", "Subtest" }, 0, "");
        yield return new TestCaseData(new[] { "Test", "subtest" }, 0, "");
        yield return new TestCaseData(new[] { "Test", "--number", "4" }, 0, "");
        yield return new TestCaseData(new string[] { }, 0, "");
        yield return new TestCaseData(new[] { "help" }, 0, "");
    }

    [TestCaseSource(nameof(CanRunSubCommandTests))]
    public void CanRunSubCommand(string[] args, int expectedExitCode, string errorMessage)
    {
        var mockLogger = Substitute.For<ILogger<TestCommand>>();
        TestCommand testCommand = new TestCommand(mockLogger);
        testCommand.Options.Add(new CommandOption<string>("number", ""));

        TestCommand subCommand = new TestCommand(mockLogger) { Name = "Subtest" };
        subCommand.Options.Add(new CommandOption<string>("otherNumber", ""));
        testCommand.SubCommands.Add(subCommand);
        var consoleContextLogger = Substitute.For<ILogger<ConsoleContext>>();

        IConfiguration config = new ConfigurationBuilder()
            .AddCommandLine(new ConsoleCommandArgs(args).GetEffectiveArgs())
            .AddEnvironmentVariables()
            .Build();
        ConsoleContext context = new ConsoleContext(
            config,
            new List<ICommand>() { testCommand },
            consoleContextLogger);
        int actualExitCode = context.ValidateAndExecute();

        Assert.That(actualExitCode, Is.EqualTo(expectedExitCode));

        if (expectedExitCode != 0)
        {
            Expression<Predicate<object>> predicate = str => str.ToString().Contains(errorMessage);
            consoleContextLogger.Received()
                .Log(Arg.Is(LogLevel.Error),
                    Arg.Any<EventId>(),
                    Arg.Is<Arg.AnyType>(predicate),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<Arg.AnyType, Exception, string>>());
        }
        else
        {
            consoleContextLogger.DidNotReceive()
                .Log(Arg.Is(LogLevel.Error),
                    Arg.Any<EventId>(),
                    Arg.Any<Arg.AnyType>(),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<Arg.AnyType, Exception, string>>());
        }
    }
}