using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.ModelBuilders
{
    public static class UserModelBuilder
    {
        public static Action<EntityTypeBuilder<User>> Get()
        {
            return
                entity =>
                {
                    entity.HasIndex(x => x.Email).IsUnique();

                    entity
                        .HasOne(x => x.Role)
                        .WithMany(r => r.Users)
                        .HasForeignKey(x => x.RoleId);

                    entity
                        .HasOne(x => x.Company)
                        .WithMany(r => r.Users)
                        .HasForeignKey(x => x.CompanyId);

                };
        }
    }
}
