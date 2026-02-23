using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface IEquimentRepository
    {
        void Add(Equiment item);
        void Edit(Equiment item);
        void Remove(string id);
        IEnumerable<Equiment> GetEquiments();
        Equiment FindByID(string id);
    }
}
