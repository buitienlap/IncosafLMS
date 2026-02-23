using IncoSafCMS.Core;
using IncoSafCMS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            InitalizeDb db = new InitalizeDb();
            System.Data.Entity.Database.SetInitializer(db);
        }
    }
}
