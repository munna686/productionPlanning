using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Core.Model
{
    public class BomDetail
    {
        public int BomDetailId { get; set; }

        public int BomId { get; set; } // FK to BomMaster

        public string MaterialCode { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public string UOM { get; set; } = string.Empty;

        public string? Remarks { get; set; }

        public virtual BomMaster? BomMaster { get; set; }
    }
}
