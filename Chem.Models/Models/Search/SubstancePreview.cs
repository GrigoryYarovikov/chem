using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chem.Models.Search
{
    public class SubstancePreview
    {
        public string Name { get; set; }
        public string Formula { get; set; }
        public string[] Categories { get; set; }
        public string[] Synonyms { get; set; }
        public string Scheme { get; set; }
        public int Id { get; set; }
    }
}