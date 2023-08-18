using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Role : BaseEntity<Guid>, IAuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }    // one to many relationship
    }
}
