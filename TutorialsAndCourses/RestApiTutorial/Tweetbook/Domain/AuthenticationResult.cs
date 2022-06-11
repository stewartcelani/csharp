namespace Tweetbook.Domain;

public class AuthenticationResult
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public bool Success => !Errors.Any();

    public IEnumerable<string> Errors { get; init; } = new List<string>();
}