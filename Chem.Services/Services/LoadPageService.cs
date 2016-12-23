using Chem.Models.Search;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CsQuery;
using Common.Helpers;

namespace Chem.Services
{
    public class LoadPageService
    {
        private static string GetWebPageAsString(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream stream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, Encoding.Default);
                return streamReader.ReadToEnd();
            }
            catch
            {
                return null;
            }
        }  

        public static List<Reaction> LoadReactionList(string q, bool organic = true)
        {
            if (organic)
            {
                var url = "http://www.xumuk.ru/organic_reactions/search.php?query=" + q;
                var data = GetWebPageAsString(url);
                if (data == null)
                    return null;
                
                CQ dom = data;
                var items = dom["table td a"].Where(x => x["href"].StartsWith("data/"));
                if (items != null && items.Any())
                {
                    var reactions = items.Select(x => new Reaction
                        {
                            Href = x["href"].Split('/', '\\', '.')[1],
                            ImgUrl = "http://www.xumuk.ru/organic_reactions/" + x.ChildElements.First()["src"]
                        });
                    return reactions.ToList();
                }
            }
            else
            {
                var url = "http://www.xumuk.ru/inorganic_reactions/search.php?query=" + q;
                var data = GetWebPageAsString(url);
                if (data == null)
                    return null;

                CQ dom = data;
                var items = dom["table td a"].Where(x => x["href"].StartsWith("http://xumuk.ru/inorganic_reactions/-/")); ;
                if (items != null && items.Any())
                {
                    var reactions = items.Select(x => new Reaction
                    {
                        TextValue = x.InnerHTML.HtmlDecode()
                    });
                    return reactions.ToList();
                }
            }
            return null;
        }
    }
}