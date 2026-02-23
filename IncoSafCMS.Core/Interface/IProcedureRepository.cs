using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface IProcedureRepository
    {
        void Add(Procedure item);
        void Edit(Procedure item);
        void Remove(string id);
        IEnumerable<Procedure> GetProcedures();
        Procedure FindByID(string id);
    }
}
