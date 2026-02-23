using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface IAccreditationRepository
    {
        void Add(Accreditation item);
        void Edit(Accreditation item);
        void Remove(string id);
        IEnumerable<Accreditation> GetAccreditations();
        Accreditation FindByID(string id);
    }
}
