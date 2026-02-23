using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Data.Notify
{
    public class OnDataChangedEventArg<T> : EventArgs
    {
        public List<T> Data { set; get; }
    }
}
