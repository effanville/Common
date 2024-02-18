using System;

using Effanville.Common.Structure.Extensions;

namespace Effanville.Common.Console.Options
{
    /// <summary>
    /// An option to be input by the user.
    /// </summary>
    /// <typeparam name="T">The type for the input value.</typeparam>
    public sealed class CommandOption<T> : CommandOption
    {
        private T _value;

        /// <summary>
        /// The method for validation of this option.
        /// </summary>
        public new Func<T, bool> Validator { get; }

        /// <inheritdoc/>
        public override object ValueAsObject => Value;

        /// <summary>
        /// The value after validation for this option.
        /// </summary>
        public T Value
        {
            get
            {
                if (_value != null)
                {
                    return _value;
                }

                return !Validate() ? default(T) : _value;
            }
            private set => _value = value;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public CommandOption(string name, string description, Func<T, bool> validator = null)
            : this(name, description, false, validator)
        {
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public CommandOption(string name, string description, T defaultValue, Func<T, bool> validator = null)
            : this(name, description, false, validator)
        {
            InputValue = defaultValue.ToString();
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public CommandOption(string name, string description, bool required, Func<T, bool> validator = null)
            : base(name, description, required, null)
        {
            // set validator here, as base validator is a Func<object, bool>
            Validator = validator ?? (_ => true);
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public CommandOption(string name, string description, bool required, T defaultValue,
            Func<T, bool> validator = null)
            : this(name, description, required, validator)
        {
            Value = defaultValue;
        }

        /// <inheritdoc/>
        public override bool Validate()
        {
            bool nullInput = string.IsNullOrEmpty(InputValue);
            if (nullInput && Required)
            {
                ErrorMessage = "Is required and no value supplied.";
                return false;
            }
            if (nullInput)
            {
                return true;
            }

            string parsedInputValue = InputValue;
            if (InputValue.StartsWith(EnvVarPrefix))
            {
                string envVarName = InputValue.Substring(EnvVarPrefix.Length);
                parsedInputValue = Environment.GetEnvironmentVariable(envVarName);
            }

            T parsedValue;
            try
            {
                if (typeof(T).IsEnum)
                {
                    parsedValue = parsedInputValue.ToEnum<T>();
                }
                else
                {
                    parsedValue = (T)Convert.ChangeType(parsedInputValue, typeof(T));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }

            bool valid = Validator(parsedValue);
            if (valid)
            {
                Value = parsedValue;
            }
            else
            {
                ErrorMessage = $"Argument '{parsedValue}' failed in validation.";
            }

            return valid;
        }
    }
}