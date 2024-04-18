using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAuto.Events.CIM
{
    internal class CimProperty
    {
        public required string Name { get; set; }
        public required object Value { get; set; }
    }
}
