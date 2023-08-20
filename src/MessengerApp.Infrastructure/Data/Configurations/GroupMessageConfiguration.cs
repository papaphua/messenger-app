using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class GroupMessageConfiguration : IEntityTypeConfiguration<GroupMessage>
{
    public void Configure(EntityTypeBuilder<GroupMessage> builder)
    {
        builder.HasMany(message => message.Attachments)
            .WithOne(attachment => attachment.Message);

        builder.HasMany(message => message.Reactions)
            .WithOne(reaction => reaction.Message);
    }
}