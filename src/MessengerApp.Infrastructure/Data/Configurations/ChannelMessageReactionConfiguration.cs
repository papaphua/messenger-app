using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class ChannelMessageReactionConfiguration : IEntityTypeConfiguration<ChannelMessageReaction>
{
    public void Configure(EntityTypeBuilder<ChannelMessageReaction> builder)
    {
        builder.HasOne(reaction => reaction.User)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}