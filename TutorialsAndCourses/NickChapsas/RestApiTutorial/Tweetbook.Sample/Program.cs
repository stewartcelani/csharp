using Refit;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.SDK;

var cachedToken = string.Empty;

const string hostUrl = "https://localhost:8001/api/v1";

var identityApi = RestService.For<IIdentityApi>(hostUrl);
var tweetbookApi = RestService.For<ITweetbookApi>(hostUrl, new RefitSettings
{
    AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
});

var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
{
    Email = "testingsdk@mcdonalds.com",
    Password = "Password1!"
});

var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
{
    Email = "testingsdk@mcdonalds.com",
    Password = "Password1!"
});

cachedToken = loginResponse.Content?.Token ?? string.Empty;

var allPosts = await tweetbookApi.GetAllAsync();

var createdPost = await tweetbookApi.CreateAsync(new CreatePostRequest
{
    Name = "This is created by the SDK",
    Tags = new[] { "sdk", "refit" }
});

var retrievedPost = await tweetbookApi.GetAsync(createdPost.Content.Id);

var updatedPost = await tweetbookApi.UpdateAsync(retrievedPost.Content.Id, new UpdatePostRequest
{
    Name = "This is updated by the SDK",
    Tags = new[] { "sdk", "refit", "updated" }
});


Console.ReadLine();