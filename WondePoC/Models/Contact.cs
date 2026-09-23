using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// A contact (parent / guardian / carer) linked to a student.
    /// Returned when students are fetched with include=contacts.
    /// </summary>
    public class Contact
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mis_id")]
        public string MisId { get; set; }

        [JsonProperty("forename")]
        public string Forename { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        /// <summary>True if this is the primary / highest-priority contact.</summary>
        [JsonProperty("primary")]
        public bool Primary { get; set; }

        /// <summary>Lower number = higher priority. Used to order contacts.</summary>
        [JsonProperty("priority")]
        public int? Priority { get; set; }

        [JsonProperty("created_at")]
        public WondeDate CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public WondeDate UpdatedAt { get; set; }

        /// <summary>
        /// Phone numbers, email addresses and postal addresses for this contact.
        /// Populated when fetched with include=contacts.contact_details.
        /// Wonde wraps single-object includes as { "data": { ... } }.
        /// </summary>
        [JsonProperty("contact_details")]
        public WondeSingleWrapper<ContactDetails> ContactDetails { get; set; }

        public string FullName
        {
            get { return ((Forename ?? "") + " " + (Surname ?? "")).Trim(); }
        }
    }
}
