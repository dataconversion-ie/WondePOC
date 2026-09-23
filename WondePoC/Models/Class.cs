using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WondePoC.Models
{
    /// <summary>
    /// A teaching class / group returned by the Wonde API.
    /// Appears as student.classes.data[] when fetched with include=classes.
    /// Field list confirmed against live raw JSON response.
    /// </summary>
    public class Class
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mis_id")]
        public string MisId { get; set; }

        /// <summary>Class type (e.g. "teaching", "registration"). Null in test data.</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>Display name of the class, e.g. "13A/Co1".</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Short code for the class. Null if not set in MIS.</summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// When fetched with include=classes only, this is a plain Wonde ID string.
        /// When fetched with include=classes.subject, this becomes a nested subject object.
        /// JToken handles both cases safely.
        /// </summary>
        [JsonProperty("subject")]
        public JToken Subject { get; set; }

        /// <summary>Returns the subject as a plain Wonde ID string regardless of include depth.</summary>
        public string SubjectId
        {
            get
            {
                if (Subject == null) return null;
                if (Subject.Type == JTokenType.String) return Subject.Value<string>();
                return Subject["id"] != null ? Subject["id"].Value<string>() : null;
            }
        }

        [JsonProperty("alternative")]
        public string Alternative { get; set; }

        [JsonProperty("priority")]
        public int? Priority { get; set; }

        /// <summary>
        /// Academic year reference. String ID or nested object depending on includes.
        /// Use JToken to avoid deserialization errors.
        /// </summary>
        [JsonProperty("academic_year")]
        public JToken AcademicYear { get; set; }

        /// <summary>
        /// Year group reference. String ID or nested object depending on includes.
        /// Use JToken to avoid deserialization errors.
        /// </summary>
        [JsonProperty("year_group")]
        public JToken YearGroup { get; set; }

        [JsonProperty("restored_at")]
        public WondeDate RestoredAt { get; set; }

        [JsonProperty("created_at")]
        public WondeDate CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public WondeDate UpdatedAt { get; set; }
    }
}
