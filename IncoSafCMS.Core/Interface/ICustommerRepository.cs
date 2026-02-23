using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface ICustommerRepository
    {
        void Add(Custommer item);
        void Edit(Custommer item);
        void Remove(string id);
        IEnumerable<Custommer> GetCustommers();
        Custommer FindByID(string id);
    }
}
