using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Chem.Models.Search;
using System.Collections.Specialized;
using System.Web;

namespace Chem.Tests
{
    public class SubstancesControllerTests
    {
        [Fact()]
        void GetBadRequestTest()
        {
            var result = DoWebAPIGetRequest<string>("Product", "Sale", null);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact()]
        void GetByIdTest()
        {
            var id = 3180;
            var name = "Углерод";
            var formula = "C";
            var meltPoint = 3500;

            var parameters = new Dictionary<string, string>();
            parameters.Add("id", id.ToString());
            var result = DoWebAPIGetRequest<FullSubstanceModel>("substances", "get", ToString(parameters, false));
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True((result.Value as FullSubstanceModel).Names.Contains(name));
            Assert.True((result.Value as FullSubstanceModel).Formula == formula);
            Assert.True((result.Value as FullSubstanceModel).MeltingPoint == meltPoint);
        }

        [Fact()]
        void GetByNameTest()
        {
            var id = 3180;
            var name = "Углерод";
            var formula = "C";

            var parameters = new Dictionary<string, string>();
            parameters.Add("q", name);
            var result = DoWebAPIGetRequest<IEnumerable<SubstancePreview>>("substances", "getByQuery", ToString(parameters));
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            var list = result.Value as IEnumerable<SubstancePreview>;
            Assert.True(list != null);
            Assert.True(list.Any(x => x.Name == name));
        }

        [Fact()]
        void GetByFormulaTest()
        {
            var id = 3180;
            var name = "Углерод";
            var formula = "C";

            var parameters = new Dictionary<string, string>();
            parameters.Add("q", formula);
            var result = DoWebAPIGetRequest<IEnumerable<SubstancePreview>>("substances", "getByQuery", ToString(parameters));
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            var list = result.Value as IEnumerable<SubstancePreview>;
            Assert.True(list != null);
            Assert.True(list.Any(x => x.Name == name));
        }

        [Fact()]
        void GetByMeltPountTest()
        {
            var id = 3180;
            var name = "Углерод";
            var formiula = "C";
            var meltPoint = 3500;

            var parameters = new Dictionary<string, string>();
            parameters.Add("mp1", meltPoint.ToString());
            var result = DoWebAPIGetRequest<IEnumerable<SubstancePreview>>("substances", "getByQuery", ToString(parameters));
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            var list = result.Value as IEnumerable<SubstancePreview>;
            Assert.True(list != null);
            Assert.True(list.Any(x => x.Name == name));
        }

        /// <summary>
        /// Submit post request with an arbitrary input model and an arbitrary output model (could be the same model).
        /// URL's of format baseURL/controller/action a la .NET MVC web api, i.e., http://something.com/api/Product/Sales
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        private Responce DoWebAPIGetRequest<U>(string controller, string action, string parameters)
        {
            string baseURL = "http://localhost/"; //add your base url for your service here

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                string url = baseURL + "api/" + controller + "/" + action;

                if (parameters != null)
                {
                    url += "?" + parameters;
                }
                var response = client.GetAsync(url);
                string error = response.Result.Content.ReadAsStringAsync().Result;

                if (response.Result.IsSuccessStatusCode)
                {
                    U result = response.Result.Content.ReadAsAsync<U>().Result;

                    return new Responce 
                    { 
                        StatusCode = response.Result.StatusCode, 
                        Value = result
                    };
                }
                else
                    return new Responce
                    {
                        StatusCode = response.Result.StatusCode
                    };
            }
        }
        public string ToString(Dictionary<string, string> parameters, bool isQuerry = true)
        {
            var sb = new StringBuilder();
            if (isQuerry)
            {
                sb.Append("query={");
                foreach (var kvp in parameters)
                {
                    sb.AppendFormat("\"{0}\":\"{1}\",",
                        WebUtility.UrlEncode(kvp.Key),
                        WebUtility.UrlEncode(kvp.Value));
                }
                sb.Append("}");
            }
            else
                foreach (var kvp in parameters)
                {
                    if (sb.Length > 0) { sb.Append("&"); }
                    sb.AppendFormat("{0}={1}",
                        WebUtility.UrlEncode(kvp.Key),
                        WebUtility.UrlEncode(kvp.Value));
                }
            return sb.ToString();
        }
    }
    
    class Responce
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Value { get; set; }
    }
}
