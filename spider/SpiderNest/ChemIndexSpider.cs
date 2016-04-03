using Abot.Crawler;
using Abot.Poco;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CsQuery;
using System.Net;
using Chem.Models;
using Common.Helpers;
using Chem.Managers;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;

namespace Spider
{
    class ChemIndexSpider : BaseSpider
    {
        SubstanceManager _substances;

        public ChemIndexSpider()
        {
            _substances = new SubstanceManager();
        }

        public override string Crawl()
        {
            Console.WriteLine("Start");
            string ans = "";

            var ward = new CriticalExCounter();

            var t = new Stopwatch();
            t.Start();

            //var catList = new List<SpiderSubstanceCI>();

            var addr = "http://www.chemindex.com/";

            var initialRange = _substances.GetAll().ToDictionary(x => x.Id);
            var resultRange = new Dictionary<int, Substance>();

            Console.WriteLine(String.Format("\nTime elapsed to dic: {0}\n", t.Elapsed));

            int count = 0;
            int all = initialRange.Count();

            foreach (var substance in initialRange)
            {
                System.Threading.Thread.Sleep(GetRnd());
                ++count;

                if (count % 25 == 0)
                {
                    Console.WriteLine(String.Format("\nElements: {0} of {1}. Time elapsed: {2}\n", count, all, t.Elapsed));
                }

                if (!substance.Value.CAS.HasValue())
                    continue;

                var crawler = GetCrawler();

                var uri = new Uri(addr + substance.Value.CAS + "-cas.html");
                var cToken = new CancellationTokenSource();

                try
                {
                    crawler.CrawlBag.elements = new ConcurrentBag<SpiderSubstanceCI>();
                    var result = crawler.Crawl(uri, cToken);

                    var element = (crawler.CrawlBag.elements as ConcurrentBag<SpiderSubstanceCI>).First();
                    //if (element.Name.Length == 0)
                    //    element.Name = "sas";
                    //element.CatId = i;
                    //catList.Add(element);

                    if (result.ErrorOccurred)
                    {
                        ans += String.Format("Crawl of {0} completed with error: {1}\n",
                            result.RootUri.AbsoluteUri,
                            result.ErrorException.Message);
                    }
                    
                    substance.Value.BoilingPoint = element.BoilingPoint;
                    substance.Value.Density = element.Density;
                    substance.Value.FlashPoint = element.FlashPoint;
                    substance.Value.HazardSymbols = element.HazardSymbols;
                    substance.Value.MeltingPoint = element.MeltingPoint;
                    substance.Value.RefractiveIndex = element.RefractiveIndex;
                    substance.Value.VapourPressur = element.VapourPressur;
                    substance.Value.WaterSolubility = element.WaterSolubility;

                    resultRange.Add(substance.Key, substance.Value);

                    ward.Tick();
                }
                catch
                {
                    ward.Bad();
                    if (ward.IsCritical())
                    {
                        Console.WriteLine(String.Format("Time elapsed scan: {0}", t.Elapsed));
                        _substances.UpdateAll(resultRange);
                        Console.WriteLine(String.Format("Time elapsed update: {0}", t.Elapsed));
                    }
                }                
            }
            Console.WriteLine(String.Format("Time elapsed scan: {0}", t.Elapsed));
            _substances.UpdateAll(resultRange);
            Console.WriteLine(String.Format("Time elapsed update: {0}", t.Elapsed));
            //var trueCatList = new List<Category>();
            //foreach (var item in catList)
            //{
            //    trueCatList.Add(new Category
            //    {
            //        Name = item.Name
            //    });
            //}
            //foreach (var item in catList)
            //{
            //    var cat = trueCatList.First(x => x.Name == item.Name);
            //    if (item.Parents != null)
            //        cat.Parents = trueCatList.Where(x => item.Parents.Contains(x.Name)).ToList();
            //}

            //_categories.AddMany(trueCatList);
            return null;// ans;
        }

        protected override void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            var crawledPage = e.CrawledPage;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("Failed {0}", crawledPage.Uri.AbsoluteUri);
                return;
            }
            else
                Console.WriteLine("Succeeded {0}", crawledPage.Uri.AbsoluteUri);

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
            {
                Console.WriteLine("Page had no content {0}", crawledPage.Uri.AbsoluteUri);
                return;
            }

            var element = ParsePage(crawledPage.Content.Text);
            (e.CrawlContext.CrawlBag.elements as ConcurrentBag<SpiderSubstanceCI>).Add(element);

