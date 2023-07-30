using Domain.Common;

namespace Domain.Entities
{
    public class Role : BaseEntity, IAuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<User> Users { get; set; }    // one to many relationship
    }
}
