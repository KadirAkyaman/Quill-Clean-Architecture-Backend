# Quill - A Modern Backend with ASP.NET Core & Clean Architecture
<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet" alt=".NET 9.0">
  <img src="https://img.shields.io/badge/Architecture-Clean-blue" alt="Clean Architecture">
  <img src="https://img.shields.io/badge/PostgreSQL-336791?logo=postgresql&logoColor=white" alt="PostgreSQL">
  <img src="https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white" alt="Docker">
  <img src="https://img.shields.io/badge/Redis-DC382D?logo=redis&logoColor=white" alt="Redis">
  <img src="https://img.shields.io/badge/Testing-xUnit-blue?logo=xunit" alt="xUnit">
  <img src="https://img.shields.io/badge/Logging-Serilog-blue" alt="Serilog">
</p>

**Quill** is a robust and scalable backend API for a modern blogging platform, designed and built with a focus on clean code, performance, and professional software engineering principles. This project serves as a comprehensive showcase of building production-ready applications using ASP.NET Core, demonstrating a deep understanding of Clean Architecture, Domain-Driven Design, and modern DevOps practices.

---

### Core Concept

The core idea behind Quill is to provide the complete server-side functionality for an interactive blogging application. It empowers users to create and publish content, follow other authors to build a personalized feed, and manage their profiles and interactions. This project is developed as a **backend-only** solution, providing a powerful API for any potential frontend client (web, mobile, etc.) to consume.

---

### Getting Started

This project is fully containerized with Docker. The only prerequisite is to have **Docker Desktop** installed and running on your machine.

#### 1. Clone the Repository

```bash
git clone https://github.com/KadirAkyaman/Quill-Clean-Architecture-Backend.git
cd Quill-Clean-Architecture-Backend/backend
```

#### 2. Configure Your Environment

Before launching, you need to set up your database password.

*   Open the **docker-compose.yml** file in the root of the /backend directory.
*   Locate the two instances of **POSTGRES_PASSWORD** and replace **YOUR_STRONG_PASSWORD_HERE** with your own secure password.

```yaml
environment:
  - POSTGRES_PASSWORD=your-chosen-password
```

*   Do the same for the **ConnectionStrings__DefaultConnection** environment variable for the quill-api service in the same file.

#### 3. Launch the Application

Run the following command from the /backend directory:

```bash
docker-compose up --build
```

This single command will:

*   Build the Docker image for the Quill API.
*   Pull the official PostgreSQL image.
*   Start both containers.
*   Create a persistent volume for the database to ensure your data is saved.
*   Run Entity Framework Core migrations automatically to set up the database schema and seed initial data.

Once the containers are running, the API will be available at http://localhost:8080.

#### 4. Explore the API

*   **API Documentation (Swagger):** Navigate to http://localhost:8080/swagger to view and interact with the API endpoints.
*   **Health Checks UI:** Navigate to http://localhost:8080/health-ui to see the real-time health status of the API and its database connection.

---

### Key Features & Implementation Details

#### Architecture & Design Patterns

The foundation of Quill is built upon the **Clean Architecture** philosophy, ensuring a clear separation of concerns, testability, and long-term maintainability.

*   **Clean Architecture:** A strict separation of concerns between Domain, Application, Infrastructure, and Presentation layers, ensuring the application is testable, maintainable, and independent of external frameworks.
*   **Hybrid Data Access Model:** Leverages the raw performance of Dapper for high-speed read operations and the safety and powerful Change Tracker of Entity Framework Core for write operations.
*   **Repository & Unit of Work Patterns:** Abstracts all data access logic and centralizes transaction management to ensure data integrity across complex operations.
*   **Domain-Driven Design (DDD) Principles:** Core business logic and entities are designed to be self-contained and expressive of the problem domain.

