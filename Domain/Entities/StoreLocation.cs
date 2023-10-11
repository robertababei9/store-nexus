using Domain.Common;

namespace Domain.Entities
{
    public class StoreLocation : BaseEntity<Guid>
    {
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string LatLng { get; set; }
        public string Location { get; set; }


        public virtual Store Store { get; set; }
    }
}
