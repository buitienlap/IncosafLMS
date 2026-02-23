using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncosafCMS.Web.Dto
{
    public class AccreditationDto: Accreditation
    {
        public AppUser Tester1 { get; set; }
        public AppUser Tester2 { get; set; }

        public string QRCodeLink { get; set; }      // 02/06/2026 link QR code in ở ReportA5
        public string QRCode_GCN { get; set; }      // mapping to Equipment.gr_gcn
    }
}