using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;

namespace ArdalisRating
{
    /// <summary>
    /// The RatingEngine reads the policy application details from a file and produces a numeric 
    /// rating value based on the details.
    /// </summary>
    public class RatingEngine
    {
        public ConsoleLogger Logger { get; set; } = new ConsoleLogger();

        private readonly FilePolicySource _policySource;
        private readonly PolicySerializer _policySerializer;

        public RatingEngine()
        {
            _policySource = new FilePolicySource();
            _policySerializer = new PolicySerializer();
        }

        public RatingEngine(FilePolicySource policySource, PolicySerializer policySerializer)
        {
            _policySource = policySource;
            _policySerializer = policySerializer;
        }

        public decimal Rating { get; set; }

        public void Rate()
        {
            Logger.Log("Starting rate.");
            Logger.Log("Loading policy.");

            var policyJson = _policySource.GetPolicyFromSource();
            var policy = _policySerializer.GetPolicyFromJsonString(policyJson);

            var rater = RaterFactory.Create(policy, this);
            rater?.Rate(policy);

            Logger.Log("Rating completed.");
        }
    }
}