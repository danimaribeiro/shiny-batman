using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shiny_batman.Controllers
{
    public class MenuController : Controller
    {
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Todos()
        {
            var retorno = new List<object>() { 
                new { id= 1, description = "Clientes", model = "tb_pessoa", action = "tab", icon = "fa fa-edit fa-fw" }, 
                new { id= 2, description = "Produtos", model = "tb_produto", action = "tab", icon = "fa fa-edit fa-fw" }, 
                new { id= 3, description = "Pedidos de venda", model = "tb_pedido_venda", action = "tab", icon = "fa fa-edit fa-fw" }, 
                new { id= 4, description = "Compras", model = "tb_pedido_compra", action = "tab", icon = "fa fa-edit fa-fw" }
            };

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

    }
}
