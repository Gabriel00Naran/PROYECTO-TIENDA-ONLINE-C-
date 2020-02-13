using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProyectoIntegrador.Models;

namespace ProyectoIntegrador.Controllers
{
    public class NuevoController : Controller
    {
        private TienditaDeadpoolDBEntities2 db = new TienditaDeadpoolDBEntities2();

        // GET: Nuevo
        public ActionResult Index()
        {
            var nuevo = db.Nuevo.Include(n => n.Usuario);
            return View(nuevo.ToList());
        }

        // GET: Nuevo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nuevo nuevo = db.Nuevo.Find(id);
            if (nuevo == null)
            {
                return HttpNotFound();
            }
            return View(nuevo);
        }

        // GET: Nuevo/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "NombreUsuario");
            return View();
        }

        // POST: Nuevo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NuevosId,UsuarioId,Titulo,DescripcionCorta,Image,Content,CreateDate,Status")] Nuevo nuevo)
        {
            if (ModelState.IsValid)
            {
                nuevo.UsuarioId = 1;
                nuevo.Status = 1;
                nuevo.CreateDate= DateTime.Now;
                db.Nuevo.Add(nuevo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "NombreUsuario", nuevo.UsuarioId);
            return View(nuevo);
        }

        // GET: Nuevo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nuevo nuevo = db.Nuevo.Find(id);
            if (nuevo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "NombreUsuario", nuevo.UsuarioId);
            return View(nuevo);
        }

        // POST: Nuevo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NuevosId,UsuarioId,Titulo,DescripcionCorta,Image,Content,CreateDate,Status")] Nuevo nuevo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nuevo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "NombreUsuario", nuevo.UsuarioId);
            return View(nuevo);
        }

        // GET: Nuevo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nuevo nuevo = db.Nuevo.Find(id);
            if (nuevo == null)
            {
                return HttpNotFound();
            }
            return View(nuevo);
        }

        // POST: Nuevo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nuevo nuevo = db.Nuevo.Find(id);
            db.Nuevo.Remove(nuevo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
