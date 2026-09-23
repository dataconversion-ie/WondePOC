using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using WondePoC.Exceptions;
using WondePoC.Models;

namespace WondePoC.Services
{
    /// <summary>
    /// Lightweight synchronous Wonde API client built on HttpWebRequest.
    /// All methods block the calling thread — safe to call directly from
    /// ASP.NET WebForms code-behinds without deadlock risk.
    /// </summary>
    public class WondeApiClient
    {
        private readonly string _token;
        private readonly string _baseUrl;   // e.g. "https://api.wonde.com/v1.0/"

        /// <summary>Timeout in milliseconds for each individual HTTP request (default 30 s).</summary>
        public int TimeoutMs { get; set; } = 30000;

        public WondeApiClient(string token, string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token");
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException("baseUrl");

            _token   = token;
            _baseUrl = baseUrl.TrimEnd('/') + "/";
        }

        // -------------------------------------------------------------------------
        // Schools  — use this first to discover the encoded Wonde school ID
        // -------------------------------------------------------------------------

        /// <summary>
        /// Returns all schools your API token has been approved to access.
        /// Use the <see cref="School.Id"/> value as the schoolId for other calls.
        /// </summary>
        public List<School> GetApprovedSchools()
        {
            var all = new List<School>();

            // cursor=true: Wonde returns a full Next URL in meta.pagination.next
            string nextUrl = "schools?cursor=true";

            while (nextUrl != null)
            {
                string json = Get(nextUrl);

                var response = JsonConvert.DeserializeObject<WondeListResponse<School>>(json);

                if (response == null || response.Data == null)
                    break;

                all.AddRange(response.Data);

                bool hasMore = response.Meta != null
                    && response.Meta.Pagination != null
                    && response.Meta.Pagination.More;

                nextUrl = hasMore ? response.Meta.Pagination.Next : null;
            }

            return all;
        }

        // -------------------------------------------------------------------------
        // Employees
        // -------------------------------------------------------------------------

        /// <summary>
        /// Retrieves ALL employees (staff) for the school, automatically paging
        /// through every result page using cursor pagination.
        /// Includes contact_details (phone, email, address) by default.
        /// Optional <paramref name="updatedAfter"/> filters to records changed
        /// since that date/time.
        /// </summary>
        public List<Employee> GetEmployees(string schoolId, DateTime? updatedAfter = null)
        {
            if (string.IsNullOrWhiteSpace(schoolId))
                throw new ArgumentNullException("schoolId");

            var all     = new List<Employee>();
            int pageNum = 1;

            string nextUrl = BuildEmployeesUrl(schoolId, updatedAfter);

            Console.WriteLine("Fetching employees from Wonde...");

            while (nextUrl != null)
            {
                Console.WriteLine("  -> GET " + nextUrl);

                string json = Get(nextUrl);

                var response = JsonConvert.DeserializeObject<WondeListResponse<Employee>>(json);

                if (response == null || response.Data == null)
                    throw new WondeApiException("Unexpected empty response from Wonde employees endpoint.");

                all.AddRange(response.Data);

                bool hasMore = response.Meta != null
                    && response.Meta.Pagination != null
                    && response.Meta.Pagination.More;

                Console.WriteLine(string.Format("  <- Page {0}: {1} employees received (more pages: {2})",
                    pageNum, response.Data.Count, hasMore));

                nextUrl = hasMore ? response.Meta.Pagination.Next : null;
                //TODO: only take 1st page for testing, then remove this line to enable full paging      
                nextUrl = null;
                pageNum++;
            }

            return all;
        }

        // -------------------------------------------------------------------------
        // Students
        // -------------------------------------------------------------------------

        /// <summary>
        /// Retrieves ALL students for the school, automatically paging through
        /// every result page.  Optional <paramref name="include"/> is a
        /// comma-separated list of related objects, e.g. "contacts,contact_details".
        /// Optional <paramref name="updatedAfter"/> filters to records changed
        /// since that date/time.
        /// </summary>
        public List<Student> GetStudents(string schoolId, DateTime? updatedAfter = null)
        {
            if (string.IsNullOrWhiteSpace(schoolId))
                throw new ArgumentNullException("schoolId");

            var all     = new List<Student>();
            int pageNum = 1;

            // cursor=true: Wonde returns a full Next URL in meta.pagination.next;
            // subsequent requests follow that URL directly — no page counter needed.
            string nextUrl = BuildStudentsUrl(schoolId, updatedAfter);

            Console.WriteLine("Fetching students from Wonde...");

            while (nextUrl != null)
            {
                Console.WriteLine("  -> GET " + nextUrl);

                string json = Get(nextUrl);

                var response = JsonConvert.DeserializeObject<WondeListResponse<Student>>(json);

                if (response == null || response.Data == null)
                    throw new WondeApiException("Unexpected empty response from Wonde students endpoint.");

                all.AddRange(response.Data);

                bool hasMore = response.Meta != null
                    && response.Meta.Pagination != null
                    && response.Meta.Pagination.More;

                Console.WriteLine(string.Format("  <- Page {0}: {1} students received (more pages: {2})",
                    pageNum, response.Data.Count, hasMore));

                nextUrl = hasMore ? response.Meta.Pagination.Next : null;
                //TODO: only take 1st page for testing, then remove this line to enable full paging      
                //nextUrl = null;
                pageNum++;
            }

            return all;
        }

        // -------------------------------------------------------------------------
        // Deletions — records removed from the MIS since a given date
        // -------------------------------------------------------------------------

