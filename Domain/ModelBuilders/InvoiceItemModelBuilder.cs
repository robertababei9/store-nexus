
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.ModelBuilders
{

    public static class InvoiceItemModelBuilder
    {
        public static Action<EntityTypeBuilder<InvoiceItem>> Get()
        {
            return
                entity =>
                {
                    entity.Property(e => e.Title).IsRequired();
                    entity.Property(e => e.Qty).IsRequired();
                    entity.Property(e => e.Price).IsRequired();

                    entity
                        .HasOne(e => e.Invoice)
                        .WithMany(x => x.InvoiceItems);
                };
        }

    }
}
