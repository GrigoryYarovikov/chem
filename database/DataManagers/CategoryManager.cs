using Chem.DataContext;
using Chem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddMany(IEnumerable<Category> set)
        {
            _context.Set<Category>().AddRange(set);
            _context.SaveChanges();
        }
    }
}
