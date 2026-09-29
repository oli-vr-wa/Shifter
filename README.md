# Shifter: Enterprise Timesheet & Scheduling System

Shifter is a modern, full-stack timesheet and scheduling SaaS platform designed for multi-tenant environments. It enforces strict **Clean Architecture** to ensure long-term maintainability and business logic isolation.

## Architecture Overview
This project follows a **Monorepo** structure, ensuring that the backend, web portal, and mobile app remain tightly synced:

- **/api**: .NET 10.0 Backend using Clean Architecture.
- **/web**: React-based Admin Portal (Vite + TypeScript).
- **/app**: React Native mobile app (Expo) for employee clock-ins.

### Backend Layers
1. **Core**: Business entities and domain interfaces (The "Center").
2. **Application**: Use cases, DTOs, and services (The "Brain").
3. **Infrastructure**: Entity Framework Core, PostgreSQL, and Identity (The "World").
4. **API**: Minimal API endpoints and JWT authentication (The "Entry Point").

## Getting Started

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Required for running the environment)
- .NET 10.0 SDK (Required for local debugging and EF Core migrations)
- Visual Studio 2026 or VS Code

### Environment Setup

This project uses a containerized architecture via `docker-compose`. Secrets are injected dynamically and must never be committed to source control.

#### 1. Setup Docker Environment Variables
At the root of the project, duplicate the `.env.example` file and rename it to `.env`.

```bash
cp .env.example .env
```

Open the new `.env` file and populate it with your local development secrets:

```text
JWT_SECRET=YourSecretKeyHere
JWT_ISSUER=YourIssuer
JWT_AUDIENCE=YourAudience
DB_PASSWORD=YourDatabasePassword
```

#### 2. Setup Local User Secrets (For EF Core Migrations)
While Docker uses the `.env` file, your local .NET CLI needs access to the database credentials to run Entity Framework Core migrations. We use .NET User Secrets to keep these safe.
Navigate to the API directory and initialize the secrets:

```bash
cd api/Shifter.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=shifterdb;Username=postgres;Password=YourDatabasePassword;Include Error Detail=true;"
```

## Running the Application

The entire stack (Database, API, and Frontend) is orchestrated via Docker.

1. Open a terminal at the root of the project.
2. Build and start the containers in the background:

```bash
docker compose up -d --build
```

**Services will be available at:**
- **Web Portal (React/Vite):** `http://localhost:5173` *(Features Hot Module Replacement)*
- **API (Endpoints):** `http://localhost:5170`
- **Database (PostgreSQL)** `localhost:5432`

*Note: The React frontend uses a Vite proxy to automatically route `/api` requests to the internal Docker network, bypassing CORS issues entirely.*


## Database Strategy & Migrations

We utilize PostgreSQL with EF Core. The schema is designed for SaaS multi-tenancy, with `CompanyId` acting as the primary discriminator across all major domain entities.

This project is configured to **automatically apply database migrations** when the API container boots up. You do not need to run `database update` manually.

To create a new migration after modifying your C# domain models run the following command:

```bash
cd api/Shifter.Infrastructure
# Add a new migration
dotnet ef migrations add <MigrationName>
# Restart backedn container to automatically apply the migration)
docker compose up -d --build backend
```

## Security
- **Identity**: Uses ASP.NET Core Identity for secure password hashing and account management.
- **Multi-Tenancy**: Implements global query filters in Entity Framework Core, scoped by `ITenantService`, to ensure data isolation between companies.
- **JWT Authentication**: Stateful multi-stage auth (Login -> MFA Challenge -> Access Token).

