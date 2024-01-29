using System.Collections.Generic;
using System.Linq;

namespace Common.Console.Options
{
    /// <summary>
    /// Extension methods for <see cref="CommandOption"/>s and <see cref="CommandOption{T}"/>s.
    /// </summary>
    public static class CommandOptionExtensions
    {
        /// <summary>
        /// Returns a <see cref="CommandOption{T}"/> from a list of <see cref="CommandOption"/>s given by
        /// the specified name. Is null if no option exists.
        /// </summary>
        public static CommandOption<T> GetOption<T>(this IList<CommandOption> options, string optionName) 
            => options.FirstOrDefault(option => option.Name == optionName) as CommandOption<T>;
    }
}
