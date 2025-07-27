# TaskFlowManager.Api

This is the backend for the Task Management Assignment, built with .NET Web API and ADO.NET (no Entity Framework).

## Setup Instructions

### Prerequisites
- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- SQL Server (Express, LocalDB, or full)

### 1. Configure Database
- Edit `Config/db.config` with your SQL Server connection string.
- Ensure your database is created and seeded using the provided SQL scripts.

### 2. Add Queries
- Place your `queries.xml` in the `Config/` directory.

### 3. Restore & Build
```sh
cd backend
 dotnet restore
 dotnet build
```

### 4. Run the API
```sh
 dotnet run
```
The API will start on `http://localhost:7000` by default.

### 5. Test
- Use any HTTP client (Postman, browser, curl) to hit `http://localhost:7000/api/users`

---

## Notes
- All dependencies are managed in `TaskManagementApi.csproj`.
- Update CORS settings if accessing from another device.
