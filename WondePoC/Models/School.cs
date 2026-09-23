using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WondePoC.Models
{
    /// <summary>
    /// Represents a school record returned by GET /v1.0/schools.
    /// 'region' and 'address' are nested objects in the Wonde response,
    /// stored as JToken so they don't break deserialization.
    /// </summary>
    public class School
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("urn")]
        public string Urn { get; set; }

        [JsonProperty("la_code")]
        public string LaCode { get; set; }

        [JsonProperty("establishment_number")]
        public string EstablishmentNumber { get; set; }

        [JsonProperty("mis_id")]
        public string MisId { get; set; }

        /// <summary>Nested object — use RegionName for a simple string.</summary>
        [JsonProperty("region")]
        public JToken Region { get; set; }

        /// <summary>The school's Wonde API domain (may differ from api.wonde.com for some regions).</summary>
        [JsonProperty("domain")]
        public JToken Domain { get; set; }

        /// <summary>Extracts a display-friendly region name from the nested region object.</summary>
        public string RegionName
        {
            get
            {
                if (Region == null) return null;
                if (Region.Type == JTokenType.String) return Region.Value<string>();
                return Region["name"] != null ? Region["name"].Value<string>() : Region.ToString();
            }
        }

        public string DomainName
        {
            get
            {
                if (Domain == null) return null;
                if (Domain.Type == JTokenType.String) return Domain.Value<string>();
                return Domain["domain"] != null ? Domain["domain"].Value<string>() : Domain.ToString();
            }
        }
    }
}
