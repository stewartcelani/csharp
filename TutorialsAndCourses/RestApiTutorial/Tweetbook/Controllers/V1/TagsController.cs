using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Extensions;
using Tweetbook.Services;

namespace Tweetbook.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPostService _postService;

    public TagsController(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    /// <summary>
    ///     Returns all the tags in the system
    /// </summary>
    /// <response code="200">Returns all the tags in the system</response>
    [HttpGet(ApiRoutes.Tags.GetAll)]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetAll()
    {
        var tags = await _postService.GetAllTagsAsync();
        var tagResponses = new Response<IEnumerable<TagResponse>>(_mapper.Map<IEnumerable<TagResponse>>(tags));
        return Ok(tagResponses);
    }

    [HttpGet(ApiRoutes.Tags.Get)]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Get([FromRoute] string tagName)
    {
        var tag = await _postService.GetTagByNameAsync(tagName);

        if (tag is null)
            return NotFound();

        var tagResponse = new Response<TagResponse>(_mapper.Map<TagResponse>(tag));

        return Ok(tagResponse);
    }

    /// <summary>
    ///     Creates a tag in the system
    /// </summary>
    /// <response code="201">Returns the created tag</response>
    /// <response code="400">Unable to create the tag due to validation error</response>
    /// <response code="401">User does not have a valid access token</response>
    /// <response code="403">User does not have the appropriate role</response>
    /// <response code="500">Internal server error</response>
    [Authorize(Roles = "Admin,User")]
    [HttpPost(ApiRoutes.Tags.Create)]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Create([FromBody] CreateTagRequest createTagRequest)
    {
        var newTag = new Tag
        {
            Name = createTagRequest.TagName,
            CreatorId = HttpContext.GetUserId(),
            CreatedOn = DateTime.UtcNow
        };

        var created = await _postService.CreateTagAsync(newTag);
        if (!created)
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Unable to create tag" });

        var locationUri = HttpContext.GetBaseUrl() + "/" + ApiRoutes.Tags.Get.Replace("{tagName}", newTag.Name);

        var tagResponse = new TagResponse { Name = newTag.Name };

        return Created(locationUri, new Response<TagResponse>(tagResponse));
    }

    [HttpDelete(ApiRoutes.Tags.Delete)]
    [Authorize(Policy = "MustWorkForMcDonalds")]
    public async Task<IActionResult> Delete([FromRoute] string tagName)
    {
        var deleted = await _postService.DeleteTagAsync(tagName);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}