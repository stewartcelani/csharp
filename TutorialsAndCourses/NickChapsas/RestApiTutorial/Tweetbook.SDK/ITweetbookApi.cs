using Refit;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;

namespace Tweetbook.SDK;

[Headers("Authorization: Bearer")]
public interface ITweetbookApi
{
    [Get("/posts")]
    Task<ApiResponse<List<PostResponse>>> GetAllAsync();

    [Get("/posts/{postId}")]
    Task<ApiResponse<PostResponse>> GetAsync(Guid postId);

    [Post("/posts")]
    Task<ApiResponse<PostCreatedResponse>> CreateAsync([Body] CreatePostRequest createPostRequest);

    [Put("/posts/{postId}")]
    Task<ApiResponse<PostResponse>> UpdateAsync(Guid postId, [Body] UpdatePostRequest updatePostRequest);

    [Delete("/posts/{postId}")]
    Task<ApiResponse<string>> DeleteAsync(Guid postId);
}