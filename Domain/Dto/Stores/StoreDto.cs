using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.Stores
{
    public class StoreDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string WorkingHours { get; set; }
        public string ManagerName { get; set; }
        public Guid ManagerId { get; set; }
        public decimal TotalSales { get; set; }
        public Guid StatusId { get; set; }
        public string StoreStatusName { get; set; }
        public string LastUpdated { get; set; }
    }
}
