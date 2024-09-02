Task Management System

This project is a backend service for a task management system built using .NET 8 and Entity Framework Core, with SQL Server for data storage. It includes functionalities for user management, task creation, updating, deletion, and querying, along with JWT-based authentication.

Features:
- User Registration and Login with JWT Authentication
- CRUD Operations for Tasks
- Task Filtering and Sorting
- Pagination for Task Listings
- Secure Password Hashing
- Logging for Key Operations

Prerequisites:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) and Docker Compose
- SQL Server (running in Docker container as part of the setup)

Setup Instructions:

1. Clone the Repository:
   git clone https://github.com/KraS27/TaskManager.git
   cd your-repository

2. Configure Environment Variables:
   Copy the `.env.example` file to `.env` and update it with your specific configuration settings (like JWT secret, connection strings, etc.).
   cp .env.example .env

3. Configure Database Connection:
   Update the `appsettings.json` file with the connection string for SQL Server. For Docker, it can be configured as follows:
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost,1433;Database=TaskManagementDB;User Id=admin;Password=YourStrongPassword;"
   }

4. Run the Application with Docker:
   Ensure Docker is running on your machine. You can start the application and the database using Docker Compose:
   docker-compose up --build

5. Apply Database Migrations:
   Once the containers are up and running, apply the EF Core migrations to set up the database schema:
   docker-compose exec webapp dotnet ef database update

6. Access the Application:
   The API will be available at http://localhost:5000.

Running the Project Locally Without Docker:

1. Ensure SQL Server is installed and running on your machine, or use a SQL Server Docker container.

2. Update the `appsettings.json` file with the correct connection string.

3. Run the following commands to set up and run the project:
   # Restore dependencies
   dotnet restore

   # Apply migrations
   dotnet ef database update

   # Run the application
   dotnet run

4. The API will be available at http://localhost:5282.

Architecture and Design Choices:

Architecture:
- Layered Architecture: The project follows a layered architecture pattern with separation of concerns:
  - Controller Layer: Handles HTTP requests and responses.
  - Service Layer: Contains business logic and interacts with the repository layer.
  - Repository Layer: Manages data access and interacts with the database.
  - Database: SQL Server is used for persistent storage.

Design Choices:
- Entity Framework Core: Used for ORM, enabling easy database operations and migrations.
- JWT Authentication: Provides secure token-based authentication for users.
- Repository Pattern: Ensures separation of data access logic from business logic, improving testability and maintainability.
- Logging: Implemented to track key operations such as task creation, updates, and deletions.
- Exception Handling: Custom exceptions are used for better clarity and error handling.
- Pagination and Filtering: Allows efficient querying and management of tasks.

License:
This project is licensed under the MIT License. See the LICENSE file for details.

Acknowledgments:
- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Docker](https://www.docker.com/)