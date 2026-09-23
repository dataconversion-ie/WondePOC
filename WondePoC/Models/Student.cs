using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// Maps to a single student record returned by the Wonde API.
    /// GET /v1.0/schools/{schoolId}/students?include=contacts
    /// Field list confirmed against live raw JSON response.
    /// </summary>
    public class Student
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mis_id")]
        public string MisId { get; set; }

        [JsonProperty("upi")]
        public string Upi { get; set; }

        [JsonProperty("initials")]
        public string Initials { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("forename")]
        public string Forename { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("middle_names")]
        public string MiddleNames { get; set; }

        [JsonProperty("legal_forename")]
        public string LegalForename { get; set; }

        [JsonProperty("legal_surname")]
        public string LegalSurname { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("gender_identity")]
        public string GenderIdentity { get; set; }

        [JsonProperty("date_of_birth")]
        public WondeDate DateOfBirth { get; set; }

        [JsonProperty("restored_at")]
        public WondeDate RestoredAt { get; set; }

        [JsonProperty("created_at")]
        public WondeDate CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public WondeDate UpdatedAt { get; set; }

        /// <summary>
        /// Classes this student belongs to.
        /// Populated when fetched with include=classes.
        /// Wonde wraps array includes as { "data": [...] }.
        /// </summary>
        [JsonProperty("classes")]
        public WondeDataWrapper<Class> Classes { get; set; }

        /// <summary>
        /// Contacts (parents / guardians) linked to this student.
        /// Populated when fetched with include=contacts.
        /// Wonde wraps array includes as { "data": [...] }.
        /// Null if the include was not requested or no contacts exist.
        /// </summary>
        [JsonProperty("contacts")]
        public WondeDataWrapper<Contact> Contacts { get; set; }

        /// <summary>
        /// Phone numbers, email addresses and postal addresses for this student directly.
        /// Populated when fetched with include=contact_details.
        /// Wonde wraps single-object includes as { "data": { ... } }.
        /// </summary>
        [JsonProperty("contact_details")]
        public WondeSingleWrapper<ContactDetails> ContactDetails { get; set; }

        public string FullName
        {
            get { return (Forename + " " + Surname).Trim(); }
        }
    }
}
