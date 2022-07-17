namespace Builder;

public class BWMBuilder : CarBuilder
{
    public BWMBuilder() : base("BMW")
    {
    }

    public override void BuildEngine()
    {
        Car.AddPart("Fancy V8 Engine");
    }

    public override void BuildFrame()
    {
        Car.AddPart("5-door with metallic finish");
    }
}