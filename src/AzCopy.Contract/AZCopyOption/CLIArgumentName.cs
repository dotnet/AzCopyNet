namespace Microsoft.AzCopy.Contract
{
    public class CLIArgumentName : System.Attribute
    {
        public CLIArgumentName(string name, bool useQuotes = false)
        {
            this.ArgumentName = name;
            this.UseQuotes = useQuotes;
        }

        public string ArgumentName { get; private set; }

        public bool UseQuotes { get; private set; }
    }
}
