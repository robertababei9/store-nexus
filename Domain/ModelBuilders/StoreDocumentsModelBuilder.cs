using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.ModelBuilders
{
    public static class StoreDocumentsModelBuilder
    {
        public static Action<EntityTypeBuilder<StoreDocuments>> Get()
        {
            return
                entity =>
                {
                    entity.HasOne(x => x.Store)
                        .WithMany(r => r.StoreDocuments)
                        .HasForeignKey(x => x.StoreId);

                };
        }
    }
}
