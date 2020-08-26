namespace AzCopy.Contract
{
    public class LocalLocation : IAZCopyLocation
    {
        public string Path { get; set; }

        public bool UseWildCard { get; set; }

        public string LocationToString()
        {
            return this.Path + (this.UseWildCard ? "*" : string.Empty);
        }
    }
}
