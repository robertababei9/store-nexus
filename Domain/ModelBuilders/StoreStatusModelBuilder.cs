using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.ModelBuilders
{
    public static class StoreStatusModelBuilder
    {
        public static Action<EntityTypeBuilder<StoreStatus>> Get()
        {
            return
                entity =>
                {
                    entity.HasIndex(x => x.StoreStatusType).IsUnique(true);

                };
        }
    }
}
