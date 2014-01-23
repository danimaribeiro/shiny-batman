using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shiny_batman.OrmBatman
{
    public interface IModelQuery
    {
        void Save(BaseModel model, long id);
        void Delete(BaseModel model, long id);

        long ModelCount(BaseModel model);
        object GetModel(BaseModel model, long id);
        List<object> QueryModel(BaseModel model, int maxResults, int page);        
    }
}
