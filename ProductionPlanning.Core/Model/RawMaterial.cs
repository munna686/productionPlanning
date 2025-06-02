using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Core.Model
{
    public class RawMaterial
    {
        [Key]
        public int MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string UOM { get; set; } = string.Empty;
        public int StockQty { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
