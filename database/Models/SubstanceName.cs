using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Models
{
    public class SubstanceName
    {
        public SubstanceName()
        {
        }
        public SubstanceName(string name)
        {
            Value = name;
        }
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
