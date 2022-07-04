using Bridge;

var noCoupon = new NoCoupon();
var oneDollarCoupon = new OneDollarCoupon();

var meatBasedMenu1 = new MeatMenu(noCoupon);
var meatBasedMenu2 = new MeatMenu(oneDollarCoupon);
Console.WriteLine($"Meat based menu, no coupon: {meatBasedMenu1.CalculatePrice()} dollars");
Console.WriteLine($"Meat based menu, one dollar coupon: {meatBasedMenu2.CalculatePrice()} dollars");

var vegetarianMenu1 = new VegetarianMenu(noCoupon);
var vegetarianMenu2 = new VegetarianMenu(oneDollarCoupon);
Console.WriteLine($"Vegetarian based menu, no coupon: {vegetarianMenu1.CalculatePrice()} dollars");
Console.WriteLine($"Vegetarian based menu, one dollar coupon: {vegetarianMenu2.CalculatePrice()} dollars");