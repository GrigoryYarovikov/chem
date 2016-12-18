using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chem.Models.Search
{
    public class QueryModel
    {
        public string q { get; set; }
        public double? mp1 { get; set; }
        public double? mp2 { get; set; }
        public double? fp1 { get; set; }
        public double? fp2 { get; set; }
        public double? bp1 { get; set; }
        public double? bp2 { get; set; }
        public double? d1 { get; set; }
        public double? d2 { get; set; }
        public double? vp1 { get; set; }
        public double? vp2 { get; set; }
        public double? ri1 { get; set; }
        public double? ri2 { get; set; }
        public bool? ws { get; set; }
    }
}