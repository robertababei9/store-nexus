using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModelBuilders
{
    public static class UserModelBuilder
    {
        public static Action<EntityTypeBuilder<User>> Get()
        {
            return
                entity =>
                {
                    entity.HasIndex(x => x.Email).IsUnique();
                };
        }
    }
}
