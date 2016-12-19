using System.Collections.Generic;
using System.Data.Entity;
using Chem.Models;

namespace Chem.Managers
{
    public interface IElementManager
    {
        DbContext GetContext();
        void AddMany(IEnumerable<Element> elements);
        IEnumerable<Element> GetAll();
        Element GetBySign(string sign);
    }
}