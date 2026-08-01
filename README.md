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
- .NET 10.0 SDK
- PostgreSQL
- Visual Studio 2026

### Environment Setup
1. **Initialize the Monorepo:**

```bash
   git init
   dotnet new gitignore
```

2. **Setup Secrets:**
The API requires a JWT secret for local authentication. Run the following command in the `api/Shifter.API` directory:

```bash
	dotnet user-secrets init
   	dotnet user-secrets set "Jwt:Secret" "YOUR_SUPER_LONG_AND_SECURE_SECRET_KEY_HERE"
   	dotnet user-secrets set "Jwt:Issuer" "ISSUER_IN_DOCUMENTATION"
   	dotnet user-secrets set "Jwt:Audience" "AUDIENCE_IN_DOCUMENTATION"
```

### Runing the API
1. Navigate to `/api`.
2. Open `Shifter.sln` in Visual Studio.
3. Set `Shifter.API` as the Startup Project.
4. Press **F5**.

### Testing the API
Use the built-in Visual Studio `.http` files located in `Shifter.API/shifter_tests.http`. You can send requests directly from the editor to test authentication and MFA flows.

## Security
- **Identity**: Uses ASP.NET Core Identity for secure password hashing and account management.
- **Multi-Tenancy**: Implements global query filters in Entity Framework Core, scoped by `ITenantService`, to ensure data isolation between companies.
- **JWT Authentication**: Stateful multi-stage auth (Login -> MFA Challenge -> Access Token).

## Database Strategy
We utilize PostgreSQL with EF Core. The schema is designed for SaaS multi-tenancy, with `CompanyId` acting as the primary discriminator across all major domain entities.

