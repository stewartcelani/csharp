using System.IO;

namespace ArdalisRating
{
    public class FilePolicySource
    {
        public string GetPolicyFromSource() => File.ReadAllText("policy.json");
    }
}