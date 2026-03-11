# March 10, 2026 – First Vertical API Slice: Users

Added the first end-to-end vertical slice for Users.

Features:
- IUserRepository interface in Domain
- UserRepository implementation in Infrastructure
- Repository registered via Infrastructure DI extension
- CreateUser and GetUserById minimal API endpoints
- Request/response DTOs for User API
- Endpoint grouping using MapGroup("/users")
- Centralized endpoint registration via EndpointMappings

Endpoints:
POST /users
GET  /users/{id}

This verifies the full application flow:
HTTP request → endpoint → repository → EF Core → PostgreSQL.

Tested via Swagger and confirmed persistence in PostgreSQL.