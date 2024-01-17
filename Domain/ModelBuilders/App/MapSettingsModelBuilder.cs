using Domain.Entities.App;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.ModelBuilders.App
{
    public static class MapSettingsModelBuilder
    {
        public static Action<EntityTypeBuilder<MapSettings>> Get()
        {
            return
                entity =>
                {
                    // Schema is defined in the entity file
                };
        }
    }
}
