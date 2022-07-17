namespace Tweetbook.Contracts.V1.Requests;

public class UpdatePostRequest
{
    public string Name { get; set; }

    public IEnumerable<string> Tags { get; set; }
}