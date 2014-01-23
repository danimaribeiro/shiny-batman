using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using shiny_batman.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace shiny_batman.Controllers
{
    public class DataController : Controller
    {

        public ActionResult ListEntities(string model, int page, int maxResults)
        {
            OrmBatman.Postgres.PostgresMetadata metadata = new OrmBatman.Postgres.PostgresMetadata();
            var modelo = metadata.InspectModel(model);

            OrmBatman.Postgres.ModelQuery query = new OrmBatman.Postgres.ModelQuery();
            var items = query.QueryModel(modelo, maxResults, page - 1);
            var total = query.ModelCount(modelo);

            var retorno = new
            {
                columns = (from p in modelo.Properties
                           select new
                           {
                               description = p.ShortDescription,
                               name = p.Name,
                               link = p.IsPrimaryKey,
                               order = p.IsPrimaryKey ? "asc" : null
                           }).ToList(),

                values = items,
                count = total,
                results = items.Count
            };
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFormDefinition(string model)
        {
            OrmBatman.Postgres.PostgresMetadata metadata = new OrmBatman.Postgres.PostgresMetadata();
            var modelo = metadata.InspectModel(model);

            var retorno = new
            {
                title = modelo.Name,
                model = modelo.TableName,
                fields = (from p in modelo.Properties
                          select new { label = p.ShortDescription, model = p.Name, type = p.Type, required = p.Required, prefix = false }).ToList()
            };

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEntity(int id, string model)
        {
            OrmBatman.Postgres.PostgresMetadata metadata = new OrmBatman.Postgres.PostgresMetadata();
            var modelo = metadata.InspectModel(model);

            OrmBatman.Postgres.ModelQuery query = new OrmBatman.Postgres.ModelQuery();
            var retorno = query.GetModel(modelo, id);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveEntity(int id, string model, string jsonData)
        {
            OrmBatman.Postgres.PostgresMetadata metadata = new OrmBatman.Postgres.PostgresMetadata();
            var modelo = metadata.InspectModel(model);

            modelo = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(jsonData, modelo);

            OrmBatman.Postgres.ModelQuery query = new OrmBatman.Postgres.ModelQuery();
            query.Save(modelo, id);
            return Json(modelo, JsonRequestBehavior.DenyGet);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
            };
        }

    }
}
