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
    public class CategoryManager
    {
        ChemContext _context;

        public CategoryManager()
        {
            _context = new ChemContext();
        }

        public CategoryManager(DbContext context)
        {
            _context = context as ChemContext;
        }

        public DbContext GetContext()
        {
            return _context;
        }

        public void AddMany(IEnumerable<Category> set)
        {
            _context.Set<Category>().AddRange(set);
            _context.SaveChanges();
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Set<Category>().Include(x => x.Parents);
        }
    }
}
