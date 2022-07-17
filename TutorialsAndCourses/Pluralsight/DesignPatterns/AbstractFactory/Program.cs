using AbstractFactory.Cart;
using AbstractFactory.ShoppingCart;

var belgiumShoppingCartPurchaseFactory = new BelgiumShoppingCartPurchaseFactory();
var shoppingCartForBelgium = new ShoppingCart(belgiumShoppingCartPurchaseFactory);
shoppingCartForBelgium.CalculateCosts();

var australianShoppingCartPurchaseFactory = new AustralianShoppingCartPurchaseFactory();
var shoppingCartForAustralia = new ShoppingCart(australianShoppingCartPurchaseFactory);
shoppingCartForAustralia.CalculateCosts();

Console.ReadKey();