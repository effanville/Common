using System.Collections.Generic;

namespace StructureCommon.Validation
{
    /// <summary>
    /// A class detailing the validity of a property or field.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// whether the result is valid or not.
        /// </summary>
        public bool IsValid
        {
            get; set;
        }

        /// <summary>
        /// The name of the property that is valid or not.
        /// </summary>
        public string PropertyName
        {
            get; set;
        }

        /// <summary>
        /// The first message about the validity.
        /// </summary>
        public string GetMessage()
        {
            if (Messages.Count > 0)
            {
                return Messages[0];
            }

            return null;
        }

        /// <summary>
        /// The place where the validation result is about.
        /// </summary>
        public string Location
        {
            get;
            set;
        }

        /// <summary>
        /// Returns all messages in one string. Convenient for display.
        /// </summary>
        public string Message
        {
            get
            {
                string output = string.Empty;
                Messages.ForEach(message => output += message);
                return output;
            }
        }

        /// <summary>
        /// All messages about the validity.
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();

        /// <summary>
        /// Adds a message to the list of messages.
        /// </summary>
        /// <param name="message">The message to add.</param>
        public void AddMessage(string message)
        {
            Messages.Add(message);
        }

        /// <summary>
        /// Amends a "parent" location to the start of the location.
        /// </summary>
        public void AmendLocation(string amending)
        {
            Location = amending + "." + Location;
        }

        /// <summary>
        /// Constructs an instance.
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="propertyName"></param>
        /// <param name="location"></param>
        public ValidationResult(bool isValid = false, string propertyName = null, string location = null)
        {
            IsValid = isValid;
            PropertyName = propertyName;
            Location = location;
        }
    }
}
