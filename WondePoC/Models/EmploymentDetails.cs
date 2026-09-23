using Newtonsoft.Json;

namespace WondePoC.Models
{
    /// <summary>
    /// Employment details for a Wonde employee.
    /// Populated when fetched with include=employment_details.
    /// Shape confirmed against live Wonde API response (all fields null in test school).
    /// Deserialised via WondeSingleWrapper&lt;EmploymentDetails&gt; — JSON shape:
    ///   "employment_details": { "data": { ... } }
    /// </summary>
    public class EmploymentDetails
    {
        /// <summary>Whether this is the employee's current role.</summary>
        [JsonProperty("current")]
        public bool? Current { get; set; }

        /// <summary>Whether the employee is classified as teaching staff.</summary>
        [JsonProperty("teaching_staff")]
        public bool? TeachingStaff { get; set; }

        [JsonProperty("teacher_number")]
        public string TeacherNumber { get; set; }

        [JsonProperty("teacher_category")]
        public string TeacherCategory { get; set; }

        [JsonProperty("staff_code")]
        public string StaffCode { get; set; }

        [JsonProperty("primary_location")]
        public string PrimaryLocation { get; set; }

        /// <summary>Plain date string (e.g. "2019-09-01") — not a WondeDate object.</summary>
        [JsonProperty("employment_start_date")]
        public string EmploymentStartDate { get; set; }

        [JsonProperty("employment_end_date")]
        public string EmploymentEndDate { get; set; }

        [JsonProperty("local_authority_start_date")]
        public string LocalAuthorityStartDate { get; set; }

        [JsonProperty("probation_end_date")]
        public string ProbationEndDate { get; set; }

        [JsonProperty("police_check_date")]
        public string PoliceCheckDate { get; set; }

        [JsonProperty("health_check_date")]
        public string HealthCheckDate { get; set; }

        [JsonProperty("employment_payroll_number")]
        public string EmploymentPayrollNumber { get; set; }

        [JsonProperty("employee_payroll_number")]
        public string EmployeePayrollNumber { get; set; }

        [JsonProperty("working_with_children_registration_number")]
        public string WorkingWithChildrenRegistrationNumber { get; set; }

        [JsonProperty("role_text")]
        public string RoleText { get; set; }

        [JsonProperty("qualified_teacher_status")]
        public string QualifiedTeacherStatus { get; set; }

        [JsonProperty("qualified_teacher_status_route")]
        public string QualifiedTeacherStatusRoute { get; set; }

        [JsonProperty("higher_level_teaching_assistant_status")]
        public string HigherLevelTeachingAssistantStatus { get; set; }
    }
}
