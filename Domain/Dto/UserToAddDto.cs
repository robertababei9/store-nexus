

using Domain.Entities;

namespace Domain.Dto
{
    public class UserToAddDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password {get; set;}
        public string Contact { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string SignUpDate { get; set; }
        public Guid RoleId { get; set; }
        public Guid StoreId { get; set; }


        public User ToEntity()
        {
            var user = new User();
            user.Name = FirstName + " " + LastName ;
            user.Email = Email;
            user.RoleId = RoleId;


            return user;
        }
    }
}
