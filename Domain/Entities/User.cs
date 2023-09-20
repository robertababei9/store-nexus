using Domain.Common;
using Domain.Dto;


namespace Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? CompanyId { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }


        public virtual Role Role { get; set; }
        public virtual Company? Company { get; set; }


        public void FromDto(UsersDto userDto)
        {
            Email = userDto.Email;
            Name = userDto.FullName;
            RoleId = userDto.RoleId;
        }

    }
}
