using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// Pagination block returned inside the "meta" object on all Wonde list responses.
    /// </summary>
    public class Pagination
    {
        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("previous")]
        public string Previous { get; set; }

        /// <summary>True when there are more pages after this one.</summary>
        [JsonProperty("more")]
        public bool More { get; set; }

        /// <summary>Null when cursor pagination is active.</summary>
        [JsonProperty("per_page")]
        public int? PerPage { get; set; }

        /// <summary>Null when cursor pagination is active.</summary>
        [JsonProperty("current_page")]
        public int? CurrentPage { get; set; }
    }
}
