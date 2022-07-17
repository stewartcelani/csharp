namespace ArdalisRating
{
    public abstract class Rater
    {
        protected readonly RatingEngine _engine;
        protected readonly ConsoleLogger _logger;

        protected Rater(RatingEngine engine, ConsoleLogger logger)
        {
            _engine = engine;
            _logger = logger;
        }

        public abstract void Rate(Policy policy);
    }
}