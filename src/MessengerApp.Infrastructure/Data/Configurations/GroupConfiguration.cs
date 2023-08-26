using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Entities.Joints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasMany(group => group.Messages)
            .WithOne(message => message.Chat)
            .HasForeignKey(message => message.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(group => group.Owner)
            .WithMany()
            .HasForeignKey(group => group.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasMany(group => group.Admins)
            .WithMany()
            .UsingEntity<GroupAdmin>();
    }
}