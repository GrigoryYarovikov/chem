using System.Collections.Generic;
using System.Web.Http;
using Chem.Managers;
using Newtonsoft.Json;
using Chem.Models.Search;
using Chem.Services.Services;

namespace Chem.Controllers
{
    
    //[Authorize]
    public class SubstancesController : ApiController
    {
        readonly SubstancesService _service = new SubstancesService(new SubstanceManager(), new ElementManager());

        // GET api/values
        public List<SubstancePreview> GetByQuery([FromUri] string query)
        {
            var q = JsonConvert.DeserializeObject<QueryModel>(query);
            return _service.GetByQuery(q);
        }

        // GET api/values/5
        public IFullSubstanceModel Get(int id)
        {
            return _service.GetById(id);
        }

        // GET api/values/5
        public List<Reaction> GetReactionList(int id)
        {
            return _service.GetReactionList(id);
        }
    }
}
