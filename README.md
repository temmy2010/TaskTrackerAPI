# TaskTrackerAPI

A simple ASP.NET Core Web API for managing tasks, user authentication, and reporting, built following the **Onion architecture** with:
- .NET 8
- SQLite (with Entity Framework Core)
- JWT-based authentication & authorization
- Repository pattern
- Swagger/OpenAPI docs
- xUnit + Moq unit tests

---

## Project Structure
TaskTrackerAPI/
├── API/ → Presentation layer (Controllers, Program.cs)
├── Application/ → Services, DTOs
├── Domain/ → Entities & Interfaces
├── Infrastructure/ → Data & Repositories
├── Tests/ → Unit tests (AuthServiceTests, TasksControllerTests)
├── TaskTrackerAPI.sln → Solution file
└── README.md

---

## Features

- User Registration & Login with JWT
- Role-based authorization (`User`, `Manager`)
- Task management: create, read, update, delete, paginate & filter by status
- Mark task as complete
- Report endpoint for completion rate (`GET /api/reports/completion`) restricted to Managers
- Swagger UI for exploring & testing endpoints
- Unit tests covering services and controllers

---

## Getting Started

**Clone the repository:**
```bash
git clone https://github.com/temmy2010/TaskTrackerAPI.git
cd TaskTrackerAPI

**Add the database:**
Make sure you have .NET SDK installed.
dotnet ef database update --project TaskTrackerAPI.Infrastructure --startup-project TaskTrackerAPI.API

**Run the project:*
dotnet run --project TaskTrackerAPI.API

**Open Swagger UI:**
https://localhost:7223/swagger

**## Authentication & Roles**

- Register: POST /api/auth/register
- Login: POST /api/auth/login → returns JWT token
- Click Authorize in Swagger and enter: Bearer YOUR_JWT_TOKEN
- Use role Manager to access reporting endpoints.

## Running Unit Tests

Unit tests are in the Tests folder.
To run: dotnet test

---

**## API Endpoints**

| Method | Endpoint                 | Description                                |
| ------ | ------------------------ | ------------------------------------------ |
| POST   | /api/auth/register       | Register a new Role                        |
| POST   | /api/auth/login          | Login & get JWT token                      |
| GET    | /api/tasks               | Get all tasks (auth required)              |
| GET    | /api/tasks/paged         | Get paginated tasks (with optional status) |
| POST   | /api/tasks               | Create new task                            |
| PUT    | /api/tasks/{id}          | Update existing task                       |
| DELETE | /api/tasks/{id}          | Delete task                                |
| POST   | /api/tasks/{id}/complete | Mark task as completed                     |
| GET    | /api/reports/completion  | Get completion report (Manager only)       |

---

## Configuration

In appsettings.json:
"Jwt": {
  "Key": "A7zA!3w5g7Q@ZpLx#VkT6%RmBcY1!Xhg",
  "Issuer": "TaskTrackerAPI",
  "Audience": "TaskTrackerAPIUsers"
},
"ConnectionStrings": {
  "DefaultConnection": "Data Source=TaskTracker.db"
}

---

## Tools & Libraries
- ASP.NET Core 8
- EF Core + SQLite
- JWT (Microsoft.AspNetCore.Authentication.JwtBearer)
- Swagger (Swashbuckle)
- xUnit, Moq

---

## Usage Example:
1. Register a user:
POST /api/auth/register
```
{
  "username": "manager",
  "password": "Password123",
  "role": "Manager"
}
```
2. Login:
POST /api/auth/login
```
{
  "username": "manager",
  "password": "Password123"
}
```

4. Copy the returned token, click Authorize in Swagger, and test other endpoints.

## Database
SQLite file TaskTracker.db will be created in the root or /bin folder after running migrations.

## Postman
To use Postman:
- Import swagger/v1/swagger.json as a collection
- Or call APIs manually with your JWT token in Authorization header.

---

## Contributing
Feel free to fork, open PRs, or report issues. If you'd like, you can:
- Add CQRS
- Add integration tests
- Deploy to cloud (e.g., Azure)

---

## License
MIT

Built with using .NET 8 & Onion Architecture
