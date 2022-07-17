using TemplateMethod;

ExchangeMailParser exchangeMailParser = new();
Console.WriteLine(exchangeMailParser.ParseMailBody(Guid.NewGuid().ToString()));
Console.WriteLine();

ApacheMailParser apacheMailParser = new();
Console.WriteLine(apacheMailParser.ParseMailBody(Guid.NewGuid().ToString()));
Console.WriteLine();

EudoraMailParser eudoraMailParser = new();
Console.WriteLine(eudoraMailParser.ParseMailBody(Guid.NewGuid().ToString()));