using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class ChannelMessageConfiguration : IEntityTypeConfiguration<ChannelMessage>
{
    public void Configure(EntityTypeBuilder<ChannelMessage> builder)
    {
        builder.HasMany(message => message.Attachments)
            .WithOne(attachment => attachment.Message);

        builder.HasMany(message => message.Reactions)
            .WithOne(reaction => reaction.Message);
    }
}