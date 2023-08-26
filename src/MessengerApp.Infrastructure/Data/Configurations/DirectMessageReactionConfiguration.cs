using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class DirectMessageReactionConfiguration : IEntityTypeConfiguration<DirectMessageReaction>
{
    public void Configure(EntityTypeBuilder<DirectMessageReaction> builder)
    {
        builder.HasOne(reaction => reaction.User)
            .WithMany()
            .HasForeignKey(reaction => reaction.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}