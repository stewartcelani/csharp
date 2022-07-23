// See https://aka.ms/new-console-template for more information

using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

var wiremockServer = WireMockServer.Start();

Console.WriteLine($"Wiremock is now running on : {wiremockServer.Url}");

wiremockServer.Given(Request.Create()
    .WithPath("example")
    .UsingGet())
    .RespondWith(Response.Create()
        .WithBody("This is coming from WireMock")
        .WithStatusCode(200));
        
Console.ReadKey();
wiremockServer.Dispose();