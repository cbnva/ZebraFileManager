using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraFileManager.Zebra
{
    public class File
    {
        public string Drive { get; set; }

        public string Filename { get; set; }

        public string Path
        {
            get
            {
                if (Drive == @"\")
                {
                    return Filename;
                }
                else
                {
                    return Drive + ":" + Filename;
                }
            }
        }

        public int Size { get; set; }

        public List<string> Attributes { get; set; }
    }
}
