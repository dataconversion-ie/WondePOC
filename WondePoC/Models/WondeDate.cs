using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// Wonde returns dates as an object, not a plain string.
    /// Example: { "date": "2010-03-15 00:00:00.000000", "timezone_type": 3, "timezone": "UTC" }
    /// </summary>
    public class WondeDate
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("timezone_type")]
        public int TimezoneType { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        /// <summary>Returns the date portion only (yyyy-MM-dd), or null if not set.</summary>
        public string ShortDate
        {
            get
            {
                if (string.IsNullOrEmpty(Date) || Date.Length < 10)
                    return null;
                return Date.Substring(0, 10);
            }
        }

        public override string ToString()
        {
            return ShortDate ?? string.Empty;
        }
    }
}
