using System;

using Common.Structure.Extensions;

namespace Common.Console.Options
{
    /// <summary>
    /// An option to be input by the user.
    /// </summary>
    /// <typeparam name="T">The type for the input value.</typeparam>
    public sealed class CommandOption<T> : CommandOption
    {
        /// <summary>
        /// The method for validation of this option.
        /// </summary>
        public new Func<T, bool> Validator
        {
            get;
        }

        /// <inheritdoc/>
        public override object ValueAsObject
        {
            get
            {
                return Value;
            }
        }

        private T fValue;
        /// <summary>
        /// The value after validation for this option.
        /// </summary>
        public T Value
        {
            get
            {
                if (fValue == null)
                {
                    if (!Validate())
                    {
                        return default(T);
                    }
                }

                return fValue;
            }

            private set
            {
                fValue = value;
            }
        }

        public CommandOption(string name, string description, Func<T, bool> validator = null)
            : this(name, description, false, validator)
        {
        }

        public CommandOption(string name, string description, T defaultValue, Func<T, bool> validator = null)
            : this(name, description, false, validator)
        {
            InputValue = defaultValue.ToString();
        }

        public CommandOption(string name, string description, bool required, Func<T, bool> validator = null)
            : base(name, description, required, null)
        {
            // set validator here, as base validator is a Func<object, bool>
            if (validator != null)
            {
                Validator = validator;
            }
            else
            {
                Validator = input => true;
            }
        }

        public CommandOption(string name, string description, bool required, T defaultValue, Func<T, bool> validator = null)
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
            else if (nullInput)
            {
                return true;
            }
            else
            {
                T parsedValue;
                try
                {
                    if (typeof(T).IsEnum)
                    {
                        parsedValue = InputValue.ToEnum<T>();
                    }
                    else
                    {
                        parsedValue = (T)Convert.ChangeType(InputValue, typeof(T));
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
}
