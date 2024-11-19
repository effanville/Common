using System;

using NUnit.Framework;

namespace Effanville.Common.Console.Tests;

public class ConsoleCommandArgsTests
{
    [TestCase(null, new string[] {})]
    [TestCase(new string[] {}, new string[] {})]
    [TestCase(new string[] {"stats", "--filepath", "../database/url-address.xml", "--outputPath", "../database/stats.html", "--mailTo", "env:mailToName"},
        new [] {"--CommandName", "stats", "--filepath", "../database/url-address.xml", "--outputPath", "../database/stats.html", "--mailTo", "env:mailToName"})]
    [TestCase(new [] {"download", "all", "--filepath", "../database/url-address.xml"},
        new [] {"--CommandName","download;all", "--filepath", "../database/url-address.xml"})]
    public void Test(string[] args, string[] expectedEffectiveArgs)
    {
        ConsoleCommandArgs commandArgs = new ConsoleCommandArgs(args);
        string[] actualEffectiveArgs = commandArgs.GetEffectiveArgs();

        Assert.That(actualEffectiveArgs, Is.EqualTo(expectedEffectiveArgs).AsCollection);
    }
}