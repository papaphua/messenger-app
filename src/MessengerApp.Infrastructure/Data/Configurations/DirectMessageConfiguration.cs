using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class DirectMessageConfiguration : IEntityTypeConfiguration<DirectMessage>
{
    public void Configure(EntityTypeBuilder<DirectMessage> builder)
    {
        builder.HasMany(message => message.Attachments)
            .WithOne(attachment => attachment.Message);

        builder.HasMany(message => message.Reactions)
            .WithOne(reaction => reaction.Message);
    }
}