            e.CrawlContext.CancellationTokenSource.Cancel();
        }

        SpiderSubstanceCI ParsePage(string page)
        {
            CQ dom = page;
            var items = dom["table tr"];
            var hazards = dom["table b"].Elements;
            if (items.Any())
            {
                var dataPairs = items.Select(x =>
                {
                    CQ elemDom = x.InnerHTML;
                    return elemDom["td"].Elements.Select(y => y.InnerText.Trim()).ToArray();
                }).Where(x => x.Count() == 2).ToList();


                var elem = new SpiderSubstanceCI
                {
                    BoilingPoint = GetDoubleOrNull(dataPairs, "Boiling Point"),
                    Density = GetDoubleOrNull(dataPairs, "Density"),
                    FlashPoint = GetDoubleOrNull(dataPairs, "Flash Point"),
                    MeltingPoint = GetDoubleOrNull(dataPairs, "Melting Point"),
                    RefractiveIndex = GetDoubleOrNull(dataPairs, "Refractive Index"),
                    VapourPressur = GetDoubleOrNull(dataPairs, "Vapour Pressur"),
                    HazardSymbols = hazards.Count() == 0 ? null : hazards.Select(x => x.InnerText).Aggregate((x,y) => {return x + ',' + y;}),//GetStringOrNull(dataPairs, "Hazard Symbols"),
                    WaterSolubility = GetBoolOrNullWater(dataPairs, "Water Solubility")
                };
                //var elem = dataPairs.Select(x =>
                //{
                //    CQ elemDom = x.InnerHTML;
                //    return new SpiderSubstanceCI
                //    {
                //        Name = elemDom["b:contains('test')"].First().Text().CutStar(),
                //        Parents = elemDom["a"].Elements.Select(y => y.InnerText.HtmlDecode().CutStar()).ToList()
                //    };
                //}).Aggregate((first, second) => { first.Parents.AddRange(second.Parents); return first; });
                return elem;
            }
            else
            {
                //items = dom["h1"];
                var elem = new SpiderSubstanceCI
                {
                    //BoilingPoint = items.First().Text().Split(',').First().CutStar()
                };
                return elem;
            }
        }

        private Regex _patt = new Regex(@"([+-]?)(?=\d|\.\d)\d*(\.\d*)?([Ee]([+-]?\d+))?");
        private Regex _waterPatt = new Regex(@"\s.*L");

        private double? GetDoubleOrNull(List<string[]> dataPairs, string p)
        {
            var pair = dataPairs.FirstOrDefault(x => x[0] == p);
            if (pair != null)
            {
                var number = _patt.Match(pair[1]).Value;
                if (number.HasValue())
                    return Double.Parse(number, CultureInfo.InvariantCulture);
            }
            return null;
        }

        private bool? GetBoolOrNullWater(List<string[]> dataPairs, string p)
        {
            var pair = dataPairs.FirstOrDefault(x => x[0] == p);
            if (pair != null)
            {
                var number = _patt.Match(pair[1]).Value;
                var units = _waterPatt.Match(pair[1]).Value;
                var mul = GetWaterMultiplier(units);
                if (mul == 0)
                    return null;
                if (number.HasValue())
                    return Double.Parse(number, CultureInfo.InvariantCulture) * mul > 0.001;
                else if (pair[1].ToLower().Contains("miscible") || pair[1].ToLower().Contains("soluble"))
                    return true;
                else if (pair[1].ToLower().Contains("react") || pair[1].ToLower().Contains("decompose"))
                    return false;
                else
                    return null;
            }
            return null;
        }

        private string GetStringOrNull(List<string[]> dataPairs, string p)
        {
            var pair = dataPairs.FirstOrDefault(x => x[0] == p);
            if (pair != null)
            {
                return pair[1];
            }
            return null;
        }

        private double GetWaterMultiplier(string s)
        {
            if (!s.HasValue())
                return 1;

            var split = s.Trim().Split('/');
            if (split.Count() < 2)
                return 0;

            var first = split[0];
            var second = split[1];
            var firstMul = .0;
            var secondMul = .0;

            if (first == "g")
                firstMul = 1;
            if (first == "mg")
                firstMul = 0.001;
            if (second == "L")
                secondMul = 0.1;
            if (second == "100 mL")
                secondMul = 1;
            return firstMul * secondMul;
        }

        private int GetRnd()
        {
            return new Random().Next(1000, 2500);
        }
    }

    class CriticalExCounter
    {
        int critical = 5;
        int good = 0;
        int bad = 0;

        public bool IsCritical()
        {
            return bad > critical;
        }

        public void Bad()
        {
            good = 0;
            ++bad;
        }

        public void Tick()
        {
            if (good > critical)
                bad = 0;
            ++good;
        }
    }
}
