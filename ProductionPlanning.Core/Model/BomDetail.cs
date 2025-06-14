using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductionPlanning.Core.Model
{
    public class BomDetail
    {
        public int BomDetailId { get; set; }

        public int BomId { get; set; } // FK to BomMaster

        public int MaterialId { get; set; } 

        public decimal Quantity { get; set; }

        public string UOM { get; set; } = string.Empty;

        public string? Remarks { get; set; }
        [JsonIgnore]
        public virtual BomMaster? BomMaster { get; set; }
        [JsonIgnore]
        public virtual RawMaterial? RawMaterial { get; set; }
    }
}
