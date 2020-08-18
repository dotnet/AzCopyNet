namespace Microsoft.AzCopy.Contract
{
    public interface IAZCopyLocation
    {
        string Path { get; set; }

        bool UseWildCard { get; set; }

        string LocationToString();
    }
}
