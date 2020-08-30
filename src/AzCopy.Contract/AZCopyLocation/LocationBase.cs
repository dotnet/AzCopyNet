namespace AzCopy.Contract
{
    public abstract class LocationBase
    {
        public string Path { get; set; }

        public bool UseWildCard { get; set; }

        public override string ToString()
        {
            return this.LocationToString();
        }

        protected abstract string LocationToString();
    }
}
