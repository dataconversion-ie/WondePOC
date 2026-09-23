using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// Represents a record that has been deleted from the MIS and reported by Wonde.
    /// Used for all deletion types: students, employees, contacts, classes.
    /// Deleted endpoints return only identity fields — no full entity data.
    /// </summary>
    public class DeletedRecord
    {
        /// <summary>Wonde-encoded ID of the deleted record.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>MIS ID of the deleted record.</summary>
        [JsonProperty("mis_id")]
        public string MisId { get; set; }

        /// <summary>Date and time the record was deleted in the MIS.</summary>
        [JsonProperty("deleted_at")]
        public WondeDate DeletedAt { get; set; }
    }
}
