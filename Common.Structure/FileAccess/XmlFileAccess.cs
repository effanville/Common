﻿using System;
using System.IO;
using System.IO.Abstractions;
using System.Xml.Serialization;

namespace Common.Structure.FileAccess
{
    /// <summary>
    /// Contains generic type routines for writing and reading from xml file.
    /// </summary>
    public static class XmlFileAccess
    {
        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="error">String containing any error that may have occurred.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, out string error, bool append = false) where T : new()
        {
            WriteToXmlFile(new FileSystem(), filePath, objectToWrite, out error, append);
        }
        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="fileSystem">The filesystem to use to write from.</param>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="error">String containing any error that may have occurred.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>(IFileSystem fileSystem, string filePath, T objectToWrite, out string error, bool append = false) where T : new()
        {
            error = null;
            try
            {
                using (Stream stream = fileSystem.FileStream.Create(filePath, append ? FileMode.Append : FileMode.Create))
                using (TextWriter writer = new StreamWriter(stream))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, objectToWrite);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message + " " + ex.InnerException ?? ex.InnerException.Message;
                return;
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <param name="error">Any error message reporting any issues.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(string filePath, out string error) where T : new()
        {
            return ReadFromXmlFile<T>(new FileSystem(), filePath, out error);
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="fileSystem">The filesystem to use to read from.</param>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <param name="error">Any error message reporting any issues.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(IFileSystem fileSystem, string filePath, out string error) where T : new()
        {
            error = null;
            try
            {
                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Open))
                using (TextReader reader = new StreamReader(stream))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return default(T);
            }
        }
    }
}