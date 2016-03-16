using Chem.DataContext;
using Chem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Managers
{
    public class SubstanceManager
    {
        ChemContext _context;

        public SubstanceManager()
        {
            _context = new ChemContext();
        }

        public void AddMany(IEnumerable<Substance> substances)
        {
            _context.Set<Substance>().AddRange(substances);
            _context.SaveChanges();
        }

        public IEnumerable<Substance> GetSubstances()
        {
            return _context.Set<Substance>()
                    .Include(x => x.Names)
                    .Include(x => x.Scheme)
                    .Include(x => x.Categories)
                    .Include(x => x.Categories.Select(y => y.Parents));
        }

        public Substance GetSubstanceById(int id)
        {
            return _context.Set<Substance>()
                    .Include(x => x.Names)
                    .Include(x => x.Scheme)
                    .Include(x => x.Categories)
                    .Include(x => x.Categories.Select(y => y.Parents))
                    .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Substance> GetSubstancesByName(string name)
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

        public IEnumerable<Substance> GetSubstancesByFormula(string formula)
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
