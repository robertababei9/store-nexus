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
        public virtual UserDetails UserDetails { get; set; }

        public virtual ICollection<UserInvitations> UserInvitations { get; set; }
        public virtual ICollection<Store> Stores { get; set; } 


        public void FromDto(UsersDto userDto)
        {
            Email = userDto.Email;
            Name = userDto.FirstName + " " + userDto.LastName;
            RoleId = userDto.RoleId;
        }

    }
}
