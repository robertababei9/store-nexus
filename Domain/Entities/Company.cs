using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Company : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string? Contact { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
