using Domain.Entities;


namespace Domain.Dto
{
    public class UsersDto
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; }
        public Guid RoleId { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string Store { get; set; }
        public Guid StoreId { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly SignUpDate { get; set; }



        public UsersDto()
        {

        }

        //public UsersDto(User user)
        //{
        //    Id = user.Id;
        //    FullName = user.Name;
        //    Email = user.Email;
        //    Role = user.Role.Description;
        //    Location = "user.Store.Location";
        //    Store = "user.Store.Name";
        //    PhoneNumber = "user.PhoneNumber";

        //}

    }
}
