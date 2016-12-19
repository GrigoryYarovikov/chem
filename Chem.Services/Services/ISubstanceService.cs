using System.Collections.Generic;
using Chem.Managers;
using Chem.Models.Search;

namespace Chem.Services.Services
{
    public interface ISubstancesService
    {
        IFullSubstanceModel GetById(int id);
        List<SubstancePreview> GetByQuery(QueryModel query);
        List<Reaction> GetReactionList(int id);
    }
}
