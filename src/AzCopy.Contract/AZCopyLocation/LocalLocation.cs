namespace AzCopy.Contract
{
    public class LocalLocation : LocationBase
    {
        protected override string LocationToString()
        {
            return $"\"{this.Path}{(this.UseWildCard ? "*" : string.Empty)}\"";
        }
    }
}
