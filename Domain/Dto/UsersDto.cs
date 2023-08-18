using Domain.Entities;


namespace Domain.Dto
{
    public class UsersDto
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string DateOfBirth { get; set; }
        public Guid RoleId { get; set; }
        public Guid AssignedStore { get; set; }

        //public Role Role { get; set; }


        public UsersDto()
        {

        }

        public UsersDto(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            //Role = user.Role;
        }

        public User ToEntity()
        {
            var user = new User();
            user.Id = Id;
            user.Name = Name;
            user.Email = Email;
            user.RoleId = RoleId;
            //user.Role = Role;

            return user;
        }

    }
}
