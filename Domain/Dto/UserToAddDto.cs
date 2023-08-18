

using Domain.Entities;

namespace Domain.Dto
{
    public class UserToAddDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password {get; set;}
        public string Location { get; set; }
        public string Contact { get; set; }
        public string DateOfBirth { get; set; }
        public Guid RoleId { get; set; }
        public Guid AssignedStore { get; set; }


        public User ToEntity()
        {
            var user = new User();
            user.Name = Name;
            user.Email = Email;
            user.RoleId = RoleId;


            return user;
        }
    }
}
