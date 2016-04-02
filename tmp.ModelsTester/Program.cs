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
            Clear();
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
