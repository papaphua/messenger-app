using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApp.Infrastructure.Data.Configurations;

public sealed class DirectConfiguration : IEntityTypeConfiguration<Direct>
{
    public void Configure(EntityTypeBuilder<Direct> builder)
    {
        builder.HasMany(direct => direct.Messages)
            .WithOne(message => message.Chat);
    }
}