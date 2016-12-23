using System.Linq;

namespace Chem.Models.Search
{
    public class FullSubstanceModel : IFullSubstanceModel
    {
        public string Formula { get; set; }
        public string[] Categories { get; set; }
        public string[] Names { get; set; }
        public string[] Schemes { get; set; }
        public double? MolecularWeight { 
            get
            {
                return Elements == null ? null : Elements.Select(x => x.Weight * x.Count).Aggregate((x1,x2) => x1 + x2) as double?;
            }
        }
        public StructureElement[] Elements { get; set; }
        public double? MeltingPoint { get; set; }
        public double? BoilingPoint { get; set; }
        public double? FlashPoint { get; set; }
        public double? Density { get; set; }
        public double? RefractiveIndex { get; set; }
        public double? VapourPressur { get; set; }
        public string WaterSolubility { get; set; }
        public string HazardSymbols { get; set; }
    }
}