using System.Text.Json.Serialization;

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
