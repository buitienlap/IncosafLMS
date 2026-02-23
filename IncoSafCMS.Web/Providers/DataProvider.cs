using IncosafCMS.Data;
using IncosafCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncosafCMS.Web.Providers
{
    public class DataProvider
    {
        const string DataProviderContextKey = "DataProviderContextKey";
        public static IncosafCMSContext DB
        {
            get
            {
                if (HttpContext.Current.Session[DataProviderContextKey] == null)
                {
                    HttpContext.Current.Session[DataProviderContextKey] = new IncosafCMSContext("name=AppContext", new DebugLogger());
                }
                return (IncosafCMSContext)HttpContext.Current.Session[DataProviderContextKey];
            }
        }
    }
}