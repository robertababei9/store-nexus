using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Company : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int NoEmployees { get; set; }
        public string Type { get; set; }
        public string? Contact { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Address { get; set; }

        //public string Description { get; set; }
        //public decimal Revenue { get; set; }
        //public string LogoUrl { get; set; }


        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }    // one to many relationship
        [JsonIgnore]
        public virtual ICollection<Store> Stores { get; set; }  // one to many relationship
    }
}
