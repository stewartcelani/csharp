using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

var server = WireMockServer.Start();

server.Given(Request.Create()
    .WithPath("/ping")
    .UsingGet())
    .RespondWith(Response.Create().WithStatusCode(200));

Console.WriteLine($"Server listening on: {server.Url}");

Console.ReadKey();