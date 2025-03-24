# ASP.NET MVC__API_CRUD(using Postman)

## Project Overview
This is an ASP.NET MVC project with Web API for performing CRUD (Create, Read, Update, Delete) operations.



## Features
- ASP.NET MVC with Web API
- CRUD operations using Entity Framework
- Role-based Authorization
- API Routing & Versioning
- Data Validation & Error Handling

## Technologies Used
- .NET Framework / .NET Core
- ASP.NET MVC 5
- Web API 2
- Entity Framework
- SQL Server

## Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/zannatulferdous0162/ASP.NET-MVC__API_CRUD.git
   ```
2. Open the project in Visual Studio.
3. Restore NuGet packages.
4. Configure database connection in `appsettings.json`.
5. Run the migrations:
   ```sh
   Update-Database
   ```
6. Start the application.

## API Endpoints
### 1. Get All Employees
   ```http
   GET /api/employees
   ```

### 2. Get Employee by ID
   ```http
   GET /api/employees/{id}
   ```

### 3. Create a New Employee
   ```http
   POST /api/employees
   Content-Type: application/json
   {
     "name": "John Doe",
     "email": "john@example.com",
     "salary": 5000
   }
   ```

### 4. Update Employee
   ```http
   PUT /api/employees/{id}
   Content-Type: application/json
   {
     "name": "John Doe Updated",
     "email": "john@example.com",
     "salary": 5500
   }
   ```

### 5. Delete Employee
   ```http
   DELETE /api/employees/{id}
   ```

## Contributing
Feel free to contribute by forking the repository and submitting a pull request.



