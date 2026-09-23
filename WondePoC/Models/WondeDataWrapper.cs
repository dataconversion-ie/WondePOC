using System.Collections.Generic;
using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// Wonde wraps every nested include (e.g. contacts inside a student) as
    ///   { "data": [ ... ] }
    /// with no "meta" block — distinct from the top-level WondeListResponse&lt;T&gt;
    /// which also carries pagination meta.
    /// </summary>
    public class WondeDataWrapper<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }
}
