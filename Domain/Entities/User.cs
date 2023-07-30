using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity, IAuditableEntity
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public Guid? RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }


        public virtual Role Role { get; set; }

    }
}
