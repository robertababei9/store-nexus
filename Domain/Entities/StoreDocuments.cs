using Domain.Common;

namespace Domain.Entities
{
    public class StoreDocuments : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Uri { get; set; }

        // it was decided to not keep the reference of the user who uploaded
        // but just to know the name of it. Change it how you need it in the future
        public string UploadedBy { get; set; }  
        public Guid StoreId { get; set; }


        public virtual Store Store { get; set; }
    }
}
