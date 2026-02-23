using IncosafCMS.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.DataTool
{
    interface IDatatool
    {
        List<T> ImportToDatabase<T>(string exelPath, bool confirm = true);
        void Commit();
        List<T> GetAll<T>();
    }
    
}
