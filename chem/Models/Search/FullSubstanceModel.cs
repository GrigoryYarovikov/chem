﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chem.Models;

namespace Chem.Models.Search
{
    public class FullSubstanceModel
    {
        public string Formula { get; set; }
        public string[] Categories { get; set; }
        public string[] Names { get; set; }
        public string[] Schemes { get; set; }
        public double MolecularWeight { 
            get
            {
                return 0;//Elements.Select(x => x.Weight * x.Count).Aggregate((x1,x2) => x1 + x2);
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