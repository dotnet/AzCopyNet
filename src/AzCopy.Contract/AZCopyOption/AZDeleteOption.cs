namespace Microsoft.AzCopy.Contract
{
    public class AZDeleteOption : CommandArgsBase
    {
        [CLIArgumentName("delete-snapshots", true)]
        public string DeleteSnapShots { get; set; }

        [CLIArgumentName("exclude-path", true)]
        public string ExcludePath { get; set; }

        [CLIArgumentName("exclude-pattern", true)]
        public string ExcludePattern { get; set; }

        [CLIArgumentName("include-path", true)]
        public string IncludePath { get; set; }

        [CLIArgumentName("include-pattern", true)]
        public string IncludePattern { get; set; }

        [CLIArgumentName("list-of-files", true)]
        public string ListOfFiles { get; set; }

        [CLIArgumentName("recursive")]
        public string Recursive { get; set; }
    }
}