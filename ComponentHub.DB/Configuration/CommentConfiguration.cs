using ComponentHub.Domain.Features.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentHub.DB.Configuration;

internal sealed class CommentConfiguration: IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(comment => comment.Id);
    }
}