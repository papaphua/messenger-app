using MessengerApp.Domain.Abstractions;
using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class PersonalChatConfiguration : IEntityTypeConfiguration<PersonalChat>
{
    public void Configure(EntityTypeBuilder<PersonalChat> builder)
    {
        builder.HasMany(chat => chat.Messages)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);;
    }
}