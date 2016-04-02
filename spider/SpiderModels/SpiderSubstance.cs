using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    class SpiderSubstance : IComparable
    {
        public string[] Names { get; set; }
        public string[] Formulas { get; set; }
        public string[] Categories { get; set; }
        public string CAS { get; set; }
        public string BruttoFormula { get; set; }

        public int CompareTo(object obj)
        {
            var other = (obj as SpiderSubstance);
            return (CAS + BruttoFormula).CompareTo(other.CAS + other.BruttoFormula);
        }
    }
}
