using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shiny_batman.OrmBatman
{
    public interface IMetadata
    {
        List<BaseModel> InspectDatabase();

        BaseModel InspectModel(string model);
    }
}
