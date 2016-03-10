using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Models
{
    public class SubstanceScheme
    {
        public SubstanceScheme()
        {
        }
        public SubstanceScheme(string scheme)
        {
            Value = scheme;
        }
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
