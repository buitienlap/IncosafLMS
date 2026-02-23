using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface IStandardRepository
    {
        void Add(Standard item);
        void Edit(Standard item);
        void Remove(string id);
        IEnumerable<Standard> GetStandards();
        Standard FindByID(string id);
    }
}
