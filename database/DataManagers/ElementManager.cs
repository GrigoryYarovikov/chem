using Chem.DataContext;
using Chem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Managers
{
    public class ElementManager
    {
        ChemContext _context;

        public ElementManager()
        {
            _context = new ChemContext();
        }

        public void AddMany(IEnumerable<Element> elements)
        {
            _context.Set<Element>().AddRange(elements);
            _context.SaveChanges();
        }

        public IEnumerable<Element> GetElements()
        {
            return _context.Set<Element>();
        }

        public Element GetElementBySign(string sign)
        {
            return _context.Set<Element>().FirstOrDefault(x => x.Sign == sign);
        }
    }
}
