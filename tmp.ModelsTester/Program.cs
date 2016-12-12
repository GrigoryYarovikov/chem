using Chem.DataContext;
using Chem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Helpers;

namespace tmp.ModelsTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //var patt = new Regex(@"^([+-]?)(?=\d|\.\d)\d*(\.\d*)?([Ee]([+-]?\d+))?");
            //var number = patt.Match("333.8°C at 760 mmHg");
            //var ttt = Double.Parse("2.55E-05", CultureInfo.InvariantCulture);
            //AddData();
            //GetData();
            //Clear();
            BFormulaOrNull("ho20oh");
            //RenweDatabase();
        }

        static List<string> _store = new List<string>();

        private static void PPP(string s, List<string> elements, string res)
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
                PPP(s.Substring(ttt.Length), elements, res);
            }
            else
            {
                var error = 0;
                var ttt = s.Substring(0, 1);
                if (elements.Contains(ttt))
                    PPP(s.Substring(1), elements, res + ttt.ToUpper());
                else
                    ++error;

                if (s.Length > 1)
                {
                    ttt = s.Substring(0, 2);
                    if (elements.Contains(ttt))
                        PPP(s.Substring(2), elements, res + ttt.Substring(0, 1).ToUpper() + ttt.Substring(1));
                    else
                        ++error;
                }

                if (error == 2) 
                    return;
            }
        }


        private static string BFormulaOrNull(string s)
        {
            var context = ChemContext.Create();
            var elements = context.Set<Element>().Select(x => x.Sign.ToLower()).ToList();

            PPP(s, elements, "");

            var iii = _store.Select(x => ParseFormula1(x)).ToList();
            return null;
        }


        private static void RenweDatabase()
        {
            using (var context = ChemContext.Create())
            {
                var elements = context.Set<Element>().Select(x => x.Sign.ToLower()).ToList();
                var l = context.Set<Substance>();
                foreach (var item in l)
                {
                    var ttt = ParseFormula1(item.Formula);
                    if (ttt.HasValue())
                        item.Formula = ttt;
                    else
                    { 
                    }
                }
                context.SaveChanges();
            }
        }

        private static string ParseFormula1(string formula)
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

        static void AddData()
        {
            using (var context = ChemContext.Create())
            {
                var c = context.Set<Category>().FirstOrDefault(x => x.Name == "Hernya");
                var c2 = context.Set<Category>().FirstOrDefault(x => x.Id == 5);
                c.Parents.Add(c2);

                //var s = new Substance
                //{
                //    BoilingPoint = 0.34343,
                //    CAS = "cas jjjJjjJjJ",
                //    Categories = new List<Category>(new Category[] { c }),
                //    Formula = "C2H6O",
                //    Names = GetNames("Этиловый спирт1", "etilovii spirt1"),
                //    Scheme = GetSchemes("scheme12", "scheme22"),
                //    WaterSolubility = true
                //};
                //context.Set<Substance>().Add(s);

                context.SaveChanges();
            }
        }

        static void GetData()
        {
            using (var context = ChemContext.Create())
            {
               // var categories = context.Set<Category>().Include(x => x.Parents).ToList();
                var substancies = context.Set<Substance>()
                    .Include(x => x.Names)
                    .Include(x => x.Scheme)
                    .Include(x => x.Categories)
                    .Include(x => x.Categories.Select(y => y.Parents)).FirstOrDefault(x => x.Names.Any(n => n.Value == "Хлорид кальция"));
                if (substancies != null)
                    Console.WriteLine(substancies.Formula + " " + substancies.CAS);
                else
                    Console.WriteLine("nety$");
            }
        }

        static List<SubstanceName> GetNames(params string[] values)
        {
            return new List<SubstanceName>(values.Select(x => { return new SubstanceName(x); }));
        }

        static List<SubstanceScheme> GetSchemes(params string[] values)
        {
            return new List<SubstanceScheme>(values.Select(x => { return new SubstanceScheme(x); }));
        }
        
        static void Clear()
        {
            using (var context = ChemContext.Create())
            {
                //foreach (var entity in context.Set<Category>())
                //    context.Set<Category>().Remove(entity);
                foreach (var name in context.Set<SubstanceName>())
                    context.Set<SubstanceName>().Remove(name);
                foreach (var scheme in context.Set<SubstanceScheme>())
                    context.Set<SubstanceScheme>().Remove(scheme);
                foreach (var entity in context.Set<Substance>())
                {
                    context.Set<Substance>().Remove(entity);
                }

                context.SaveChanges();
            }
        }
    }
}
