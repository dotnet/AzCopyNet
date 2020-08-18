using System;
using Microsoft.AzCopy.Contract;
using Newtonsoft.Json.Linq;

namespace Microsoft.AzCopy
{
    public static class AZCopyMessageFactory
    {
        public static AZCopyMessageBase CreateFromJson(string json)
        {
            try
            {
                var jToken = JValue.Parse(json);
                var type = jToken.Value<string>("MessageType");
                switch (type)
                {
                    case "Info":
                        return new AZCopyInfoMessage().FromJson(jToken);
                    case "Init":
                        return new AZCopyInitMessage().FromJson(jToken);
                    case "Error":
                        return new AZCopyErrorMessage().FromJson(jToken);
                    case "Progress":
                        return new AZCopyProgressMessage().FromJson(jToken);
                    case "EndOfJob":
                        return new AZCopyEndOfJobMessage().FromJson(jToken);
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception)
            {
                // If there is an error parsing the json, just return the json directly instead of an error
                return new AZCopyStringMessage(json);
            }
        }
    }
}
