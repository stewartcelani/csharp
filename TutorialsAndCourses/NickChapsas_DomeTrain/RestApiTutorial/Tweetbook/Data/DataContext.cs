using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tweetbook.Domain;
using Tweetbook.Extensions;

namespace Tweetbook.Data;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<PostTag> PostTags { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*
         * PostTag many-to-many
         * By default EFCore will try to set up PostTag with ON CASCADE DELETE which causes
         * a circular error so we turn that off using an extension method
         */
        modelBuilder.Entity<PostTag>()
            .HasKey(x => new { x.PostId, x.TagName });
        modelBuilder.SetDeleteBehaviorForType<PostTag>(DeleteBehavior.Restrict);
    }
}