using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Chem
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private const string ROOT_DOCUMENT = "/Index.html";

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            var path = Request.Url.AbsolutePath;
            var isApi = path.StartsWith("/api", StringComparison.InvariantCultureIgnoreCase);
            var isImageUpload = path.StartsWith("/home", StringComparison.InvariantCultureIgnoreCase);

            if (isApi || isImageUpload)
                return;

            string url = Request.Url.LocalPath;
            if (!System.IO.File.Exists(Context.Server.MapPath(url)))
                Context.RewritePath(ROOT_DOCUMENT);
        }
    }
}
