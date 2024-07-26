using System.Text.Json.Serialization;

namespace McenterTest.Models
{
    public class NcPackage
    {
        [JsonPropertyName("Name")]
        public required string Name { get; set; }
        
        [JsonPropertyName("WorkpieceId")]
        public string WorkpieceId { get; set; } = string.Empty;
        
        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; } = string.Empty;
        
        [JsonPropertyName("TrialRun")]
        public string TrialRun { get; set; } = string.Empty;
        
        [JsonPropertyName("ManualVerificationAfterTrialRun")]
        public string ManualVerificationAfterTrialRun { get; set; } = string.Empty;
    }
    
    
}
