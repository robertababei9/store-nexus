using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.ModelBuilders
{
    public static class StoreModelBuilder
    {
        public static Action<EntityTypeBuilder<Store>> Get()
        {
            return
                entity =>
                {
                    entity.HasOne(x => x.Company)
                        .WithMany(r => r.Stores)
                        .HasForeignKey(x => x.CompanyId);

                    entity.HasOne(x => x.Manager)
                        .WithMany(r => r.Stores)
                        .HasForeignKey(x => x.ManagerId);

                    entity.HasOne(x => x.StoreStatus);

                    entity.HasOne(x => x.StoreLocation);

                    entity.Property(x => x.TotalSales).HasPrecision(16, 2);

                };
        }
    }
}
