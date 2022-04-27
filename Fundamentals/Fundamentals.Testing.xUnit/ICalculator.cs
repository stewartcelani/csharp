namespace Fundamentals.Testing.xUnit;

public interface ICalculator
{
    public void SetValue(decimal x);
    public void Add(decimal x);
    public void Multiply(decimal x);
    public void Multiply(decimal x, decimal y);
    public void Divide(decimal x);
    public void Divide(decimal x, decimal y);
}

