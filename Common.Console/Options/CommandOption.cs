using System;

namespace Common.Console.Options
{
    /// <summary>
    /// A container for a user specified command line input.
    /// </summary>
    public class CommandOption
    {
        /// <summary>
        /// The name of this option. Used to input the value
        /// for this option in the command line.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// A description of this option. Printed in help docs.
        /// </summary>
        public string Description
        {
            get;
        }

        /// <summary>
        /// If true a value for this option must be set.
        /// </summary>
        public bool Required
        {
            get;
        }

        /// <summary>
        /// The value input by the user, before parsing.
        /// </summary>
        public string InputValue
        {
            get;
            set;
        }

        /// <summary>
        /// The mechanism to validate the option.
        /// </summary>
        public Func<object, bool> Validator
        {
            get;
        }

        /// <summary>
        /// Any error gone wrong in validation.
        /// </summary>
        protected string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// The value after validation.
        /// </summary>
        public virtual object ValueAsObject
        {
            get;
            private set;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public CommandOption(string name, string description, Func<object, bool> validator)
            : this(name, description, false, validator)
        {
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public CommandOption(string name, string description, string defaultValue, Func<object, bool> validator = null)
            : this(name, description, false, validator)
        {
            InputValue = defaultValue;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public CommandOption(string name, string description, bool required, Func<object, bool> validator)
        {
            Name = name;
            Description = description;
            Required = required;
            Validator = validator;
        }

        /// <summary>
        /// Return a friendly string detailing the error message.
        /// </summary>
        public string GetPrettyErrorMessage()
        {
            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                return $"[Option {Name}] - No Error.";
            }

            return $"[Option {Name}] - {ErrorMessage}";
        }

        /// <summary>
        /// Validate the already specified <see cref="InputValue"/> value based on the validation
        /// specified in construction.
        /// </summary>
        public virtual bool Validate()
        {
            ValueAsObject = InputValue;
            return Validator(InputValue);
        }
    }
}
