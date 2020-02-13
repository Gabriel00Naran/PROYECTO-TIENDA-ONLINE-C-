using System.Web;
using System.Web.Optimization;

namespace ProyectoIntegrador
{
    public class BundleConfig
    {
        // PARA CONFIGUARA LOS ESTILOS Y JAVA SCRIPS DEL Proyecto
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                       "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //STYLOS DE LA PLANTILLA DE BOOSTRAP BILL TORNER DE PIRATAS DEL CARIBE :v 

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                            "~/Content/vendor/fontawesome-free/css/all.min.css",
                            "~/Content/css/sb-admin-2.min.css"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Content/vendor/jquery/jquery.min.js",
                        "~/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                        "~/Content/vendor/jquery-easing/jquery.easing.min.js",
                        "~/Content/js/sb-admin-2.min.js",
                        "~/Content/vendor/chart.js/Chart.min.js",
                        "~/Content/js/demo/chart-area-demo.js",
                        "~/Content/js/demo/chart-pie-demo.js"
                         ));

            //          < !--Bootstrap core JavaScript-->|||||||


            /// ESTA COSA SE USA PARA EL LOGIN NO LO OLVIDES. CON ANGULAR DE VISUAL STUDIO
            bundles.Add(new ScriptBundle("~/bundles/angular").Include("~/Scrips/angular.min.js"));

        }
    }
}
