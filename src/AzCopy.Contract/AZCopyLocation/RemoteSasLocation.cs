namespace AzCopy.Contract
{
    public class RemoteSasLocation : LocationBase
    {
        public string SasToken { get; set; }

        public string Container { get; set; }

        public string ResourceUri { get; set; }

        protected override string LocationToString()
        {
            return $"\"{this.ResourceUri}/{this.Container}/{this.Path}{this.SasToken}\"";
        }
    }
}
