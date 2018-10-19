using System.Web;
using System.Web.Optimization;

namespace WebApplication9
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/Scripts/jquery-3.0.0.js",
                        "~/Content/Scripts/jquery.freezeheader.js",
                        "~/Content/Scripts/umd/popper.min.js",
                        "~/Content/Scripts/jquery.table2excel.js"));

             bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Content/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/Scripts/bootstrap.js",
                      "~/Content/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/jquery-ui.css","~/Content/css/bootstrap.css", "~/Content/css/site.css", "~/Content/css/PagedList.css", "~/Content/css/grayplaceholder.css"));
        }
    }
}