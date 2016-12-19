using Chem.DataContext;
using Chem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Chem.Managers
{
    public class SubstanceManager : ISubstanceManager
    {
        ChemContext _context;

        public SubstanceManager()
        {
            _context = new ChemContext();
        }
        public SubstanceManager(DbContext context)
        {
            _context = context as ChemContext;
        }

        public DbContext GetContext()
        {
            return _context;
        }

        public void AddMany(IEnumerable<Substance> substances)
        {
            _context.Set<Substance>().AddRange(substances);
            _context.SaveChanges();
        }

        public IQueryable<Substance> GetAll()
        {
            return _context.Set<Substance>()
                    .Include(x => x.Names)
                    .Include(x => x.Scheme)
                    .Include(x => x.Categories)
                    .Include(x => x.Categories.Select(y => y.Parents));
        }

        public IEnumerable<Substance> GetSeveral(int count)
        {
            return _context.Set<Substance>()
                    .Include(x => x.Names)
                    .Include(x => x.Scheme)
                    .Include(x => x.Categories)
                    .Include(x => x.Categories.Select(y => y.Parents)).Take(count);
        }

        public Substance GetById(int id)
        {
            return _context.Set<Substance>()
                    .Include(x => x.Names)
                    .Include(x => x.Scheme)
                    .Include(x => x.Categories)
                    .Include(x => x.Categories.Select(y => y.Parents))
                    .FirstOrDefault(x => x.Id == id);
        }

        public void Update(Substance s)
        {
            if (s == null)
                return;

            var cs = _context.Set<Substance>().FirstOrDefault(x => x.Id == s.Id);

            if (cs == null)
            {
                return;
            }

            cs.BoilingPoint = s.BoilingPoint;
            cs.CAS = s.CAS;
            cs.Categories = s.Categories;
            cs.Density = s.Density;
            cs.FlashPoint = s.FlashPoint;
            cs.Formula = s.Formula;
            cs.HazardSymbols = s.HazardSymbols;
            cs.MeltingPoint = s.MeltingPoint;
            cs.Names = s.Names;
            cs.RefractiveIndex = s.RefractiveIndex;
            cs.Scheme = s.Scheme;
            cs.VapourPressur = s.VapourPressur;
            cs.WaterSolubility = s.WaterSolubility;
            _context.SaveChanges();
        }

        public void UpdateAll(Dictionary<int, Substance> dic)
        {
            foreach (var cs in _context.Set<Substance>())
            {
                if (String.IsNullOrEmpty(cs.CAS) || ! dic.ContainsKey(cs.Id))
                    continue;

                var s = dic[cs.Id];

                cs.BoilingPoint = s.BoilingPoint;
                cs.CAS = s.CAS;
                cs.Categories = s.Categories;
                cs.Density = s.Density;
                cs.FlashPoint = s.FlashPoint;
                cs.Formula = s.Formula;
                cs.HazardSymbols = s.HazardSymbols;
                cs.MeltingPoint = s.MeltingPoint;
                cs.Names = s.Names;
                cs.RefractiveIndex = s.RefractiveIndex;
                cs.Scheme = s.Scheme;
                cs.VapourPressur = s.VapourPressur;
                cs.WaterSolubility = s.WaterSolubility;
            }
            _context.SaveChanges();
        }

        public IEnumerable<Substance> GetByName(string name)
        {
            name = name.ToLower();
            return _context.Set<Substance>()
                    .Include(x => x.Names)
                    .Include(x => x.Scheme)
                    .Include(x => x.Categories)
                    .Include(x => x.Categories.Select(y => y.Parents))
                    .Where(x => x.Names.Any(
                        y => y.Value.ToLower().Contains(name)
                        ));
        }

        public IEnumerable<Substance> GetByFormula(string formula)
        {
            return _context.Set<Substance>()
                    .Include(x => x.Names)
                    .Include(x => x.Scheme)
                    .Include(x => x.Categories)
                    .Include(x => x.Categories.Select(y => y.Parents))
                    .Where(x => x.Formula == formula);
        }
    }
}
