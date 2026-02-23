using IncoSafCMS.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncoSafCMS.Core;

namespace IncoSafCMS.Infrastructure
{
    public class StandardRepository : IStandardRepository
    {
        public void Add(Standard item)
        {
            throw new NotImplementedException();
        }

        public void Edit(Standard item)
        {
            throw new NotImplementedException();
        }

        public Standard FindByID(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Standard> GetStandards()
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }
    }
}
