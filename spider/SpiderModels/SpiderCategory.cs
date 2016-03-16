using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    class SpiderCategory
    {
        public int CatId { get; set; }
        public string Name { get; set; }
        public List<string> Parents { get; set; }
    }
}
