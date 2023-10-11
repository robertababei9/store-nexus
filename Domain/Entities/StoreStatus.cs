using Domain.Common;

namespace Domain.Entities
{
    public class StoreStatus : BaseEntity<Guid>
    {
        public int StoreStatusType { get; set; }
        public string Description { get; set; }
    }
}
