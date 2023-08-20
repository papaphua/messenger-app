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
            .WithOne(message => message.Chat);

        builder.HasMany(group => group.Admins)
            .WithMany()
            .UsingEntity<GroupAdmin>();
    }
}