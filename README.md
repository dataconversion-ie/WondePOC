# W2P_WondeAPI

Proof of concept for integrating **Wonde** (MIS data API) with **Way2Pay**, so school student/contact data can be synced from the school's MIS instead of CSV imports.

## Contents

| Path | What it is |
|---|---|
| `WondePoC/` | Visual Studio solution – .NET Framework 4.7.2 console app that calls the Wonde API |
| `Docs/` | Design proposal (Jul 26) and design & implementation plan (incl. Rev A) |
| `WONDE various.txt` | Working notes |

## WondePoC

Console app that exercises the Wonde REST API (`https://api.wonde.com/v1.0/`):

1. **Get approved schools** – `GET schools` (schools that have granted access to our token)
2. **Get students** – `GET schools/{id}/students` with `include=classes,contacts,contacts.contact_details,contact_details`, optionally filtered by `updated_after`
3. **Employees** and **deletions** (`/{resource}/deleted` for students, employees, contacts, classes) – implemented in the client, currently commented out in `Program.cs`

Key files:

- `Services/WondeApiClient.cs` – HTTP client (Bearer token auth, cursor paging via `meta.pagination.next`)
- `Models/` – DTOs for Wonde responses (School, Student, Contact, Class, Employee, DeletedRecord, paging wrappers)
- `Exceptions/WondeApiException.cs` – API error with HTTP status

## Setup

1. Open `WondePoC/WondePoC.sln` in Visual Studio (Newtonsoft.Json restores via NuGet).
2. Create `WondePoC/WondePoC/secrets.config` (git-ignored) with your token:

   ```xml
   <appSettings>
     <add key="WondeApiToken" value="YOUR_TOKEN" />
   </appSettings>
   ```

3. Other settings are in `App.config` (`WondeBaseUrl`, `WondeSchoolId` – test school URN 10003 = Wonde ID `A1930499544`).
4. Run. The `updatedAfter` date for delta syncs is set at the top of `Program.Main`.

## Notes

- Target is net472 / C# 7.3 to match the Way2Pay (DNN 9) stack.
- Never commit `secrets.config`.
