using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class GroupMessageReactionConfiguration : IEntityTypeConfiguration<GroupMessageReaction>
{
    public void Configure(EntityTypeBuilder<GroupMessageReaction> builder)
    {
        builder.HasOne(reaction => reaction.User)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}