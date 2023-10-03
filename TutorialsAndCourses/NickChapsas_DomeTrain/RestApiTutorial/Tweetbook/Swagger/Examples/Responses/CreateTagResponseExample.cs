using Swashbuckle.AspNetCore.Filters;
using Tweetbook.Contracts.V1.Responses;

namespace Tweetbook.SwaggerExamples.Responses;

public class CreateTagResponseExample : IExamplesProvider<TagResponse>
{
    public TagResponse GetExamples()
    {
        return new TagResponse
        {
            Name = "example tag"
        };
    }
}