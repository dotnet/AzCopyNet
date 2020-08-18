namespace Microsoft.AzCopy.Contract
{
    public class RemoteSasLocation : IAZCopyLocation
    {
        public string Path { get; set; }

        public bool UseWildCard { get; set; }

        public string SasToken { get; set; }

        public string Container { get; set; }

        public string ResourceUri { get; set; }

        public string LocationToString()
        {
            return $"{this.ResourceUri}/{this.Container}/{this.Path}{this.SasToken}";
        }
    }
}
