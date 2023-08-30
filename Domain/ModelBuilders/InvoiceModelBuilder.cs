using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Domain.ModelBuilders
{
    public static class InvoiceModelBuilder
    {
        public static Action<EntityTypeBuilder<Invoice>> Get()
        {
            return
                entity =>
                {
                    entity.Property(e => e.BillTo).IsRequired();
                    entity.Property(e => e.BillToEmail).IsRequired();
                    entity.Property(e => e.BillToAddress).IsRequired();

                    entity.Property(e => e.BillFrom).IsRequired();
                    entity.Property(e => e.BillFromEmail).IsRequired();
                    entity.Property(e => e.BillFromAddress).IsRequired();

                    entity.Property(e => e.DueDate).IsRequired();
                    entity.Property(e => e.RecordId).ValueGeneratedOnAdd();

                    entity
                        .HasMany(e => e.InvoiceItems)
                        .WithOne(x => x.Invoice)
                            .HasForeignKey(x => x.InvoiceId);
                };
        }

    }
}
