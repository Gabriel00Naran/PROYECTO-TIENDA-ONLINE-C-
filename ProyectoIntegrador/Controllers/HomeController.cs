using System.Linq;
using System.Web.Mvc;
using ProyectoIntegrador.Models;

namespace ProyectoIntegrador.Controllers
{
    public class HomeController : Controller
    {
        private TienditaDeadpoolDBEntities2 db = new TienditaDeadpoolDBEntities2();
        public ActionResult Index()
        {
            return View(db.Nuevo.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "pagina del proyecto INTEGRADOR 6TO semestre 2020";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Gabriel Naran DP";

            return View();
        }
    }
}