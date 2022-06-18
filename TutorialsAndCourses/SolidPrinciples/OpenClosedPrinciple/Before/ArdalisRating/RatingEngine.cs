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
        private readonly ConsoleLogger _logger;
        private readonly FilePolicySource _policySource;
        private readonly PolicySerializer _policySerializer;

        public RatingEngine()
        {
            _logger = new ConsoleLogger();
            _policySource = new FilePolicySource();
            _policySerializer = new PolicySerializer();
        }

        public RatingEngine(ConsoleLogger logger, FilePolicySource policySource, PolicySerializer policySerializer)
        {
            _logger = logger;
            _policySource = policySource;
            _policySerializer = policySerializer;
        }

        public decimal Rating { get; set; }

        public void Rate()
        {
            _logger.Log("Starting rate.");
            _logger.Log("Loading policy.");

            var policyJson = _policySource.GetPolicyFromSource();
            var policy = _policySerializer.GetPolicyFromJsonString(policyJson);

            switch (policy.Type)
            {
                case PolicyType.Auto:
                    _logger.Log("Rating AUTO policy...");
                    _logger.Log("Validating policy.");
                    if (String.IsNullOrEmpty(policy.Make))
                    {
                        _logger.Log("Auto policy must specify Make");
                        return;
                    }

                    if (policy.Make == "BMW")
                    {
                        if (policy.Deductible < 500)
                        {
                            Rating = 1000m;
                        }

                        Rating = 900m;
                    }

                    break;

                case PolicyType.Land:
                    _logger.Log("Rating LAND policy...");
                    _logger.Log("Validating policy.");
                    if (policy.BondAmount == 0 || policy.Valuation == 0)
                    {
                        _logger.Log("Land policy must specify Bond Amount and Valuation.");
                        return;
                    }

                    if (policy.BondAmount < 0.8m * policy.Valuation)
                    {
                        _logger.Log("Insufficient bond amount.");
                        return;
                    }

                    Rating = policy.BondAmount * 0.05m;
                    break;

                case PolicyType.Life:
                    _logger.Log("Rating LIFE policy...");
                    _logger.Log("Validating policy.");
                    if (policy.DateOfBirth == DateTime.MinValue)
                    {
                        _logger.Log("Life policy must include Date of Birth.");
                        return;
                    }

                    if (policy.DateOfBirth < DateTime.Today.AddYears(-100))
                    {
                        _logger.Log("Centenarians are not eligible for coverage.");
                        return;
                    }

                    if (policy.Amount == 0)
                    {
                        _logger.Log("Life policy must include an Amount.");
                        return;
                    }

                    int age = DateTime.Today.Year - policy.DateOfBirth.Year;
                    if (policy.DateOfBirth.Month == DateTime.Today.Month &&
                        DateTime.Today.Day < policy.DateOfBirth.Day ||
                        DateTime.Today.Month < policy.DateOfBirth.Month)
                    {
                        age--;
                    }

                    decimal baseRate = policy.Amount * age / 200;
                    if (policy.IsSmoker)
                    {
                        Rating = baseRate * 2;
                        break;
                    }

                    Rating = baseRate;
                    break;

                default:
                    _logger.Log("Unknown policy type");
                    break;
            }

            _logger.Log("Rating completed.");
        }
    }
}