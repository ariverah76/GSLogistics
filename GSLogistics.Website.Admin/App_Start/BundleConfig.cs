using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace GSLogistics.Website.Admin.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                        "~/Scripts/select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/bootstrap.js",
                     "~/Scripts/bootstrap.datepicker.js",
                     "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                    "~/Scripts/DataTables/jquery.dataTables.min.js",
                    "~/Scripts/DataTables/dataTables.fixedColumns.min.js",
                    //"~/Scripts/DataTables/dataTables.bootstrap.min.js",
                    "~/Scripts/DataTables/dataTables.select.min.js",
                    "~/Scripts/dataTablesCellEdit.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                       "~/Scripts/smalot-datetimepicker/bootstrap-datetimepicker.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

           

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-theme.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include(
              "~/Content/smalot-datetimepicker/bootstrap-datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Content/dataTables").Include(
                "~/Content/DataTables/css/jquery.dataTables.min.css",
                //"~/Content/DataTables/css/dataTables.bootstrap.min.css",
                "~/Content/DataTables/css/fixedColumns.dataTables.min.css",
                "~/Content/DataTables/css/bootstrap.min.css",
                "~/Content/DataTables/css/select.dataTables.min.css"
               ));

            bundles.Add(new StyleBundle("~/Content/select2").Include(
                        "~/Content/css/select2.css", // NUGET
                        "~/Content/select2-bootstrap.css"));
        }
    }
}