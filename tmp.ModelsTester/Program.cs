using Chem.DataContext;
using Chem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp.ModelsTester
{
    class Program
    {
        static void Main(string[] args)
        {
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
                var categories = context.Set<Category>().Include(x => x.Parents);
                var substancies = context.Set<Substance>()
                    .Include(x => x.Names)
                    .Include(x => x.Scheme)
                    .Include(x => x.Categories)
                    .Include(x => x.Categories.Select(y => y.Parents));
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
                foreach (var entity in context.Set<Category>())
                    context.Set<Category>().Remove(entity);
                foreach (var entity in context.Set<Substance>())
                    context.Set<Substance>().Remove(entity);

                context.SaveChanges();
            }
        }
    }
}
