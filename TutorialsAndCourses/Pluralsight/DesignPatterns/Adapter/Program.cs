using Adapter.ObjectAdapter;
//using Adapter.ClassAdapter;

/*
 * Swap out the using statements to test Class or Object adapters, both produce "Adelaide - Radelaide, 1000000"
 */
ICityAdapter adapter = new CityAdapter();
var city = adapter.GetCity();

Console.WriteLine($"{city.FullName}, {city.Inhabitants}");
