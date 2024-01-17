using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.ModelBuilders
{
    public static class RolePermissionsModelBuilder
    {
        public static Action<EntityTypeBuilder<RolePermissions>> Get()
        {
            return
                entity =>
                {
                    // Schema was defined in the entity file
                };
        }
    }
}
