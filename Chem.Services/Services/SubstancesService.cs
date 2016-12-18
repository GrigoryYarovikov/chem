using Chem.Managers;
using Chem.Models;
using Chem.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Helpers;

namespace Chem
{
    public class SubstancesService
    {
        SubstanceManager _substances;
        ElementManager _elements;

        public SubstancesService() 
        {
            _substances = new SubstanceManager();
            _elements = new ElementManager();
        }

        public FullSubstanceModel GetById(int id)
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

        public List<SubstancePreview> GetByQuery(QueryModel query)
        {
            var elements = _elements.GetAll().ToList();
            var qToDb = _substances.GetAll();

            var name = "";
            qToDb = BuildQuery(qToDb, query, out name);
            var list = qToDb.Take(100).ToList();
            name = name.ToLower();
            var result = list.Select(x => new SubstancePreview 
            {
                Name = x.Names.FirstOrDefault(n => n.Value.ToLower().Contains(name))?.Value,
                Formula = x.Formula,
                Categories = GetCategoryList(x.Categories).OrderBy(c => c.Id).Select(c => c.Name).Distinct().ToArray(),
                Synonyms = x.Names.Select(n => n.Value).Take(12).ToArray(),
                Scheme = x.Scheme.Select(s => s.Value.HtmlDecode()).FirstOrDefault(),
                Id = x.Id

            }).ToList();
            return result;
        }

        public List<Reaction> GetReactionList(int id)
        {
            var elem = GetById(id);
            var formula = elem.Formula;
            var isOrganic = elem.Categories.Any(x => x.ToLower() == "органическое вещество");
            var f = LoadPageService.LoadReactionList(formula, isOrganic);
            if (f == null) return null;
            return f.Take(100).ToList();
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

        List<string> _store = new List<string>();

        private void CombineFormulaVariants(string s, List<string> elements, string res)
        {
            if (!s.HasValue())
            {
                _store.Add(res);
                return;
            }

            var uuu = s.TakeWhile(x => x.IsNumeric());
            if (uuu != null && uuu.Any())
            {
                var ttt = uuu.Select(x => x.ToString()).Aggregate((s1, s2) => s1 + s2);
                res += ttt;
                CombineFormulaVariants(s.Substring(ttt.Length), elements, res);
            }
            else
            {
                var error = 0;
                var ttt = s.Substring(0, 1);
                if (elements.Contains(ttt))
                    CombineFormulaVariants(s.Substring(1), elements, res + ttt.ToUpper());
                else
                    ++error;

                if (s.Length > 1)
                {
                    ttt = s.Substring(0, 2);
                    if (elements.Contains(ttt))
                        CombineFormulaVariants(s.Substring(2), elements, res + ttt.Substring(0, 1).ToUpper() + ttt.Substring(1));
                    else
                        ++error;
                }

                if (error == 2)
                    return;
            }
        }

        private List<string> BFormulaOrNull(string s)
        {
            var elements = _elements.GetAll().Select(x => x.Sign.ToLower()).ToList();

            CombineFormulaVariants(s.ToLower(), elements, "");

            return _store.Select(x => FormulaToBrutto(x)).ToList();
        }

        private string FormulaToBrutto(string formula)
        {
            var list = new Dictionary<string, int>();
            var num = "";
            var name = "";
            foreach (var ch in formula)
            {
                if (ch.IsUpper())
                {
                    var dg = num.ToNaturalOrZero();
                    dg = dg == 0 ? 1 : dg;
                    if (name.HasValue())
                        if (list.ContainsKey(name))
                            list[name] += dg;
                        else
                            list.Add(name, dg);

                    name = ch.ToString();
                    num = "";
                }
                else if (ch.IsNumeric())
                    num += ch;
                else
                    name += ch;
            }
            var d = num.ToNaturalOrZero();
            d = d == 0 ? 1 : d;
            if (name.HasValue())
                if (list.ContainsKey(name))
                    list[name] += d;
                else
                    list.Add(name, d);

            return list.OrderBy(x => x.Key).Select(x => x.Key + (x.Value == 1 ? "" : x.Value.ToString())).Aggregate((x, y) => x + y);
        }

        private StructureElement[] ParseFormula(string formula)
        {
            var list = new List<StructureElement>();
            var num = "";
            var name = "";
            foreach (var ch in formula)
            {
                if (ch.IsUpper())
                {
                    var e = GetElement(name, num);
                    if (e != null)
                        list.Add(e);

                    name = ch.ToString();
                    num = "";
                }
                else if (ch.IsNumeric())
                    num += ch;
                else
                    name += ch;
            }
            var elem = GetElement(name, num);
            if (elem != null)
                list.Add(elem);

            var elements = _elements.GetAll();

            foreach (var item in list)
            {
                var element = elements.FirstOrDefault(x => x.Sign == item.Sign);
                if (element == null)
                    return null;
                item.Weight = element.Weight;
                item.Rus = element.Name;
            }
            return list.ToArray();
        }

        private StructureElement GetElement(string name, string num)
        {
            if (name.HasValue())
            {
                var elem = new StructureElement
                {
                    Sign = name,
                    Count = num.HasValue() ? num.ToNaturalOrZero() : 1
                };
                return elem;
            }
            return null;
        }

        private IQueryable<Substance> BuildQuery(IQueryable<Substance> set, QueryModel q, out string name)
        {
            name = "";

            if (q.bp1.HasValue)
                set = set.Where(x => x.BoilingPoint >= q.bp1.Value);
            if (q.bp2.HasValue)
                set = set.Where(x => x.BoilingPoint <= q.bp2.Value);
            if (q.d1.HasValue)
                set = set.Where(x => x.Density >= q.d1.Value);
            if (q.d2.HasValue)
                set = set.Where(x => x.Density <= q.d2.Value);
            if (q.fp1.HasValue)
                set = set.Where(x => x.FlashPoint >= q.fp1.Value);
            if (q.fp2.HasValue)
                set = set.Where(x => x.FlashPoint <= q.fp2.Value); 
            if (q.mp1.HasValue)
                set = set.Where(x => x.MeltingPoint >= q.mp1.Value);
            if (q.mp2.HasValue)
                set = set.Where(x => x.MeltingPoint <= q.mp2.Value); 
            if (q.ri1.HasValue)
                set = set.Where(x => x.RefractiveIndex >= q.ri1.Value);
            if (q.ri2.HasValue)
                set = set.Where(x => x.RefractiveIndex <= q.ri2.Value);
            if (q.vp1.HasValue)
                set = set.Where(x => x.VapourPressur >= q.vp1.Value);
            if (q.vp2.HasValue)
                set = set.Where(x => x.VapourPressur <= q.vp2.Value);
            if (q.ws.HasValue)
                set = set.Where(x => x.WaterSolubility == q.ws.Value);
            if (q.q.HasValue())
            {
                //var list = q.q.Split();
                var formulas = BFormulaOrNull(q.q.Trim());
                if (formulas != null && formulas.Count > 0)
                    set = set.Where(x => formulas.Contains(x.Formula));
                else
                {
                    set = set.Where(x => x.Names.Any(
                        y => y.Value.ToLower().Contains(q.q)
                        ));
                    name = q.q.Trim().ToLower();
                }
            }
            return set;
        }
    }
}