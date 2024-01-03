using Domain.Common;
//using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Store : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Contact { get; set; }
        public string WorkingHours { get; set; }
        public decimal TotalSales { get; set; }

        public Guid CompanyId { get; set; }
        public Guid ManagerId { get; set; } // managerId = userId
        public Guid StoreStatusId { get; set; }
        public Guid? StoreLocationId { get; set; }


        //[JsonIgnore]
        //public virtual ICollection<User> Users { get; set; }    // one to many relationship

        public virtual Company Company { get; set; }
        public virtual User Manager { get; set; }
        public virtual StoreStatus StoreStatus { get; set; }
        public virtual StoreLocation? StoreLocation { get; set; }
        public virtual ICollection<StoreDocuments> StoreDocuments { get; set; }
    }
}
