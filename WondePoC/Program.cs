using System;
using System.Collections.Generic;
using System.Configuration;
using WondePoC.Models;
using WondePoC.Services;
using WondePoC.Exceptions;

namespace WondePoC
{
    class Program
    {
        static void Main(string[] args)
        {
            string token   = ConfigurationManager.AppSettings["WondeApiToken"];
            string baseUrl = ConfigurationManager.AppSettings["WondeBaseUrl"];

            DateTime? updatedAfter = new DateTime(2026, 5, 21); // Optional: filter records updated/deleted after this date
            Console.WriteLine("Wonde API Client");
            Console.WriteLine();

            try
            {
                var client = new WondeApiClient(token, baseUrl);

                Console.WriteLine("Step 1: get approved Schools");
                List<School> schools = client.GetApprovedSchools();

                if (schools.Count == 0)
                {
                    Console.WriteLine("No approved schools found for this API token.");
                    Pause();
                    return;
                }

                Console.WriteLine(string.Format("{0} school(s) found:", schools.Count));
                Console.WriteLine();

                foreach (var s in schools)
                {
                    Console.WriteLine(string.Format("WONDE ID: {0, -15}  Name: {1,-15}  MIS ID: {2,-12}  URN: {3,-15}  Establishment No: {4,-15} La Code: {5,-15}",
                        s.Id,
                        s.Name,
                        s.MisId,
                        s.Urn,
                        s.EstablishmentNumber,
                        s.LaCode));
                    Console.WriteLine();
                }

                Console.WriteLine("Step 2: GetStudents");

                foreach (var s in schools)
                {
                    // Use the Wonde-encoded ID of the first approved school
                    string wondeSchoolId = s.Id;
                    Console.WriteLine("School: " + s.Name);
                    Console.WriteLine();

                    List<Student> students = client.GetStudents(wondeSchoolId, updatedAfter);

                    Console.WriteLine();
                    Console.WriteLine(string.Format("Total students returned: {0}", students.Count));
                    Console.WriteLine();

                    for (int i = 0; i < students.Count; i++)
                    {
                        Student st = students[i];

                        Console.WriteLine(string.Format("[{0,4}]  {1,-20} {2,-20}  MIS ID: {3,-12}  WONDE ID: {4,-15}  CREATED: {5,-15}  UPDATED: {6,-20}",
                            i + 1,
                            st.Forename,
                            st.Surname,
                            st.MisId,
                            st.Id,
                            st.CreatedAt,
                            st.UpdatedAt));

                        if (st.Contacts != null)
                        {
                            foreach (var c in st.Contacts.Data)
                            {
                                Console.WriteLine(string.Format("Contact: {0,-20} {1,-20}  MIS ID: {2,-12}  WONDE ID: {3,-15}  CREATED: {4,-15}  UPDATED: {5,-20}  Priority: {6,-15}",
                                    c.Forename,
                                    c.Surname,
                                    c.MisId,
                                    c.Id,
                                    c.CreatedAt,
                                    c.UpdatedAt,
                                    c.Priority.HasValue ? c.Priority.Value.ToString() : "N/A"

                                    ));
                                if (c.Priority.HasValue && c.Priority.Value == 1)
                                {
                                    Console.WriteLine("Primary Contact");
                                }

                            }
                        }
                    }

                    // Step 3: Fetch all employees
                    Console.WriteLine();
                    //Console.WriteLine("Step 3: GetEmployees");

                    //List<Employee> employees = client.GetEmployees(wondeSchoolId);

                    //Console.WriteLine();
                    //Console.WriteLine(string.Format("Total employees returned: {0}", employees.Count));
                    //Console.WriteLine();

                    //for (int i = 0; i < employees.Count; i++)
                    //{
                    //    Employee emp = employees[i];

                    //    Console.WriteLine(string.Format("[{0,4}]  {1,-20} {2,-20}  MIS ID: {3,-8}  EMAIL: {4,-35}  PHONE: {5}",
                    //        i + 1,
                    //        emp.Forename,
                    //        emp.Surname,
                    //        emp.MisId,
                    //        emp.PrimaryEmail ?? "-",
                    //        emp.PrimaryPhone ?? "-"));
                    //}

                    //// Step 4: Fetch deletions
                    //Console.WriteLine();
                    //Console.WriteLine("Step 4: GetDeletions");

                    //List<DeletedRecord> deletedStudents  = client.GetDeletedStudents(wondeSchoolId, updatedAfter);
                    //List<DeletedRecord> deletedEmployees = client.GetDeletedEmployees(wondeSchoolId, updatedAfter);
                    //List<DeletedRecord> deletedContacts  = client.GetDeletedContacts(wondeSchoolId, updatedAfter);
                    //List<DeletedRecord> deletedClasses   = client.GetDeletedClasses(wondeSchoolId, updatedAfter);

                    //Console.WriteLine();
                    //Console.WriteLine(string.Format("Deleted students:  {0}", deletedStudents.Count));
                    //Console.WriteLine(string.Format("Deleted employees: {0}", deletedEmployees.Count));
                    //Console.WriteLine(string.Format("Deleted contacts:  {0}", deletedContacts.Count));
                    //Console.WriteLine(string.Format("Deleted classes:   {0}", deletedClasses.Count));
                }
            }
            catch (WondeApiException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wonde API Error: " + ex.Message);
                if (ex.HttpStatusCode.HasValue)
                    Console.WriteLine("HTTP Status: " + ex.HttpStatusCode.Value);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unexpected error: " + ex.Message);
                Console.ResetColor();
            }

            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            if (!Console.IsInputRedirected)
                Console.ReadKey();
        }
    }
}
