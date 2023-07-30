using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.ModelBuilders
{
    public static class RoleModelBuilder
    {
        public static Action<EntityTypeBuilder<Role>> Get()
        {
            return
                entity =>
                {
                    entity
                        .HasMany(x => x.Users)
                        .WithOne(u => u.Role);
                        
                };
        }
    }
}
