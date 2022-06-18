using System;

namespace ArdalisRating
{
    public static class RaterFactory
    {
        /*public static Rater Create(Policy policy, RatingEngine engine)
        {
            switch (policy.Type)
            {
                case PolicyType.Auto:
                    return new AutoPolicyRater(engine, engine.Logger);
                case PolicyType.Land:
                    return new LandPolicyRater(engine, engine.Logger);
                case PolicyType.Life:
                    return new LifePolicyRater(engine, engine.Logger);
                default:
                    // TODO: Implement Null Object Pattern
                    throw new ArgumentException(nameof(policy.Type));
            }
        }*/

        // More advanced method applying OCP to the Factory itself using reflection
        public static Rater Create(Policy policy, RatingEngine engine)
        {
            try
            {
                return (Rater)Activator.CreateInstance(Type.GetType($"ArdalisRating.{policy.Type}PolicyRater"),
                    new object[] { engine, engine.Logger });
            }
            catch
            {
                return null;
            }
        }
    }
}