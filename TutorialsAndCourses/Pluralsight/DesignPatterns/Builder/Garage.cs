namespace Builder;

public class Garage
{
    private CarBuilder? _builder;
    
    public Garage()
    {
    }

    public void Construct(CarBuilder builder)
    {
        _builder = builder;
        
        _builder.BuildEngine();
        _builder.BuildFrame();
    }
}