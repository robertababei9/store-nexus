using Domain.Common;
using Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public Guid? RoleId { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }


        public virtual Role Role { get; set; }


        public void FromDto(UsersDto userDto)
        {
            Email = userDto.Email;
            Name = userDto.Name;
            RoleId = userDto.RoleId;
        }

    }
}
