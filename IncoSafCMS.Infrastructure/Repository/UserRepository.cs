using IncoSafCMS.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncoSafCMS.Core;

namespace IncoSafCMS.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        public void Add(User item)
        {
            throw new NotImplementedException();
        }

        public void Edit(User item)
        {
            throw new NotImplementedException();
        }

        public User FindByID(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }
    }
}
