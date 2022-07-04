using Decorator;

/*
 * Default behavior
 */
var cloudMailService = new CloudMailService();
cloudMailService.SendMail("Hi there.");
Console.WriteLine();

var onPremiseMailService = new OnPremiseMailService();
onPremiseMailService.SendMail("Hi there.");
Console.WriteLine();

/*
 * Adding behavior with decorators
 */
var statisticsDecorator = new StatisticsDecorator(cloudMailService);
statisticsDecorator.SendMail($"Hi there via {nameof(StatisticsDecorator)} wrapper.");
Console.WriteLine();

var messageDatabaseDecorator = new MessageDatabaseDecorator(onPremiseMailService);
messageDatabaseDecorator.SendMail($"Hi there via {nameof(MessageDatabaseDecorator)} wrapper.");
Console.WriteLine($"Sent messages: {messageDatabaseDecorator.SentMessages.Count}");

Console.ReadKey();