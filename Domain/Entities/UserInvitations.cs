using Domain.Common;
using Domain.Dto;


namespace Domain.Entities
{
    public class UserInvitations : BaseEntity<Guid>
    {
        public Guid InviterId { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public bool Created { get; set; }


        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

    }
}
