namespace Microsoft.AzCopy.Contract
{
    public abstract class AZCopyMessageBase
    {
        // "Info" | "Init" | "Progress" | "Exit" | "Error" | "Prompt"
        public abstract string MessageType { get; }

        public string MessageContent { get; set; }

        public string TimeStamp { get; set; }

        // should always be empty
        public string PromptDetails { get; set; }
    }
}