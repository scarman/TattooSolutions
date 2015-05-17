using System.Web.Optimization;

namespace Tattoo.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js"));

            // Pages with jQuery validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/jquery.validate*")
                .Include("~/Scripts/bootstrap.validate.js"));
                
            // Pages with any Ajax activity
            bundles.Add(new ScriptBundle("~/bundles/jqueryajax")
                .Include("~/Scripts/jquery.unobtrusive-ajax.js")
                .Include("~/Scripts/jquery.blockUI.js")
                .Include("~/Scripts/knockout-{version}.js")
                .Include("~/Scripts/knockout.mapping-latest.js"));
                
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr-*"));

            // Pages with any Bootstrap styles
            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/respond.js")
                .Include("~/Scripts/bootbox.js")
                .Include("~/Scripts/toastr.js")
                .Include("~/Scripts/app-scripts.js"));

            // Pages with DateTime pickers
            bundles.Add(new ScriptBundle("~/bundles/datetime")
                .Include("~/Scripts/moment-with-langs.js")
                .Include("~/Scripts/bootstrap-datetimepicker.js")
                .Include("~/Scripts/bootstrap-datetimepicker-loader.js"));

            // Pages with Country / State pickers
            bundles.Add(new ScriptBundle("~/bundles/country")
                .Include("~/Scripts/bootstrap-countrypicker.js"));

            // Pages with Flot Charts
            bundles.Add(new ScriptBundle("~/bundles/flot")
                .Include("~/Scripts/flot/jquery.flot.js")
                .Include("~/Scripts/flot/jquery.flot.canvas.js")
                .Include("~/Scripts/flot/jquery.flot.selection.js")
                .Include("~/Scripts/flot/jquery.flot.stack.js")
                .Include("~/Scripts/flot/jquery.flot.pie.js"));

            // Pages with DataTables
            bundles.Add(new ScriptBundle("~/bundles/datatables")
                .Include("~/Scripts/DataTables-1.10.0/jquery.dataTables.js")
                .Include("~/Scripts/DataTables-1.10.0/dataTables.bootstrap.js"));

            // All Pages Styles
            bundles.Add(new StyleBundle("~/styles/datatables")
                .Include("~/Content/DataTables-1.10.0/css/jquery.dataTables.css")
                .Include("~/Content/DataTables-1.10.0/css/dataTables.bootstrap.css"));

            // All Pages Styles
            bundles.Add(new StyleBundle("~/styles/css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/bootstrap-theme.css")
                .Include("~/Content/bootstrap-countrypicker.css")
                .Include("~/Content/bootstrap-datetimepicker.css")
                .Include("~/Content/font-awesome.css")
                .Include("~/Content/toastr.css")
                .Include("~/Content/app-styles.css"));

#if (!DEBUG)
                BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
