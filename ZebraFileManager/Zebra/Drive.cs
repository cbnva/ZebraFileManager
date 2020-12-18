using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraFileManager.Zebra
{
    public class Drive
    {
        public string Letter { get; set; }

        public int Free { get; set; }

        public int Used { get; set; }

        public int Size => Free + Used;

        public string Name { get; set; }
    }
}
