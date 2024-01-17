using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.Settings
{
    public class MapSettingsDto
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int Zoom { get; set; }
    }
}
