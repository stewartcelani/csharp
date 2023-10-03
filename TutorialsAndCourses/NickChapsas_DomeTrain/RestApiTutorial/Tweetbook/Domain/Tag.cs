using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Tweetbook.Domain;

public class Tag
{
    [Key]
    [Column(TypeName = "VARCHAR(50)")]
    public string Name { get; set; }

    public string CreatorId { get; set; }

    [ForeignKey(nameof(CreatorId))] public virtual IdentityUser CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }
}