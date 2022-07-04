namespace Facade;

public class DayOfTheWeekFactorService
{
    public double CalculateDayOfTheWeekFactor()
    {
        switch (DateTime.UtcNow.DayOfWeek)
        {
            case DayOfWeek.Saturday:
            case DayOfWeek.Sunday:
                return 0.8;
            default:
                return 1.2;
        }
    }
}