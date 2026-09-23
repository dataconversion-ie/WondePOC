using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// Root of the contact_details include block, confirmed against live Wonde API response.
    /// Shared shape for both student.contact_details and contact.contact_details.
    /// </summary>
    public class ContactDetails
    {
        [JsonProperty("phones")]
        public ContactPhones Phones { get; set; }

        [JsonProperty("emails")]
        public ContactEmails Emails { get; set; }
    }

    public class ContactPhones
    {
        /// <summary>General / unclassified phone number.</summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("primary")]
        public string Primary { get; set; }

        [JsonProperty("home")]
        public string Home { get; set; }

        [JsonProperty("work")]
        public string Work { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }
    }

    public class ContactEmails
    {
        /// <summary>General / unclassified email address.</summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("primary")]
        public string Primary { get; set; }

        [JsonProperty("home")]
        public string Home { get; set; }

        [JsonProperty("work")]
        public string Work { get; set; }
    }
}
