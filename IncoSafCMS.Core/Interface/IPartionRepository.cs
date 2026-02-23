using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface IPartionRepository
    {
        void Add(EquimentPartion item);
        void Edit(EquimentPartion item);
        void Remove(string id);
        IEnumerable<EquimentPartion> GetEquimentPartions();
        EquimentPartion FindByID(string id);
    }
}
