using System;

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
            : base(name, description, null)
        {
            if (validator != null)
            {
                Validator = validator;
            }
            else
            {
                Validator = input => true;
            }
        }

        /// <inheritdoc/>
        public override bool Validate()
        {
            T parsedValue;
            try
            {
                parsedValue = (T)Convert.ChangeType(InputValue, typeof(T));
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
                ErrorMessage = "Failed to validate option.";
            }

            return valid;
        }
    }
}
