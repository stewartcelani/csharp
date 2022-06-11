using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tweetbook.Cache;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Requests.Queries;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Data;
using Tweetbook.Domain;
using Tweetbook.Extensions;
using Tweetbook.Helpers;
using Tweetbook.Services;

namespace Tweetbook.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPostService _postService;
    private readonly IUriService _uriService;


    public PostsController(IPostService postService, IMapper mapper, IUriService uriService,
        PaginationSettings paginationSettings)
    {
        _postService = postService;
        _mapper = mapper;
        _uriService = uriService;
    }

    [Cached(60)]
    [HttpGet(ApiRoutes.Posts.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPostsQuery query,
        [FromQuery] PaginationQuery paginationQuery)
    {
        var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
        var queryFilter = _mapper.Map<GetAllPostsFilter>(query);
        var posts = await _postService.GetPostsAsync(queryFilter, paginationFilter);
        var postsResponse = _mapper.Map<List<PostResponse>>(posts);
        var paginationResponse =
            PaginationHelpers.CreatePaginatedResponse(_uriService, paginationFilter, postsResponse);
        return Ok(paginationResponse);
    }

    [Cached(150)]
    [HttpGet(ApiRoutes.Posts.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid postId)
    {
        var post = await _postService.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        var postResponse = new Response<PostResponse>(_mapper.Map<PostResponse>(post));

        return Ok(postResponse);
    }

    [HttpPut(ApiRoutes.Posts.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
    {
        var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

        if (!userOwnsPost) return BadRequest(new { error = "You do not own this post" });

        var post = await _postService.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        post.Name = request.Name;
        post.Tags = request.Tags.Select(x => new PostTag { PostId = post.Id, TagName = x }).ToList();

        var updated = await _postService.UpdatePostAsync(post);

        if (!updated)
            return NotFound();

        var postResponse = new Response<PostResponse>(_mapper.Map<PostResponse>(post));

        return Ok(postResponse);
    }

    [HttpDelete(ApiRoutes.Posts.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid postId)
    {
        var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

        if (!userOwnsPost) return BadRequest(new { error = "You do not own this post" });

        var deleted = await _postService.DeletePostAsync(postId);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost(ApiRoutes.Posts.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
    {
        var newPostId = Guid.NewGuid();
        var post = new Post
        {
            Id = newPostId,
            Name = postRequest.Name,
            UserId = HttpContext.GetUserId(),
            Tags = postRequest.Tags.Select(x => new PostTag { PostId = newPostId, TagName = x }).ToList()
        };

        var created = await _postService.CreatePostAsync(post);

        if (!created)
            return StatusCode(500);

        var locationUri = _uriService.GetPostUri(post.Id.ToString());

        return Created(locationUri, new Response<PostCreatedResponse>(_mapper.Map<PostCreatedResponse>(post)));
    }
}