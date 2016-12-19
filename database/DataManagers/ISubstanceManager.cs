using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Chem.Models;

namespace Chem.Managers
{
    public interface ISubstanceManager
    {
        DbContext GetContext();
        void AddMany(IEnumerable<Substance> substances);
        IQueryable<Substance> GetAll();
        IEnumerable<Substance> GetSeveral(int count);
        Substance GetById(int id);
        void Update(Substance s);
        void UpdateAll(Dictionary<int, Substance> dic);
        IEnumerable<Substance> GetByName(string name);
        IEnumerable<Substance> GetByFormula(string formula);
    }
}