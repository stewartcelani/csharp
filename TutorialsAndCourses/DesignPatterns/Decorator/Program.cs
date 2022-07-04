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
messageDatabaseDecorator.SendMail($"Hi there via {nameof(MessageDatabaseDecorator)} wrapper, message 1.");
messageDatabaseDecorator.SendMail($"Hi there via {nameof(MessageDatabaseDecorator)} wrapper, message 2.");
Console.WriteLine($"Sent messages via {nameof(MessageDatabaseDecorator)} wrapper: {messageDatabaseDecorator.SentMessages.Count}");

Console.ReadKey();