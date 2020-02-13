using System;
using System.Linq;
using PagedList;
using System.Web.Mvc;
using System.Net;
using ProyectoIntegrador.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace ProyectoIntegrador.Controllers
{
    public class OrdenController : Controller
    {
        private TienditaDeadpoolDBEntities2 db = new TienditaDeadpoolDBEntities2();
        // GET: Orden de compra con paginado
        public ActionResult Index(int? paginacion)
        {
            var paginaNumero = (paginacion ?? 1);
            var paginaTamaño = 5;
            var ordenLista = db.Orden.OrderByDescending(x => x.OrdenID).ToPagedList(paginaNumero, paginaTamaño);
            return View(ordenLista);
        }

        //REPORTE CON YASUO MANCO TROLL AFK CON LA LIBRERIA CRYSTAL REPORT PARA VISUAL STUDIO
        public ActionResult ExportacionLista()
        {
            ReportDocument reporteNormal = new ReportDocument();
            reporteNormal.Load(Path.Combine(Server.MapPath("~/MisReportes/OrdenLista.rpt")));
            reporteNormal.SetDataSource(db.Orden.ToList());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream ReportTodos = reporteNormal.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            ReportTodos.Seek(0, SeekOrigin.Begin);
            string guardararchivo = string.Format("OrdenLista_{0}", DateTime.Now);
            return File(ReportTodos, "appication/pdf", guardararchivo);
        }

        // GET: Orden  DETALLES DE COMPRA
        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orden = db.Orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }
    }
}