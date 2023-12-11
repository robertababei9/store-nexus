using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Domain.ModelBuilders
    {
        public static class UserInvitationsModelBuilder
    {
            public static Action<EntityTypeBuilder<UserInvitations>> Get()
            {
                return
                    entity =>
                    {
                        entity.HasOne(x => x.User)
                            .WithMany(y => y.UserInvitations)
                            .HasForeignKey(x => x.InviterId)
                            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);

                    };
            }
        }
    }
