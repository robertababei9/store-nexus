using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.ModelBuilders
{
    public static class StoreLocationModelBuilder
    {
        public static Action<EntityTypeBuilder<StoreLocation>> Get()
        {
            return
                entity =>
                {


                };
        }
    }
}
