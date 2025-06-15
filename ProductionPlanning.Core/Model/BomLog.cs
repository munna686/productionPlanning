using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductionPlanning.Core.Model
{
    public class BomLog
    {
        public int BomLogId { get; set; }
        public int BomId { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndingTime { get; set; }
        public string StartedBy { get; set; } = string.Empty;
        public bool isFinished { get; set; }
        public bool isActive { get; set; }
        [JsonIgnore]
        public virtual BomMaster? BomMaster { get; set; }

    }
}
