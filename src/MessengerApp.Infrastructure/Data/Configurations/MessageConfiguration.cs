using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasMany(message => message.Attachments)
            .WithOne(attachment => attachment.Message)
            .HasForeignKey(attachment => attachment.MessageId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(message => message.Reactions)
            .WithOne(reaction => reaction.Message)
            .HasForeignKey(reaction => reaction.MessageId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}