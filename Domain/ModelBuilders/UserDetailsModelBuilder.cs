using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Domain.ModelBuilders
{
    public static class UserDetailsModelBuilder
    {
        public static Action<EntityTypeBuilder<UserDetails>> Get()
        {
            return
                entity =>
                {
                    entity
                        .HasOne(x => x.User)
                        .WithOne(r => r.UserDetails);

                };
        }
    }
}
