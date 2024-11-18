using System;
using System.Collections.Generic;

namespace Effanville.Common.Console;

/// <summary>
/// Wrapper for arguments passed into console.
/// </summary>
public sealed class ConsoleCommandArgs
{
    private readonly string[] _commandNames;
    private readonly string[] _args;

    /// <summary>
    /// Construct an instance
    /// </summary>
    /// <param name="args">The arguments to use.</param>
    public ConsoleCommandArgs(string[] args)
    {
        if (args == null || args.Length == 0)
        {
            _args = args;
            return;
        }

        int index = 0;
        var commandNames = new List<string>();
        while (index < args.Length && !args[index].StartsWith("--"))
        {
            commandNames.Add(args[index]);
            index++;
        }

        _commandNames = commandNames.ToArray();

        _args = args[index..args.Length];
    }

    /// <summary>
    /// Returns the args with the command names prefaced so that the configuration builder can utilise.
    /// </summary>
    public string[] GetEffectiveArgs()
    {
        if (_commandNames == null)
        {
            return _args ?? Array.Empty<string>();
        }

        var args = new List<string> { "--CommandName", string.Join(';', _commandNames) };
        args.AddRange(_args);
        return args.ToArray();
    }
}