﻿using System.Text;

namespace Effanville.Common.Structure.DataEdit;

/// <summary>
/// Encompasses details about an update to an object of type <see typecref="TValue"/>
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class UpdateResult<TValue>
{
    /// <summary>
    /// 
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Is the update an addition
    /// </summary>
    public bool IsAdd { get; init; }

    /// <summary>
    /// Is the update a change
    /// </summary>
    public bool IsChange { get; init; }

    /// <summary>
    /// Is the update a deletion
    /// </summary>
    public bool IsDelete { get; init; }

    /// <summary>
    /// The previous value before the update
    /// </summary>
    public TValue OldValue { get; init; }

    /// <summary>
    /// The value after the update
    /// </summary>
    public TValue NewValue { get; init; }

    /// <summary>
    /// Any message about the update
    /// </summary>
    public string Message { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("Value");
        if (IsChange)
        {
            sb.Append(" changed. OldValue=")
                .Append(OldValue)
                .Append(",NewValue=")
                .Append(NewValue);
        }

        if (IsDelete)
        {
            sb.Append(" deleted. OldValue=").Append(OldValue);
        }

        if (IsAdd)
        {
            sb.Append(" added. NewValue=").Append(NewValue);
        }

        string successString = Success ? "Succeeded" : "Failed";
        sb.Append($",Update {successString}");

        if (!string.IsNullOrEmpty(Message))
        {
            sb.Append(", Message=").Append(Message);
        }

        return sb.ToString();
    }
}
