using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.Settings
{
    public class SaveRolePermissionsDto
    {
        public Dictionary<string, bool> RolePermissions { get; set; }
    }
}
