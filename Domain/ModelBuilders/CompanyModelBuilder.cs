
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.ModelBuilders
{
    public static class CompanyModelBuilder
    {
        public static Action<EntityTypeBuilder<Company>> Get()
        {
            return
                entity =>
                {
                    entity.Property(e => e.Name).IsRequired();

                };
        }

    }
}
