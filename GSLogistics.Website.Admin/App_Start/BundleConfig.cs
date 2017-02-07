﻿using System;
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

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/bootstrap.js",
                     "~/Scripts/bootstrap.datepicker.js",
                     "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                    "~/Scripts/DataTables/jquery.dataTables.js",
                    "~/Scripts/DataTables/dataTables.bootstrap.js",
                    "~/Scripts/DataTables/dataTables.select.js",
                    "~/Scripts/Editor/dataTables.editor.js",
                    "~/Scripts/Editor/editor.bootstrap.js"
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
                "~/Content/DataTables/css/dataTables.bootstrap.css",
                "~/Content/DataTables/css/select.dataTables.min.css",
                "~/Content/Editor/dataTables.editor.css",
                "~/Content/Editor/editor.bootstrap.css"
                ));
        }
    }
}