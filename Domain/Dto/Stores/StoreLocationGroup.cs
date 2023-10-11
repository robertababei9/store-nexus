
namespace Domain.Dto.Stores
{
    public class StoreLocationGroup
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public IList<StoreLocationInfo> Stores { get; set; }
    }



    public class StoreLocationInfo
    {
        public string StoreName { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
}
