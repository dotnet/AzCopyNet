using System;
using System.Linq;
using System.Reflection;

namespace AzCopy.Contract
{
    public class CommandArgsBase : ICommandArgs
    {
        private const string ArgumentPrefix = "--";

        /// <summary>
        /// Builds a set of command-line flags for the training parameters, in the form "--flagname value --flagname2 value 2".
        /// </summary>
        /// <returns>serialized CLI command.</returns>
        public string ToCommandLineString()
            => string.Join(" ", this.GetType().GetProperties()
                                              .Where(property => Attribute.IsDefined(property, typeof(CLIArgumentName)) && property.GetValue(this) != null)
                                              .Select(p => this.BuildCLIArgument(p)));

        private string BuildCLIArgument(PropertyInfo property)
        {
            var cliArgName = (CLIArgumentName)property.GetCustomAttribute(typeof(CLIArgumentName));
            var argValue = property.GetValue(this);

            // handle bool value
            if (argValue is bool)
            {
                argValue = (bool)argValue ? "true" : "false";
            }

            if (cliArgName.UseQuotes)
            {
                return $"{ArgumentPrefix}{cliArgName.ArgumentName}=\"{argValue}\"";
            }
            else
            {
                return $"{ArgumentPrefix}{cliArgName.ArgumentName}={argValue}";
            }
        }
    }
}
