# School Management System API

This is a RESTful API for a School Management System built with **ASP.NET Core (.NET 8)**, using **Entity Framework Core**, **JWT Authentication**, and **Clean Architecture** principles.

---

## Technologies Used

- **.NET 8 (ASP.NET Core Web API)**
- **Entity Framework Core** (Code-First)
- **SQL Server**
- **JWT Authentication**
- **Swagger (Swashbuckle)**
- **Clean Architecture** (Domain, Application, Infrastructure, WebAPI)

---

## Required Packages

Install the following NuGet packages in each layer:

### Infrastructure Project (`SchoolManagement.Infrastructure`)

```bash
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.Extensions.Configuration
Install-Package Microsoft.Extensions.Configuration.Json
Install-Package BCrypt.Net-Next
```

### WebAPI Project (`SchoolManagement.WebAPI`)

```bash
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
Install-Package Swashbuckle.AspNetCore
```

> Note: You can install packages via **Package Manager Console** or **NuGet GUI**.

---

## How to Run the Project

### 1. Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/SchoolManagementTask.git
cd SchoolManagementTask
```

### 2. Set Up the Database

Ensure SQL Server is installed and running. Then configure your connection string in:

```json
SchoolManagement.WebAPI/appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=SchoolManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Apply Migrations

```bash
dotnet ef database update   --project SchoolManagement.Infrastructure   --startup-project SchoolManagement.WebAPI
```

> This will automatically create the database and schema.

### 4. Seed Data (Admin, Teacher, Student)

`DbSeeder.Seed()` will run automatically on startup in `Program.cs`.

---

## Swagger API Documentation

Swagger is enabled to help you explore and test the API endpoints.

### To Access Swagger:

Run the project, then visit:

```
https://localhost:{PORT}/swagger
```

### Add Authorization in Swagger

1. Click **Authorize** button at the top.
2. Enter your JWT token like this:

```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

3. Click **Authorize** → **Close**.
4. Now you can access protected routes.

---

## API Features Implemented

| Feature         | Role           | Description                   |
| --------------- | -------------- | ----------------------------- |
| Register/Login  | All            | JWT-based authentication      |
| Get Profile     | All            | Authorized user profile       |
| Manage Courses  | Admin, Teacher | CRUD on courses               |
| Enroll Students | Admin          | Assign students to courses    |
| Add Assignments | Teacher        | Add assignments to courses    |
| Submit Grades   | Teacher        | Grade students per assignment |
| View Grades     | Student        | View their grades             |

---

## Best Practices Used

- DTOs (never expose entities directly)
- API responses wrapped in `ApiResponse`
- Separation of concerns via Clean Architecture
- Try-catch with informative error handling
- Swagger + Bearer Token Integration
- Dependency Injection used for all services
- Role-based Authorization (`[Authorize(Roles = ...)]`)
- Paging and Filtering (for large lists)

---

## Next Improvements (Suggestions)

- File Upload (for assignments)
- Notification system (e.g. grades posted)
- Unit testing with xUnit or NUnit

---

## Default Seeded Users

| Role    | Email              | Password    |
| ------- | ------------------ | ----------- |
| Admin   | admin@school.com   | password123 |
| Teacher | teacher@school.com | password123 |
| Student | student@school.com | password123 |

> These are seeded on first run by `DbSeeder.cs`.

---

## License

This project is licensed for educational and testing use. All rights reserved.
