using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// Wonde wraps single-object includes (e.g. contact_details) as
    ///   { "data": { ... } }
    /// where "data" is a single object, not an array.
    /// Contrast with <see cref="WondeDataWrapper{T}"/> which handles array includes.
    /// </summary>
    public class WondeSingleWrapper<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
