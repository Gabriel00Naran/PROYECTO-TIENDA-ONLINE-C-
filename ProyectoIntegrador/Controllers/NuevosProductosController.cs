using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProyectoIntegrador.Models;

namespace ProyectoIntegrador.Controllers
{
    public class NuevosProductosController : Controller
    {
        private TienditaDeadpoolDBEntities2 db = new TienditaDeadpoolDBEntities2();

        // GET: NuevosProductos
        public ActionResult Index()
        {
            var producto = db.Producto.Include(p => p.Almacen).Include(p => p.Categoria).Include(p => p.Color).Include(p => p.Modelo).Include(p => p.Usuario);
            return View(producto.ToList());
        }

        // GET: NuevosProductos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: NuevosProductos/Create
        public ActionResult Create()
        {
            ViewBag.AlmacenId = new SelectList(db.Almacen, "AlmacenId", "Almacen1");
            ViewBag.CategoriaId = new SelectList(db.Categoria, "CategoriaId", "NombreCategoria");
            ViewBag.ColorId = new SelectList(db.Color, "ColorId", "Color1");
            ViewBag.ModeloId = new SelectList(db.Modelo, "ModeloId", "Modelo1");
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "NombreUsuario");
            return View();
        }

        // POST: NuevosProductos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoId,ProductoName,Image,Precio,UsuarioId,CategoriaId,ColorId,ModeloId,AlmacenId,VentaFechaIncial,VentaFechaFinal,IsNew")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.IsNew = 1;
                producto.UsuarioId = 1;
                producto.VentaFechaIncial = DateTime.Now;
                producto.VentaFechaFinal = DateTime.Today;
                db.Producto.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlmacenId = new SelectList(db.Almacen, "AlmacenId", "Almacen1", producto.AlmacenId);
            ViewBag.CategoriaId = new SelectList(db.Categoria, "CategoriaId", "NombreCategoria", producto.CategoriaId);
            ViewBag.ColorId = new SelectList(db.Color, "ColorId", "Color1", producto.ColorId);
            ViewBag.ModeloId = new SelectList(db.Modelo, "ModeloId", "Modelo1", producto.ModeloId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "NombreUsuario", producto.UsuarioId);
            return View(producto);
        }

        // GET: NuevosProductos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlmacenId = new SelectList(db.Almacen, "AlmacenId", "Almacen1", producto.AlmacenId);
            ViewBag.CategoriaId = new SelectList(db.Categoria, "CategoriaId", "NombreCategoria", producto.CategoriaId);
            ViewBag.ColorId = new SelectList(db.Color, "ColorId", "Color1", producto.ColorId);
            ViewBag.ModeloId = new SelectList(db.Modelo, "ModeloId", "Modelo1", producto.ModeloId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "NombreUsuario", producto.UsuarioId);
            return View(producto);
        }

        // POST: NuevosProductos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoId,ProductoName,Image,Precio,UsuarioId,CategoriaId,ColorId,ModeloId,AlmacenId,VentaFechaIncial,VentaFechaFinal,IsNew")] Producto producto)
        {
            if (ModelState.IsValid)

            {
                producto.VentaFechaIncial = DateTime.Now;
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AlmacenId = new SelectList(db.Almacen, "AlmacenId", "Almacen1", producto.AlmacenId);
            ViewBag.CategoriaId = new SelectList(db.Categoria, "CategoriaId", "NombreCategoria", producto.CategoriaId);
            ViewBag.ColorId = new SelectList(db.Color, "ColorId", "Color1", producto.ColorId);
            ViewBag.ModeloId = new SelectList(db.Modelo, "ModeloId", "Modelo1", producto.ModeloId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "NombreUsuario", producto.UsuarioId);
            return View(producto);
        }

        // GET: NuevosProductos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: NuevosProductos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            db.Producto.Remove(producto);
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
