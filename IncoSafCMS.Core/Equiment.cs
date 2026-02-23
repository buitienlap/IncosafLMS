using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    public class Equiment
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        // Mã hiệu
        public string Code { get; set; }
        // Nhà sản xuất
        public string ManuFacturer { get; set; }
        // Số chế tạo
        public string No { get; set; }
        // Năm sản xuất
        public string YearOfProduction { get; set; }
        // Danh sách Thống số kỹ thuật của thiết bị
        public virtual List<Specifications> specifications { get; set; }
        // Danh sách Cơ cấu, bộ phận của thiết bị
        public virtual List<EquimentPartion> Partions { get; set; }
        // Danh sách hồ sơ kỹ thuật
        public virtual List<TechnicalDocument> TechnicalDocuments { get; set; }
    }
}
