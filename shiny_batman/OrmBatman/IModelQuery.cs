using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shiny_batman.OrmBatman
{
    public interface IModelQuery
    {
        long ModelCount(BaseModel model);
        object GetModel(BaseModel model, long id);
        List<object> QueryModel(BaseModel model, int maxResults, int page);        
    }
}
