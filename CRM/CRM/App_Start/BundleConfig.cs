using System.Web;
using System.Web.Optimization;

namespace CRM
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/chat").Include(
                        "~/Scripts/chat/chat.js",
                        "~/Scripts/notifications/notifications.js",
                        "~/Scripts/delete/delete.js",
                        "~/Scripts/join_requests/join_requests.js"
                        ));


            //MODIFY THE NAME OF THIS BUNDLE TO INCLUDE ALL THIRD PARTY LIBRARIES
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootbox.min.js",
                        "~/Scripts/underscore-min.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/flatly.min.css",
                      "~/Content/animate.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/Site.css"));
        }
    }
}
