using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class SelectOptionModel<T>
    {
        public string label { get; set; }
        public T value { get; set; }
    }
}
