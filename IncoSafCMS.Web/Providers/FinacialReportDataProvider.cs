using IncosafCMS.Data;
using IncosafCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncosafCMS.Web.Providers
{
    public class FinacialReportDataProvider
    {
        const string FinacialReportDataContextKey = "FinacialReportDataContextKey";
        public static IncosafCMSContext DB
        {
            get
            {
                if (HttpContext.Current.Session[FinacialReportDataContextKey] == null)
                {
                    HttpContext.Current.Session[FinacialReportDataContextKey] = new IncosafCMSContext("name=AppContext", new DebugLogger());
                }
                return (IncosafCMSContext)HttpContext.Current.Session[FinacialReportDataContextKey];
            }
        }

        //public static IQueryable<TienVeViewModel> GetAllPaymentsForReport()
        //{
        //    var payments = DB.Database
        //        .SqlQuery<TienVeViewModel>("GetAllPaymentsForReport")
        //        .AsQueryable();
        //    return payments;
        //}
    }
}