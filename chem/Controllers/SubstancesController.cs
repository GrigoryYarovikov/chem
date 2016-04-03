using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Chem.Models;
using Chem.Managers;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Web.Mvc;
using Chem.Models.Search;
using System.Web;
using Common.Helpers;

namespace Chem.Controllers
{
    
    //[Authorize]
    public class SubstancesController : ApiController
    {
        SubstanceManager _substances = new SubstanceManager();
        ElementManager _elements = new ElementManager();

        // GET api/values
        public List<SubstancePreview> GetByQuery(string query)
        {
            var list = _substances.GetByName(query.Trim()).Take(200).ToList();
            //if (!list.Any())
            //{
            //    return null;
            //}

            var result = list.Select(x => new SubstancePreview 
            {
                Name = x.Names.FirstOrDefault(n => n.Value.ToLower().Contains(query.ToLower())).Value,
                Formula = x.Formula,
                Categories = GetCategoryList(x.Categories).OrderBy(c => c.Id).Select(c => c.Name).Distinct().ToArray(),
                Synonyms = x.Names.Select(n => n.Value).Take(12).ToArray(),
                Scheme = x.Scheme.Select(s => s.Value.HtmlDecode()).FirstOrDefault(),
                Id = x.Id

            }).ToList();
            return result;
        }

        // GET api/values/5
        public FullSubstanceModel Get(int id)
        {
            var item = _substances.GetById(id);
            var result = new FullSubstanceModel
            {
                BoilingPoint = item.BoilingPoint,
                Categories = GetCategoryList(item.Categories).OrderBy(c => c.Id).Select(c => c.Name).Distinct().ToArray(),
                Density = item.Density,
                Elements = ParseFormula(item.Formula),
                FlashPoint = item.FlashPoint,
                Formula = item.Formula,
                HazardSymbols = item.HazardSymbols,
                MeltingPoint = item.MeltingPoint,
                Names = item.Names.Select(x => x.Value).ToArray(),
                RefractiveIndex = item.RefractiveIndex,
                Schemes = item.Scheme.Select(x => x.Value.HtmlDecode()).ToArray(),
                VapourPressur = item.VapourPressur,
                WaterSolubility = WaterBoolToStr(item.WaterSolubility)
            };
            return result;
        }

        private List<Category> GetCategoryList(IEnumerable<Category> catList)
        {
            var result = new List<Category>();
            if (catList != null && catList.Any())
            {
                foreach (var item in catList)
                {
                    result.Add(item);
                    result.AddRange(GetCategoryList(item.Parents));
                }
            }
            return result;
        }

        private string WaterBoolToStr(bool? b)
        {
            if (!b.HasValue)
                return null;
            return b.Value ? "да" : "нет";
        }

        private StructureElement[] ParseFormula(string formula)
        {
            var elements = _elements.GetAll();
            return null;
        }
    }
}
