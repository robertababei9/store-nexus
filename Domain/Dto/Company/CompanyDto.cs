using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.Company
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TotalStores { get; set; }
        public int TotalMembers { get; set; }
        public string ImageUrl { get; set; }
    }
}
