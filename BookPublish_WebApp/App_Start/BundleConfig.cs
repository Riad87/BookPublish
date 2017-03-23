using System.Web;
using System.Web.Optimization;

namespace BookPublish_WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(                        
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        "~/Scripts/scripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/nifty").Include(
                        "~/Scripts/nifty.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/form-elements.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/style.css",
                      "~/Content/PagedList.css",                     
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/niftyCorner").Include(
                      "~/Content/niftyCorners.css"));

            bundles.Add(new StyleBundle("~/Content/niftyPrint").Include(
                            "~/Content/niftyPrint.css"));


        }
    }
}
