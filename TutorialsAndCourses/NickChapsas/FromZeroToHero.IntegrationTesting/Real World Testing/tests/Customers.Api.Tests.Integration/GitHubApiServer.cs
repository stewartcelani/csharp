using System;
using System.Diagnostics.CodeAnalysis;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Customers.Api.Tests.Integration;

[ExcludeFromCodeCoverage]
public class GitHubApiServer : IDisposable
{
    private WireMockServer _server;

    public string Url => _server.Url!;

    public void Start()
    {
        _server = WireMockServer.Start();
    }

    public void SetupThrottledUser(string username)
    {
        _server.Given(Request.Create()
                .WithPath($"/users/{username}")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(403));
    }

    public void SetupUser(string username)
    {
        _server.Given(Request.Create()
                .WithPath($"/users/{username}")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBodyAsJson(GenerateGitHubUserResponseBody(username))
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));
    }

    public void Dispose()
    {
        _server.Stop();
        _server.Dispose();
    }
    
     private static string GenerateGitHubUserResponseBody(string username)
    {
        return $@"{{
                  ""login"": ""{username}"",
                  ""id"": 391792,
                  ""node_id"": ""MDQ6VXNlcjM5MTc5Mg=="",
                  ""avatar_url"": ""https://avatars.githubusercontent.com/u/391792?v=4"",
                  ""gravatar_id"": """",
                  ""url"": ""https://api.github.com/users/{username}"",
                  ""html_url"": ""https://github.com/{username}"",
                  ""followers_url"": ""https://api.github.com/users/{username}/followers"",
                  ""following_url"": ""https://api.github.com/users/{username}/following{{/other_user}}"",
                  ""gists_url"": ""https://api.github.com/users/{username}/gists{{/gist_id}}"",
                  ""starred_url"": ""https://api.github.com/users/{username}/starred{{/owner}}{{/repo}}"",
                  ""subscriptions_url"": ""https://api.github.com/users/{username}/subscriptions"",
                  ""organizations_url"": ""https://api.github.com/users/{username}/orgs"",
                  ""repos_url"": ""https://api.github.com/users/{username}/repos"",
                  ""events_url"": ""https://api.github.com/users/{username}/events{{/privacy}}"",
                  ""received_events_url"": ""https://api.github.com/users/{username}/received_events"",
                  ""type"": ""User"",
                  ""site_admin"": false,
                  ""name"": ""Name"",
                  ""company"": null,
                  ""blog"": """",
                  ""location"": null,
                  ""email"": null,
                  ""hireable"": null,
                  ""bio"": null,
                  ""twitter_username"": null,
                  ""public_repos"": {new Random().Next(1, 100)},
                  ""public_gists"": {new Random().Next(1, 100)},
                  ""followers"": {new Random().Next(1, 100)},
                  ""following"": {new Random().Next(1, 100)},
                  ""created_at"": ""2010-09-08T09:52:23Z"",
                  ""updated_at"": ""2022-07-02T10:08:04Z""
                }}";
    }
}