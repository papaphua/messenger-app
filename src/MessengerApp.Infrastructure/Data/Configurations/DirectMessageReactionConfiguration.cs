using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class DirectMessageReactionConfiguration : IEntityTypeConfiguration<DirectReaction>
{
    public void Configure(EntityTypeBuilder<DirectReaction> builder)
    {
        builder.HasOne(reaction => reaction.User)
            .WithMany()
            .HasForeignKey(reaction => reaction.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}