using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// A staff member returned by GET /v1.0/schools/{schoolId}/employees.
    /// Field list confirmed against live Wonde API response.
    /// Shape mirrors Student closely, with the following differences:
    ///   - No gender_identity field
    ///   - No contacts include (employees do not have parent/guardian contacts)
    ///   - contact_details returns real data for employees (phone, email)
    /// </summary>
    public class Employee
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

        [JsonProperty("date_of_birth")]
        public WondeDate DateOfBirth { get; set; }

        [JsonProperty("restored_at")]
        public WondeDate RestoredAt { get; set; }

        [JsonProperty("created_at")]
        public WondeDate CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public WondeDate UpdatedAt { get; set; }

        /// <summary>
        /// Phone numbers, email addresses and postal addresses for this employee.
        /// Populated when fetched with include=contact_details.
        /// Returns real data for employees (unlike the test school's students).
        /// </summary>
        [JsonProperty("contact_details")]
        public WondeSingleWrapper<ContactDetails> ContactDetails { get; set; }

        /// <summary>
        /// Employment record for this employee (payroll, dates, teacher status, etc.).
        /// Populated when fetched with include=employment_details.
        /// </summary>
        [JsonProperty("employment_details")]
        public WondeSingleWrapper<EmploymentDetails> EmploymentDetails { get; set; }

        public string FullName
        {
            get { return (Forename + " " + Surname).Trim(); }
        }

        /// <summary>Primary email — shortcut into contact_details.data.emails.primary.</summary>
        public string PrimaryEmail
        {
            get
            {
                return ContactDetails != null
                    && ContactDetails.Data != null
                    && ContactDetails.Data.Emails != null
                    ? ContactDetails.Data.Emails.Primary
                    : null;
            }
        }

        /// <summary>Primary phone — shortcut into contact_details.data.phones.phone.</summary>
        public string PrimaryPhone
        {
            get
            {
                return ContactDetails != null
                    && ContactDetails.Data != null
                    && ContactDetails.Data.Phones != null
                    ? ContactDetails.Data.Phones.Phone
                    : null;
            }
        }
    }
}
