using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(user => user.PersonalChats)
            .WithMany(chat => chat.Users)
            .UsingEntity("UserPersonalChat");

        builder.HasMany(user => user.GroupChats)
            .WithMany(chat => chat.Users)
            .UsingEntity("UserGroupChat");
    }
}