using MessengerApp.Domain.Abstractions;
using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class GroupChatConfiguration : IEntityTypeConfiguration<GroupChat>
{
    public void Configure(EntityTypeBuilder<GroupChat> builder)
    {
        builder.HasMany(chat => chat.Messages)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);;

        builder.HasMany(chat => chat.Admins)
            .WithMany()
            .UsingEntity("GroupChatAdmin");
    }
}