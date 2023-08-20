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
            .WithMany(direct => direct.Users)
            .UsingEntity<UserDirect>();

        builder.HasMany(user => user.Groups)
            .WithMany(group => group.Users)
            .UsingEntity<UserGroup>();

        builder.HasMany(user => user.Channels)
            .WithMany(channel => channel.Users)
            .UsingEntity<UserChannel>();
    }
}