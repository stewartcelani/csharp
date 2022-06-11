using Microsoft.EntityFrameworkCore;
using Tweetbook.Data;
using Tweetbook.Domain;

namespace Tweetbook.Services;

public class PostService : IPostService
{
    private readonly DataContext _dataContext;

    public PostService(DataContext dataContext, PaginationSettings paginationSettings)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Post>> GetPostsAsync(GetAllPostsFilter? queryFilter = null,
        PaginationFilter? paginationFilter = null)
    {
        var queryable = GetPostsQueryableForQueryFilter(queryFilter);

        if (paginationFilter is null) return await queryable.Include(x => x.Tags).ToListAsync();

        var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

        return await queryable.Include(x => x.Tags)
            .Skip(skip)
            .Take(paginationFilter.PageSize)
            .ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(Guid postId)
    {
        return await _dataContext.Posts
            .Include(x => x.Tags)
            .SingleOrDefaultAsync(x => x.Id == postId);
    }

    public async Task<bool> CreatePostAsync(Post post)
    {
        post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

        await AddNewTags(post);
        await _dataContext.Posts.AddAsync(post);

        var created = await _dataContext.SaveChangesAsync();
        return created > 0;
    }

    public async Task<bool> UpdatePostAsync(Post postToUpdate)
    {
        postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
        await AddNewTags(postToUpdate);
        _dataContext.Posts.Update(postToUpdate);
        var updated = await _dataContext.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<bool> DeletePostAsync(Guid postId)
    {
        var post = new Post { Id = postId };
        _dataContext.Posts.Attach(post);
        _dataContext.Posts.Remove(post);

        var deleted = 0;
        try
        {
            deleted = await _dataContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            // ignored
        }

        return deleted > 0;
    }

    public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
    {
        var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);

        if (post is null) return false;

        return post.UserId == userId;
    }

    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _dataContext.Tags.AsNoTracking().ToListAsync();
    }

    public async Task<bool> CreateTagAsync(Tag tag)
    {
        tag.Name = tag.Name.ToLower();
        var existingTag = await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tag.Name);
        if (existingTag is not null)
            return true;

        await _dataContext.Tags.AddAsync(tag);
        var created = await _dataContext.SaveChangesAsync();
        return created > 0;
    }

    public async Task<Tag?> GetTagByNameAsync(string tagName)
    {
        return await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName.ToLower());
    }

    public async Task<bool> DeleteTagAsync(string tagName)
    {
        var tag = await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName.ToLower());

        if (tag is null)
            return true;

        var postTags = await _dataContext.PostTags.Where(x => x.TagName == tagName.ToLower()).ToListAsync();

        _dataContext.PostTags.RemoveRange(postTags);
        _dataContext.Tags.Remove(tag);
        return await _dataContext.SaveChangesAsync() > postTags.Count;
    }

    private async Task AddNewTags(Post post)
    {
        foreach (var postTag in post.Tags)
        {
            var existingTag =
                await _dataContext.Tags.SingleOrDefaultAsync(x =>
                    x.Name == postTag.TagName);
            if (existingTag != null)
                continue;

            await _dataContext.Tags.AddAsync(new Tag
                { Name = postTag.TagName, CreatedOn = DateTime.UtcNow, CreatorId = post.UserId });
        }
    }

    private IQueryable<Post> GetPostsQueryableForQueryFilter(GetAllPostsFilter? queryFilter = null)
    {
        var queryable = _dataContext.Posts.AsQueryable();

        if (queryFilter is not null)
            if (!string.IsNullOrEmpty(queryFilter.UserId))
                queryable = queryable.Where(x => x.UserId == queryFilter.UserId);

        return queryable;
    }
}