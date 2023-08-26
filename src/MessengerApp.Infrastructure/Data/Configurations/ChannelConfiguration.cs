using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Entities.Joints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.HasMany(channel => channel.Messages)
            .WithOne(message => message.Chat)
            .HasForeignKey(message => message.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(channel => channel.Owner)
            .WithMany()
            .HasForeignKey(channel => channel.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasMany(channel => channel.Admins)
            .WithMany()
            .UsingEntity<ChannelAdmin>();
    }
}