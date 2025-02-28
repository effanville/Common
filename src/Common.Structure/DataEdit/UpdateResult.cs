﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Effanville.Common.Structure.DataEdit;

/// <summary>
/// Contains <see cref="UpdateResult{TValue}"/> creation helpers
/// </summary>
public static class UpdateResult
{
    /// <summary>
    /// The update was an addition and it succeeded.
    /// </summary>
    public static UpdateResult<TValue> Add<TValue>(TValue newValue)
    {
        return new UpdateResult<TValue>()
        {
            Success = true,
            IsAdd = true,
            NewValue = newValue
        };
    }

    /// <summary>
    /// The update was a change and it succeeded
    /// </summary>
    public static UpdateResult<TValue> Change<TValue>(TValue oldValue, TValue newValue)
    {
        return new UpdateResult<TValue>()
        {
            Success = true,
            IsChange = true,
            OldValue = oldValue,
            NewValue = newValue
        };
    }

    /// <summary>
    /// The update was a deletion and it succeeded
    /// </summary>
    public static UpdateResult<TValue> Delete<TValue>(TValue oldValue)
    {
        return new UpdateResult<TValue>()
        {
            Success = true,
            IsDelete = true,
            OldValue = oldValue
        };
    }

    /// <summary>
    /// The update did not succeed
    /// </summary>
    public static UpdateResult<TValue> Fail<TValue>(TValue value, string message = null, bool isAdd = false, bool isChange = false, bool isDelete = false)
    {
        return new UpdateResult<TValue>()
        {
            Success = false,
            OldValue = value,
            IsAdd = isAdd,
            IsChange = isChange,
            IsDelete = isDelete,
            Message = message
        };
    }

    /// <summary>
    /// Returns an update detailing whether All are successful or not.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static UpdateResult<TValue> All<TValue>(IEnumerable<UpdateResult<TValue>> values)
    {
        bool success = values.All(x => x.Success);
        bool isAdd = values.Any(x => x.IsAdd);
        bool isChange = values.Any(x => x.IsChange);
        bool isDelete = values.Any(x => x.IsDelete);
        TValue OldValue = values.First().OldValue;
        TValue NewValue = values.Last().NewValue;
        StringBuilder builder = new StringBuilder();
        values.Select(x => builder.Append(x.Message));

        return new UpdateResult<TValue>
        {
            Success = success,
            IsAdd = isAdd,
            IsChange = isChange,
            IsDelete = isDelete,
            OldValue = OldValue,
            NewValue = NewValue,
            Message = builder.ToString()
        };
    }
}
