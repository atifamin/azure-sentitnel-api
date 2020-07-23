using Newtonsoft.Json;

namespace AzureSentinel_ManagementAPI.DataConnectors.Models
{
    public class ASCDataConnectorPayload : DataConnectorPayload
    {
        public ASCDataConnectorPayload()
        {
            Kind = DataConnectorKind.Office365;
        }
         [JsonProperty("properties")] public ASCDataConnectorPropertiesPayload PropertiesPayload { get; set; }
    }
}