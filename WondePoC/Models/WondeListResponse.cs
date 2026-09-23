using System.Collections.Generic;
using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// Generic wrapper for all Wonde list endpoints.
    /// Response shape: { "data": [ ... ], "meta": { "pagination": { ... } } }
    /// Reusable for future endpoints (classes, employees, etc.).
    /// </summary>
    public class WondeListResponse<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }

        [JsonProperty("meta")]
        public WondeMeta Meta { get; set; }
    }
}
