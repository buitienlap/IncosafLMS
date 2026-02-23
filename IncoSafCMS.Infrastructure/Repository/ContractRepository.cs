using IncoSafCMS.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncoSafCMS.Core;

namespace IncoSafCMS.Infrastructure
{
    public class ContractRepository : IContractRepository
    {
        public void Add(Contract item)
        {
            throw new NotImplementedException();
        }

        public void Edit(Contract item)
        {
            throw new NotImplementedException();
        }

        public Contract FindByID(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contract> GetContracts()
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }
    }
}
