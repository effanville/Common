namespace StructureCommon.Extensions
{
    /// <summary>
    /// Writes properties of an object into strings.
    /// </summary>
    public static class PropertyWriters
    {
        /// <summary>
        /// Outputs a string of the property names listed in the class <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Class one is obtaining properties from.</typeparam>
        /// <param name="objectToUse">The instance of the type <typeparamref name="T"/>.</param>
        /// <param name="separator">The string to separate the names.</param>
        public static string PropertyNames<T>(this T objectToUse, string separator) where T : class
        {
            var properties = objectToUse.GetType().GetProperties();
            string header = string.Empty;
            foreach (var property in properties)
            {
                header += property.Name;
                header += separator;
            }

            return header;
        }

        /// <summary>
        /// Outputs a string of the property values listed in the class <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Class one is obtaining properties from.</typeparam>
        /// <param name="objectToUse">The instance of the type <typeparamref name="T"/>.</param>
        /// <param name="separator">The string to separate the values.</param>
        public static string PropertyValues<T>(this T objectToUse, string separator)
        {
            var properties = objectToUse.GetType().GetProperties();
            string data = string.Empty;

            for (int i = 0; i < properties.Length; i++)
            {
                bool isDouble = double.TryParse(properties[i].GetValue(objectToUse).ToString(), out double value);
                if (isDouble)
                {
                    data += value.TruncateToString();
                }
                else
                {
                    data += properties[i].GetValue(objectToUse);
                }

                data += separator;
            }

            return data;
        }
    }
}
