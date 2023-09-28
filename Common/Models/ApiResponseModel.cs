using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ApiResponseModel<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
