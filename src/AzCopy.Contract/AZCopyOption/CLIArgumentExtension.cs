using System;
using System.Reflection;

namespace Microsoft.AzCopy.Contract
{
    public static class CLIArgumentExtension
    {
        /// <summary>
        /// The characters that separate a flag.
        /// </summary>
        private const string FlagIndicator = "--";

        /// <summary>
        /// Retrieves the name of the <paramref name="property"/> as a command line flag, in the form "--flagname". "flagname" is set by the CLIArgumentName attribute.
        /// </summary>
        /// <param name="property">proprty.</param>
        /// <returns>string.</returns>
        public static string GetPropertyAsCLIFlag(this PropertyInfo property)
        {
            string flagName = ((CLIArgumentName)property.GetCustomAttribute(typeof(CLIArgumentName))).ArgumentName;
            return FlagIndicator + flagName;
        }

        public static string GetFlagValue(this PropertyInfo property, object obj)
        {
            var value = property.GetValue(obj);
            if (value == null)
            {
                return null;
            }

            value = value.ToString();

            if (((CLIArgumentName)property.GetCustomAttribute(typeof(CLIArgumentName))).UseQuotes)
            {
                value = string.Format("\"{0}\"", value);
            }

            return (string)value;
        }

        /// <summary>
        /// Returns whether or not the property should be part of the command line arguments.
        /// </summary>
        /// <param name="property">proprty.</param>
        /// <returns> bool. </returns>
        public static bool IsCommandLineArgument(this PropertyInfo property) => Attribute.IsDefined(property, typeof(CLIArgumentName));
    }
}
