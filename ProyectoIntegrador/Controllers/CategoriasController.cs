using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.Models;

namespace ProyectoIntegrador.Controllers
{
    public class CategoriasController : Controller
    {
        private TienditaDeadpoolDBEntities2 db = new TienditaDeadpoolDBEntities2();

        //Get de categoria trae todas las categorias registradas
        public PartialViewResult CategoriasPartial()
        {
            var categoriaList = db.Categoria.OrderBy(categoria => categoria.NombreCategoria).ToList();
            return PartialView(categoriaList);
        }
    }
}