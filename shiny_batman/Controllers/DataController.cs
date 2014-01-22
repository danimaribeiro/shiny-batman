using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shiny_batman.Controllers
{
    public class DataController : Controller
    {
        public ActionResult GetFormDefinition(string id)
        {
            var retorno = new
            {
                title = "Cadastro de clientes",
                model = "Clientes",
                fields = new List<object>() { 
                    new { label = "Id", model = "id", type = "number", required = true},
                    new { label = "Nome", model = "nome", type = "text", required = true},
                    new { label = "Telefone", model = "telefone", type = "text", required = true, prefix = "phone-alt" },
                    new { label = "Estado", model = "estado", type = "text", required = true }
                }
            };

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEntity(long id)
        {
            return Json(new { Id = id, Nome = "Danimar Ribeiro", Telefone = "(48) 9801-6226", Estado = "Santa Catarina" }, JsonRequestBehavior.AllowGet);
        }

    }
}
