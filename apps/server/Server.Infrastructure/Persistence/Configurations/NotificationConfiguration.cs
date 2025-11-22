using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");

            builder.HasKey(e => e.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.FromUser)
                .WithMany()
                .HasForeignKey(x => x.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Subject)
                .IsRequired();

            builder.Property(x => x.Message)
                .IsRequired();

            builder.Property(x => x.IsRead)
                .IsRequired();
        }

    }
}