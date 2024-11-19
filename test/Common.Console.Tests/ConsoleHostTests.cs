using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NSubstitute;

using NUnit.Framework;

namespace Effanville.Common.Console.Tests;

public class ConsoleHostTests
{
    private static IEnumerable<TestCaseData> CanRunTests()
    {
        yield return new TestCaseData(new[] { "NotATest" }, 2,
            "Command line input failed validation.");
        yield return new TestCaseData(new[] { "Test" }, 0, "");
        yield return new TestCaseData(new[] { "Test", "--number", "4" }, 0, "");
    }

    [TestCaseSource(nameof(CanRunTests))]
    public async Task CanRun(string[] args, int expectedExitCode, string errorMessage)
    {
        ILogger<ConsoleHost> logger = Substitute.For<ILogger<ConsoleHost>>();
        ILogger<ConsoleContext> consoleContextLogger = Substitute.For<ILogger<ConsoleContext>>();
        ILogger<TestCommand> mockLogger = Substitute.For<ILogger<TestCommand>>();
        TestCommand testCommand = new TestCommand(mockLogger);
        testCommand.Options.Add(new CommandOption<string>("number", ""));
        IHostApplicationLifetime applicationLifetime = Substitute.For<IHostApplicationLifetime>();
        CancellationTokenSource applicationStartedCts = new CancellationTokenSource();
        IConfiguration config = new ConfigurationBuilder()
            .AddCommandLine(new ConsoleCommandArgs(args).GetEffectiveArgs())
            .AddEnvironmentVariables()
            .Build();
        applicationLifetime.ApplicationStarted.Returns(applicationStartedCts.Token);
        ConsoleContext consoleContext = new ConsoleContext(
            config,
            new List<ICommand>() { testCommand },
            consoleContextLogger);
        ConsoleHost host = new ConsoleHost(
            consoleContext,
            logger,
            applicationLifetime);

        CancellationTokenSource cts = new CancellationTokenSource();
        await host.StartAsync(cts.Token);

        applicationStartedCts.Cancel();
        while (host.ExitCode == null)
        {
            await Task.Delay(10);
        }

        Assert.That(host.ExitCode, Is.EqualTo(expectedExitCode));
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
}