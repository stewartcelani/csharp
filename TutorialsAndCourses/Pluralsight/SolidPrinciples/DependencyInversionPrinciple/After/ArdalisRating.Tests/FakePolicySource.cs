namespace ArdalisRating
{
    public class FakePolicySource : IPolicySource
    {
        public string PolicyString { get; set; } = string.Empty;
        public string GetPolicyFromSource()
        {
            return PolicyString;
        }
    }
}