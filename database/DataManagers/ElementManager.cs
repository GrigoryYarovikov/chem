using Chem.DataContext;
using Chem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Managers
{
    public class ElementManager
    {
        ChemContext _context;
        IEnumerable<Element> _elements;

        public ElementManager()
        {
            _context = new ChemContext();
        }

        public ElementManager(DbContext context)
        {
            _context = context as ChemContext;
        }

        public DbContext GetContext()
        {
            return _context;
        }

        public void AddMany(IEnumerable<Element> elements)
        {
            _context.Set<Element>().AddRange(elements);
            _context.SaveChanges();
        }

        public IEnumerable<Element> GetAll()
        {
            if (_elements == null)
                _elements = _context.Set<Element>();

            return _elements;
        }

        public Element GetBySign(string sign)
        {
            return _context.Set<Element>().FirstOrDefault(x => x.Sign == sign);
        }
    }
}
