namespace Chem.Models.Search
{
    public interface IFullSubstanceModel
    {
        string Formula { get; set; }
        string[] Categories { get; set; }
        string[] Names { get; set; }
        string[] Schemes { get; set; }
        double? MolecularWeight { get; }
        StructureElement[] Elements { get; set; }
        double? MeltingPoint { get; set; }
        double? BoilingPoint { get; set; }
        double? FlashPoint { get; set; }
        double? Density { get; set; }
        double? RefractiveIndex { get; set; }
        double? VapourPressur { get; set; }
        string WaterSolubility { get; set; }
        string HazardSymbols { get; set; }
    }
}