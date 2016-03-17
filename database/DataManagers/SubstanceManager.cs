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

        public IEnumerable<Substance> GetAll()
        {
            return _context.Set<Substance>()
                    .Include(x => x.Names)
                    .Include(x => x.Scheme)
                    .Include(x => x.Categories)
                    .Include(x => x.Categories.Select(y => y.Parents));
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
