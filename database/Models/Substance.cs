using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Models
{
    public class Substance
    {
        public int Id { get; set; }
        public virtual List<SubstanceName> Names { get; set; }
        public virtual List<Category> Categories { get; set; }
        public string Formula { get; set; }
        public virtual List<SubstanceScheme> Scheme { get; set; }
        public double? MeltingPoint { get; set; }
        public double? BoilingPoint { get; set; }
        public double? FlashPoint { get; set; }
        public double? Density { get; set; }
        public double? RefractiveIndex { get; set; }
        public double? VapourPressur { get; set; }
        public bool? WaterSolubility { get; set; }
        public string HazardSymbols { get; set; }
        public string CAS { get; set; }
    }
}
