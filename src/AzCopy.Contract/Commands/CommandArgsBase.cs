using System;
using System.Linq;
using System.Reflection;

namespace AzCopy.Contract
{
    public class CommandArgsBase
    {
        private const string ArgumentPrefix = "--";

        public override string ToString()
        {
            return string.Join(" ", this.GetType().GetProperties()
                                              .Where(property => Attribute.IsDefined(property, typeof(CLIArgumentName)) && property.GetValue(this) != null)
                                              .Select(p => this.BuildCLIArgument(p)));
        }

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
