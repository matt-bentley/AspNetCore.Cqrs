

# AspNetCore.Cqrs

**ASP.NET Core** API setup using Command Query Responsibility Segregation (CQRS). The solution follows the 'Different Read/Write Models, Single Database' approach described here: https://medium.com/better-programming/choosing-a-cqrs-architecture-that-works-for-you-02619555b0a0

- **Write Side**: Entity Framework Core is used for Commands
- **Read Side**: Dapper is used for Queries

The setup follows important modern development principles described here: https://github.com/matt-bentley/CleanArchitecture

The application extends the typical *Weather Forecast* example provided in default .NET project templates and contains the following components:

- **API** - ASP.NET 7 REST API with Swagger support
- **Database** - SQL Server database integration via Entity Framework Core and Dapper
- **Migrations** - Code-First database migrations managed using a console application
  

## Quick Start

1. Start local database in Docker

```bash
docker-compose --profile dev up -d
```

2. Run the **AspNetCore.Cqrs.Migrations** project to deploy database schema
3. Run the **AspNetCore.Cqrs.Api** projects to debug the API and view Swagger