        /// <summary>Returns all students deleted from the MIS, optionally filtered by <paramref name="updatedAfter"/>.</summary>
        public List<DeletedRecord> GetDeletedStudents(string schoolId, DateTime? updatedAfter = null)
        {
            return GetDeletedRecords(schoolId, "student", updatedAfter);
        }

        /// <summary>Returns all employees deleted from the MIS, optionally filtered by <paramref name="updatedAfter"/>.</summary>
        public List<DeletedRecord> GetDeletedEmployees(string schoolId, DateTime? updatedAfter = null)
        {
            return GetDeletedRecords(schoolId, "employee", updatedAfter);
        }

        /// <summary>Returns all contacts deleted from the MIS, optionally filtered by <paramref name="updatedAfter"/>.</summary>
        public List<DeletedRecord> GetDeletedContacts(string schoolId, DateTime? updatedAfter = null)
        {
            return GetDeletedRecords(schoolId, "contact", updatedAfter);
        }

        /// <summary>Returns all classes deleted from the MIS, optionally filtered by <paramref name="updatedAfter"/>.</summary>
        public List<DeletedRecord> GetDeletedClasses(string schoolId, DateTime? updatedAfter = null)
        {
            return GetDeletedRecords(schoolId, "class", updatedAfter);
        }

        /// <summary>
        /// Shared cursor-paginated implementation for all deletion endpoints.
        /// Calls GET schools/{schoolId}/{resource}/deleted?cursor=true[&amp;updated_after=...].
        /// </summary>
        private List<DeletedRecord> GetDeletedRecords(string schoolId, string resource, DateTime? updatedAfter)
        {
            if (string.IsNullOrWhiteSpace(schoolId))
                throw new ArgumentNullException("schoolId");

            var all     = new List<DeletedRecord>();
            int pageNum = 1;

            string nextUrl = BuildDeletedUrl(schoolId, resource, updatedAfter);

            Console.WriteLine(string.Format("Fetching deleted {0} from Wonde...", resource));

            while (nextUrl != null)
            {
                Console.WriteLine("  -> GET " + nextUrl);

                string json = Get(nextUrl);

                var response = JsonConvert.DeserializeObject<WondeListResponse<DeletedRecord>>(json);

                if (response == null || response.Data == null)
                    throw new WondeApiException(string.Format(
                        "Unexpected empty response from Wonde deleted {0} endpoint.", resource));

                all.AddRange(response.Data);

                bool hasMore = response.Meta != null
                    && response.Meta.Pagination != null
                    && response.Meta.Pagination.More;

                Console.WriteLine(string.Format("  <- Page {0}: {1} deleted {2} received (more pages: {3})",
                    pageNum, response.Data.Count, resource, hasMore));

                nextUrl = hasMore ? response.Meta.Pagination.Next : null;
                pageNum++;
            }

            return all;
        }

        // -------------------------------------------------------------------------
        // Core HTTP — HttpWebRequest (100% synchronous, no Tasks)
        // -------------------------------------------------------------------------

        /// <summary>
        /// Issues a synchronous GET request and returns the response body as a string.
        /// Throws <see cref="WondeApiException"/> on non-2xx responses.
        /// </summary>
        private string Get(string relativeOrAbsoluteUrl)
        {
            string url = relativeOrAbsoluteUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? relativeOrAbsoluteUrl
                : _baseUrl + relativeOrAbsoluteUrl;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method  = "GET";
            request.Timeout = TimeoutMs;
            request.Accept  = "application/json";
            request.Headers.Add("Authorization", "Bearer " + _token);

            //TO DO: Log request details in DB

            // Enables gzip/deflate decompression automatically
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return ReadResponseBody(response);
                }
            }
            catch (WebException ex)
            {
                // Non-2xx responses land here via WebException
                if (ex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
                    {
                        int statusCode = (int)errorResponse.StatusCode;
                        string body    = ReadResponseBody(errorResponse);

                        throw new WondeApiException(
                            string.Format("Wonde API returned HTTP {0} for {1}. Body: {2}",
                                statusCode, url, body),
                            statusCode,
                            ex);
                    }
                }

                throw new WondeApiException(
                    string.Format("Network error calling {0}: {1}", url, ex.Message), ex);
            }
        }

        private static string ReadResponseBody(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                //TO DO Log response details in DB
                return reader.ReadToEnd();
            }
        }

        // -------------------------------------------------------------------------
        // URL helpers
        // -------------------------------------------------------------------------

        private string BuildEmployeesUrl(string schoolId, DateTime? updatedAfter)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("schools/{0}/employees?cursor=true&include=contact_details,employment_details", schoolId);

            if (updatedAfter.HasValue)
                sb.AppendFormat("&updated_after={0}",
                    Uri.EscapeDataString(updatedAfter.Value.ToString("yyyy-MM-dd HH:mm:ss")));

            return sb.ToString();
        }

        private string BuildStudentsUrl(string schoolId, DateTime? updatedAfter)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("schools/{0}/students?cursor=true&include=classes,contacts,contacts.contact_details,contact_details", schoolId);

            if (updatedAfter.HasValue)
                sb.AppendFormat("&updated_after={0}",
                    Uri.EscapeDataString(updatedAfter.Value.ToString("yyyy-MM-dd HH:mm:ss")));

            return sb.ToString();
        }

        private string BuildDeletedUrl(string schoolId, string resource, DateTime? updatedAfter)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("schools/{0}/deletions?cursor=true&type={1}", schoolId, resource);

            if (updatedAfter.HasValue)
                sb.AppendFormat("&updated_after={0}",
                    Uri.EscapeDataString(updatedAfter.Value.ToString("yyyy-MM-dd HH:mm:ss")));

            return sb.ToString();
        }
    }
}
