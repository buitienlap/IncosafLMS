using IncosafCMS.Core.DomainModels;
using IncosafCMS.Web.Dto;
using IncosafCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace IncosafCMS.Web.Helpers
{
    public class ReportHelper
    {
        const string AccreditationReportSessionKey = "AccreditationReportSessionKey";
        public static AccreditationDto AccreditationReportDataSource
        {
            get
            {
                /*
                if (Session[AccreditationReportSessionKey] == null)
                    Session[AccreditationReportSessionKey] = 0;
                return (AccreditationDto)Session[AccreditationReportSessionKey];
                */
                if (Session[AccreditationReportSessionKey] == null)
                    return null;
                return (AccreditationDto)Session[AccreditationReportSessionKey];

            }
            set { HttpContext.Current.Session[AccreditationReportSessionKey] = value; }
        }

        const string ContractReportSessionKey = "ContractReportSessionKey";
        public static Contract ContractReportDataSource
        {
            get
            {
                if (Session[ContractReportSessionKey] == null)
                    Session[ContractReportSessionKey] = 0;
                return (Contract)Session[ContractReportSessionKey];
            }
            set { HttpContext.Current.Session[ContractReportSessionKey] = value; }
        }

        const string TurnOverReportSessionKey = "TurnOverReportSessionKey";
        public static Contract TurnOverReportDataSource
        {
            get
            {
                if (Session[TurnOverReportSessionKey] == null)
                    Session[TurnOverReportSessionKey] = 0;
                return (Contract)Session[TurnOverReportSessionKey];
            }
            set { HttpContext.Current.Session[TurnOverReportSessionKey] = value; }
        }

        protected static HttpSessionState Session { get { return HttpContext.Current.Session; } }
    }
}