using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Core.DTOs
{
    public class BomMasterDTO
    {
        public int ProductId { get; set; } 
        public List<BomDetailDTO> BomDetails { get; set; } = new();
    }
    public class BomDetailDTO
    {
        public int MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }
    public class EditBomMasterDTO
    {
        public int BomId { get; set; }
        public int ProductId { get; set; }
        public List<EditBomDetailDTO> BomDetails { get; set; } = new();
    }

    public class EditBomDetailDTO
    {
        public int? BomDetailId { get; set; } // nullable for new items
        public int MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }

}
