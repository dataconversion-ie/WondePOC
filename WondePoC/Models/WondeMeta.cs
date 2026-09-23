using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// The "meta" envelope returned on every Wonde list response.
    /// </summary>
    public class WondeMeta
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}
