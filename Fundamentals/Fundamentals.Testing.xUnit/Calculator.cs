namespace Fundamentals.Testing.xUnit;

public class Calculator : ICalculator
{
    public decimal Value { get; private set; } = 0;

    public void SetValue(decimal x)
    {
        Value = x;
    }
    
    public void Add(decimal x)
    {
        Value += x;
    }

    public void Multiply(decimal x)
    {
        Value *= x;
    }
    
    public void Multiply(decimal x, decimal y)
    {
        Value = x * y;
    }

    public void Divide(decimal x)
    {
        throw new NotImplementedException();
    }

    public void Divide(decimal x, decimal y)
    {
        throw new NotImplementedException();
    }
}