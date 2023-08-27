using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Entities.Joints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(user => user.Directs)
            .WithMany(direct => direct.Members)
            .UsingEntity<DirectMember>();

        builder.HasMany(user => user.Groups)
            .WithMany(group => group.Members)
            .UsingEntity<GroupMember>();

        builder.HasMany(user => user.Channels)
            .WithMany(channel => channel.Members)
            .UsingEntity<ChannelMember>();
    }
}