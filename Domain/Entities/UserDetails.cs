using Domain.Common;
using Domain.Dto;

namespace Domain.Entities
{
    public class UserDetails : BaseEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Contact { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public DateOnly SignUpDate { get; set; }

        public Guid UserId { get; set; }


        public virtual User User { get; set; }
    }
}
