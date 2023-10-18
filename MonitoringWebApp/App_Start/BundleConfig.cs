using System.Web;
using System.Web.Optimization;

namespace MonitoringWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/customscripts").Include(
                      "~/Scripts/js/admin-sharbox.js",
                      "~/Scripts/js/adminlte.min.js",
                      "~/Scripts/js/custome-js.js",
                      "~/Scripts/js/demo.js",
                      "~/Scripts/js/pages/dashboard.js",
                      "~/Content/plugins/jquery/jquery.min.js",
                      "~/Content/plugins/jquery-ui/jquery-ui.min.js",
                      "~/Content/plugins/bootstrap/js/bootstrap.bundle.min.js",
                      "~/Content/plugins/chart.js/Chart.min.js",
                      "~/Content/plugins/sparklines/sparkline.js",
                      "~/Content/plugins/jqvmap/jquery.vmap.min.js",
                      "~/Content/plugins/jqvmap/maps/jquery.vmap.usa.js",
                      "~/Content/plugins/jquery-knob/jquery.knob.min.js",
                      "~/Content/plugins/moment/moment.min.js",
                      "~/Content/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                      "~/Content/plugins/summernote/summernote-bs4.min.js",
                      "~/Content/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/customcss").Include(
                     "~/Content/css/adminlte.css",
                     "~/Content/css/custom-css.css",
                     "~/Content/css/sharbox.min.css",
                     "~/Content/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                     "~/Content/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
                     "~/Content/plugins/jqvmap/jqvmap.min.css",
                     "~/Content/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                      "~/Content/plugins/daterangepicker/daterangepicker.css",
                      "~/Content/plugins/summernote/summernote-bs4.min.css"
                     ));
        }
    }
}