#### Core API Features
*   **Authentication & Authorization:** Secure user registration and login using JWT (JSON Web Tokens) with role-based access control (Admin, Author).
*   **Content Management:** Full CRUD operations for Posts, Categories, and Tags, including the robust handling of the many-to-many relationship between posts and tags during create and update operations.
*   **Subscription System:** A complete follow/unfollow system allowing users to subscribe to other authors, built with a "soft delete" approach for re-subscription.
*   **File Uploads:** A dedicated and secure endpoint for uploading profile pictures (IFormFile), with server-side validation for file size and content type (MIME type). Uploaded files are persistently stored using Docker volumes.
#### API Management & Security
*   **API Versioning:** Implemented URL-segment based versioning (e.g., /api/v1/...) to allow for future non-breaking changes and long-term API maintainability, with full Swagger integration.
*   **Rate Limiting:** A layered, policy-based rate limiting system protects the API from abuse and brute-force attacks, with different limits for general (fixed), authentication (auth), and admin (admin) endpoints.
*   **Centralized Error Handling:** A global exception handling middleware catches all unhandled exceptions, logs them, and returns a standardized, user-friendly ProblemDetails error response.
*   **Input Validation:** Uses FluentValidation to define and apply clear, strongly-typed validation rules for all incoming DTOs.
#### Development, Testing & Operations (DevOps)
*   **Containerization:** Full Docker & Docker Compose support provides a single-command (docker-compose up) setup for the entire application stack (API + PostgreSQL), ensuring a consistent environment.
*   **Automated Database Migrations:** The application automatically applies pending Entity Framework Core migrations on startup, simplifying database schema management.
*   **Health Checks:** A dedicated /health endpoint provides real-time status of the application and its database, fully integrated with Docker's internal healthcheck mechanism and a visual UI dashboard.
*   **Comprehensive Unit Testing:** The critical business logic in the Application layer is thoroughly tested using xUnit, Moq, and FluentAssertions, covering happy paths, failure paths, and edge cases to ensure code quality and reliability.
*   **Logging & Documentation:** Structured logging is implemented with Serilog, and a rich API documentation with versioning support is automatically generated via Swashbuckle (Swagger).

---

### Technology Stack

| Category | Technology / Library | Purpose |
| :--- | :--- | :--- |
| **Core Framework** | [![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) | Main application framework for building the API. |
| **Architecture** | **Clean Architecture** | Ensures separation of concerns, testability, and maintainability. |
| **Database** | [![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?logo=postgresql&logoColor=white)](https://www.postgresql.org/) | Primary relational database for data storage. |
| **Data Access** | **Hybrid Model (EF Core & Dapper)** | **EF Core** for safe write operations; **Dapper** for high-performance read queries. |
| **Authentication** | ![JWT](https://img.shields.io/badge/Authentication-JWT-black) | Secure, stateless authentication for API endpoints. |
| **API Versioning**| [![Asp.Versioning](https://img.shields.io/badge/API%20Versioning-Asp.Versioning-blue)](https://www.nuget.org/packages/Asp.Versioning.Mvc.ApiExplorer/) | Manages API lifecycle and enables non-breaking changes. |
| **Validation** | [![FluentValidation](https://img.shields.io/badge/Validation-FluentValidation-blue)](https://fluentvalidation.net/) | Defines and applies strongly-typed validation rules for DTOs. |
| **Mapping** | [![AutoMapper](https://img.shields.io/badge/Mapping-AutoMapper-blue)](https://automapper.org/) | Automates the mapping between DTOs and Domain entities. |
| **Logging** | [![Serilog](https://img.shields.io/badge/Logging-Serilog-blue)](https://serilog.net/) | Provides structured and configurable logging capabilities. |
| **API Docs** | [![Swashbuckle](https://img.shields.io/badge/API%20Docs-Swashbuckle-blue)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) | Generates interactive API documentation (Swagger UI). |
| **Testing** | **xUnit, Moq, FluentAssertions** | A comprehensive suite for unit testing business logic. |
| **Containerization**| [![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)](https://www.docker.com/) | Packages the application and its dependencies into portable containers. |
| **Orchestration** | **Docker Compose** | Manages the multi-container application stack (API + Database). |

---

### Project Status & Roadmap

The current implementation provides a solid foundation. Future development will focus on the following advanced features to make the API fully production-ready and scalable:

Future development will focus on advanced features such as:
*   **Pagination** for all list-based endpoints to handle large data sets efficiently.
*   **Integration Testing** to validate the entire request pipeline, from API endpoint to the database.
*   **CI/CD Pipeline** setup using GitHub Actions for automated builds and testing.
*   **Caching with Redis** for performance enhancement.
*   **Advanced Security Features** like secure logout (token revocation) and token sliding.

This repository stands as a testament to modern backend development practices in the .NET ecosystem.
