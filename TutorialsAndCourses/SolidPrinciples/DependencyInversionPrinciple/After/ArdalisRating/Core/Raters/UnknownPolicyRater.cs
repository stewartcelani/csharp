namespace ArdalisRating
{
    public class UnknownPolicyRater : Rater
    {

        public override decimal Rate(Policy policy)
        {
            Logger.Log("Unknown policy type");
            return 0m;
        }

        public UnknownPolicyRater(ILogger logger) : base(logger)
        {
        }
    }
}
