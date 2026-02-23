using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface IContractRepository
    {
        void Add(Contract item);
        void Edit(Contract item);
        void Remove(string id);
        IEnumerable<Contract> GetContracts();
        Contract FindByID(string id);
    }
}
