using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArdalisRating
{
    public class PolicySerializer
    {
        public Policy GetPolicyFromJsonString(string policyJson) => JsonConvert.DeserializeObject<Policy>(policyJson,
            new StringEnumConverter());
    }
}