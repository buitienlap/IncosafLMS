using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface ISpecificationsRepository
    {
        void Add(Specifications item);
        void Edit(Specifications item);
        void Remove(string id);
        IEnumerable<Specifications> GetSpecificationses();
        Specifications FindByID(string id);
    }
}
