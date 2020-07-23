using Newtonsoft.Json;

namespace AzureSentinel_ManagementAPI.Incidents.Models
{
    public class IncidentPayload
    {
        [JsonProperty("properties")]
        public IncidentPropertiesPayload PropertiesPayload { get; set; }
    }
}