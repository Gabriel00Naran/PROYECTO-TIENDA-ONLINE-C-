using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProyectoIntegrador.Models;

namespace ProyectoIntegrador.Controllers
{
    public class ProductoController : Controller
    {
        TienditaDeadpoolDBEntities2 db = new TienditaDeadpoolDBEntities2();
        // GET: PRODUCTOS/DETALLES/
        public ActionResult Detalles(int? id)
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
        //ESTA SECCION ES LA PAGINACION DE PRODUCTOS
        public PartialViewResult Index(int? paginacion, int? categoria)
        {
            int paginaNumero = paginacion ?? 1;
            int paginaTamaño = 5;
            if (categoria != null)
            {
                ViewBag.categoria = categoria;
                var productoList = db.Producto
                    .OrderByDescending(x => x.ProductoId)
                    .Where(x => x.CategoriaId == categoria)
                    .ToPagedList(paginaNumero, paginaTamaño);
                return PartialView(productoList);
            }
            else
            {
                var productoList = db.Producto.
                    OrderByDescending(x => x.ProductoId).ToPagedList(paginaNumero, paginaTamaño);
                return PartialView(productoList);

            }
        }

        //INNER JOIN PARA LISTAR LOS DATOS DEL PRODUCTO
        public ActionResult inner()
        {
            List<Producto> LSRT;
            LSRT = (from d in db.Producto
                    select new Producto
                    {
                        ProductoId = d.ProductoId,
                        ProductoName = d.ProductoName,
                    }).ToList();

            return View(LSRT);
        }////INNER JOIN COMPLETO PARA BUSQUEDA E UNION DE DATOS
         //---------------------------------------------------------------------------------------////

        //BUSCADOR//
        public PartialViewResult buscar()
        {
            var buscarProducto = db.Producto.OrderBy(producto => producto.ProductoName.Trim()).ToList();
            return PartialView(buscarProducto);
       
        }
    }
}