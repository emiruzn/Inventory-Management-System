# Inventory Management System API

A robust **RESTful API** for managing an inventory system with user role-based access control. This project enables users to manage products within the inventory based on their assigned roles. Designed for scalability and high performance, the application incorporates modern technologies and best practices.

## Key Features

- **User Management**: Supports role-based access control (RBAC) to ensure secure and restricted access to resources.
- **Inventory Management**: Create, update, view, and delete products based on user permissions.
- **Authentication and Authorization**: Implements strong API validation with JWT-based authentication.
- **Database Integration**:
  - **MongoDB** for NoSQL data storage (Products).
  - **PostgreSQL** with a relational database for structured data (Users and Categories).
- **Caching with Redis**: Enhances API performance with efficient caching mechanisms (Cached Products).
- **Scalable Architecture**: Built with C# and .NET Core for modern and maintainable backend solutions.

## Tech Stack

- **Backend**: C# with .NET Core
- **Databases**: MongoDB, PostgreSQL
- **Caching**: Redis
- **Authentication**: JWT-based secure API token management

## Getting Started

1. **Clone the Repository**:

   ```bash
   git clone <clone-url>
   cd inventory-management-system
   ```

2. **Configure the Environment**:
   
   Update the `appsettings.json` file with your connection strings for:
   - MongoDB
   - Redis
   - PostgreSQL

3. **Run Database Migrations**:

   Use Entity Framework to apply migrations:
   
   ```bash
   cd Data
   dotnet ef migrations add InitialMigration --context AppDbContext --project <Project File Path>
   dotnet ef database update --context AppDbContext --project <Project File Path>
   ```

4. **Start the Application**:

   Run the application locally:
   
   ```bash
   dotnet run
   ```

5. **Explore the API**:

   Open Swagger to test the API endpoints:
   
   ```bash
   http://localhost:5079/swagger/index.html
   ```  
