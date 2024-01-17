using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Domain.Entities.App
{
    [Table("MapSettings", Schema = "app")]
    public class MapSettings: BaseEntity<Guid>
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }

        public string Lat { get; set; }
        public string Lng { get; set; }
        public int Zoom { get; set; }



        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual Company Company { get; set; }
    }
}
