using Builder;

var garage = new Garage();

var miniBuilder = new MiniBuilder();
var bwmBuilder = new BWMBuilder();

garage.Construct(miniBuilder);
Console.WriteLine(miniBuilder.Car.ToString());

garage.Construct(bwmBuilder);
Console.WriteLine(bwmBuilder.Car.ToString());

Console.ReadKey();