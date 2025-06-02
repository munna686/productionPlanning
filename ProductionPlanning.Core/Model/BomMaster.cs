using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Core.Model
{
    public class BomMaster
    {
        [Key]
        public int BomId { get; set; }

        public string BomCode { get; set; } = string.Empty;

        public string ProductCode { get; set; } = string.Empty; // Final Product Code

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public bool IsApproved { get; set; } = false;

        public virtual List<BomDetail> BomDetails { get; set; } = new List<BomDetail>();
    }
}
