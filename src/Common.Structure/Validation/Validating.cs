using System;

namespace Common.Structure.Validation
{
    /// <summary>
    /// Contains standard validation routines.
    /// </summary>
    public static class Validating
    {
        /// <summary>
        /// Provides validation on a value being not negative ( i.e. x \geq 0).
        /// </summary>
        /// <param name="value">The value x to validate.</param>
        /// <param name="propertyName">The metadata name of the property the value represents.</param>
        /// <param name="location">The location where the validation takes place.</param>
        /// <returns>A validation result in the case that the value is negative, specifying the property and the location from the location and property.</returns>
        public static ValidationResult NotNegative(double value, string propertyName, string location)
        {
            if (value < 0)
            {
                ValidationResult notNegativeResult = new ValidationResult(isValid: false, propertyName, location);
                notNegativeResult.AddMessage($"{propertyName} cannot take a negative value.");
                return notNegativeResult;
            }

            return null;
        }

        /// <summary>
        /// Provides validation on a value being smaller than a value ( i.e. x \geq a).
        /// </summary>
        /// <param name="value">The value x to validate.</param>
        /// <param name="lowerLimit">The lower value a.</param>
        /// <param name="propertyName">The metadata name of the property the value represents.</param>
        /// <param name="location">The location where the validation takes place.</param>
        /// <returns>A validation result in the case that the value is below the lowerLimit, specifying the property and the location from the location and property.</returns>
        public static ValidationResult NotLessThan(double value, double lowerLimit, string propertyName, string location)
        {
            if (value < lowerLimit)
            {
                ValidationResult notLessThanResult = new ValidationResult(isValid: false, propertyName, location);
                notLessThanResult.AddMessage($"{propertyName} cannot take values below {lowerLimit}.");
                return notLessThanResult;
            }

            return null;
        }

        /// <summary>
        /// Provides validation on a value being smaller than a value ( i.e. x \geq a).
        /// </summary>
        /// <param name="value">The value x to validate.</param>
        /// <param name="lowerLimit">The lower value a.</param>
        /// <param name="propertyName">The metadata name of the property the value represents.</param>
        /// <param name="location">The location where the validation takes place.</param>
        /// <returns>A validation result in the case that the value is below the lowerLimit, specifying the property and the location from the location and property.</returns>
        public static ValidationResult NotLessThan<T>(T value, T lowerLimit, string propertyName, string location)
            where T : IComparable<T>
        {
            if (value.CompareTo(lowerLimit) < 0)
            {
                ValidationResult notLessThanResult = new ValidationResult(isValid: false, propertyName, location);
                notLessThanResult.AddMessage($"{propertyName} cannot take values below {lowerLimit}.");
                return notLessThanResult;
            }

            return null;
        }

        /// <summary>
        /// Provides validation on a value being larger than a value ( i.e. x \leq a).
        /// </summary>
        /// <param name="value">The value x to validate.</param>
        /// <param name="upperLimit">The upper value a.</param>
        /// <param name="propertyName">The metadata name of the property the value represents.</param>
        /// <param name="location">The location where the validation takes place.</param>
        /// <returns>A validation result in the case that the value is above the upperLimit, specifying the property and the location from the location and property.</returns>
        public static ValidationResult NotGreaterThan(double value, double upperLimit, string propertyName, string location)
        {
            if (value > upperLimit)
            {
                ValidationResult notMoreThanResult = new ValidationResult(isValid: false, propertyName, location);
                notMoreThanResult.AddMessage($"{propertyName} cannot take values above {upperLimit}.");
                return notMoreThanResult;
            }

            return null;
        }

        /// <summary>
        /// Provides validation on a value being larger than a value ( i.e. x \leq a).
        /// </summary>
        /// <param name="value">The value x to validate.</param>
        /// <param name="upperLimit">The upper value a.</param>
        /// <param name="propertyName">The metadata name of the property the value represents.</param>
        /// <param name="location">The location where the validation takes place.</param>
        /// <returns>A validation result in the case that the value is above the upperLimit, specifying the property and the location from the location and property.</returns>
        public static ValidationResult NotGreaterThan<T>(T value, T upperLimit, string propertyName, string location)
            where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) > 0)
            {
                ValidationResult notMoreThanResult = new ValidationResult(isValid: false, propertyName, location);
                notMoreThanResult.AddMessage($"{propertyName} cannot take values above {upperLimit}.");
                return notMoreThanResult;
            }

            return null;
        }

        /// <summary>
        /// Provides validation on a value not being equal to another ( i.e. x \neq a).
        /// </summary>
        /// <param name="value">The value x to validate.</param>
        /// <param name="expected">The expected value to equal a.</param>
        /// <param name="propertyName">The metadata name of the property the value represents.</param>
        /// <param name="location">The location where the validation takes place.</param>
        /// <returns>A validation result in the case that the value is not equal to the expected, specifying the property and the location from the location and property.</returns>
        public static ValidationResult NotEqualTo(double value, double expected, string propertyName, string location)
        {
            if (!value.Equals(expected))
            {
                ValidationResult notEqualTo = new ValidationResult(isValid: false, propertyName, location);
                notEqualTo.AddMessage($"{propertyName} was expected to be equal to {expected}.");
                return notEqualTo;
            }

            return null;
        }

        /// <summary>
        /// Provides validation on a value not being equal to another ( i.e. x \neq a).
        /// </summary>
        /// <param name="value">The value x to validate.</param>
        /// <param name="expected">The expected value to equal a.</param>
        /// <param name="propertyName">The metadata name of the property the value represents.</param>
        /// <param name="location">The location where the validation takes place.</param>
        /// <returns>A validation result in the case that the value is not equal to the expected, specifying the property and the location from the location and property.</returns>
        public static ValidationResult NotEqualTo<T>(T value, T expected, string propertyName, string location)
        {
            if (!value.Equals(expected))
            {
                ValidationResult notEqualTo = new ValidationResult(isValid: false, propertyName, location);
                notEqualTo.AddMessage($"{propertyName} was expected to be equal to {expected}.");
                return notEqualTo;
            }

            return null;
        }

        /// <summary>
        /// Provides validation on a value being not null or empty.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The metadata name of the property the value represents.</param>
        /// <param name="location">The location where the validation takes place.</param>
        /// <returns>A validation result in the case that the value is null or empty, specifying the property and the location from the location and property.</returns>
        public static ValidationResult IsNotNullOrEmpty(string value, string propertyName, string location)
        {
            if (string.IsNullOrEmpty(value))
            {
                ValidationResult result = new ValidationResult(isValid: false, propertyName, location);
                result.AddMessage($"{propertyName} cannot be empty or null.");
                return result;
            }

            return null;
        }

        /// <summary>
        /// Provides validation on a value being different from the default for that type.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to assert as not default.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="location">The location where the validation takes place.</param>
        /// <returns>A validation result in the case that the value is the default, specifying the property and the location from the location and property.</returns>
        public static ValidationResult IsNotDefault<T>(T value, string propertyName, string location) where T : struct
        {
            if (value.Equals(default(T)))
            {
                ValidationResult result = new ValidationResult(isValid: false, propertyName, location);
                result.AddMessage($"{propertyName} cannot be the default value.");
                return result;
            }

            return null;
        }
    }
}
