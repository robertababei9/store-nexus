using Domain.Entities;


namespace Domain.Dto.Stores
{
    public class CreateStoreDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Contact { get; set; }
        public string WorkingHours { get; set; }
        public Guid ManagerId { get; set; }
        public Guid StoreStatusId { get; set; }

        // I'm not putting it in an object because ATM it's easier for me to send it like that from front-end
        #region Location 
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string LatLng { get; set; }
        public string Location { get; set; }
        #endregion
    }

}
