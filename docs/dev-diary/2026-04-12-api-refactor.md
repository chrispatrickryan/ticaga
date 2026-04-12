# 2026-04-12 - Auth / JWT Refactor Completion

Today I completed a substantial refactor of Ticaga’s authentication flow and JWT architecture.

## What I changed

### Moved application registration into the Application project
I moved the `AddApplication()` dependency injection registration into `Ticaga.Application` so the Application layer now owns registration of its own handlers.

This cleaned up project boundaries and removed the awkward situation where the API project was registering Application services.

### Cleaned up password hashing ownership
I introduced a proper `IPasswordHasher` abstraction in the Application layer and kept the concrete `AspNetIdentityPasswordHasher` implementation in Infrastructure.

That means:
- Application depends only on a simple abstraction
- Infrastructure owns the ASP.NET Identity implementation details
- Handlers no longer care how password hashing is actually performed

### Clarified JWT ownership
I kept JWT implementation details in the API layer, where they naturally belong.

That includes:
- `JwtOptions`
- `IJwtTokenService`
- `JwtTokenService`
- `JwtTokenResult`

To bridge this cleanly into Application, I added a `JwtAccessTokenGenerator` adapter in the API project that implements the Application-level `IAccessTokenGenerator` abstraction.

This gave me a clean separation:
- Application knows it needs an access token generator
- API knows how to generate JWTs
- Application does not need to know anything about JWT internals

### Finished the GetCurrentUser application flow
I completed the `GetCurrentUserHandler` in the Application layer so `/auth/me` now follows the same architectural pattern as the rest of the system.

The API endpoint is now responsible only for:
- reading the authenticated user id from claims
- passing that id into the Application handler
- translating the result into an HTTP response

The actual user lookup logic now lives in Application instead of directly in the endpoint.

### Moved endpoint DTOs to a more appropriate location
I moved `CurrentUserResponse` out of the JWT folder and into the Auth endpoint DTO folder.

That better reflects what it actually is:
- an endpoint response DTO
- not a JWT-specific type

### Removed duplicated JWT abstractions from Infrastructure
Earlier in the refactor, JWT-related abstractions had started to appear in multiple places. I cleaned that up so Infrastructure no longer contains overlapping JWT concepts.

That made project ownership much clearer:
- API owns JWT
- Infrastructure owns repositories and hashing
- Application owns use-case abstractions

## Resulting architecture

After this refactor, auth is now structured like this:

### Registration
API endpoint  
→ `RegisterUserHandler`  
→ `IUserRepository` + `IPasswordHasher`

### Login
API endpoint  
→ `LoginHandler`  
→ `IUserRepository` + `IPasswordHasher` + `IAccessTokenGenerator`

### Token generation
`IAccessTokenGenerator`  
→ API `JwtAccessTokenGenerator`  
→ API `JwtTokenService`

### Current authenticated user
JWT middleware populates `ClaimsPrincipal`  
→ API endpoint extracts user id claim  
→ `GetCurrentUserHandler` loads the user from the repository

## Why this refactor mattered

This refactor significantly improved the separation of concerns in the solution.

Before this work, auth logic and JWT-related responsibilities were spread across layers in a way that made ownership less clear.

Now:
- Application contains use-case logic
- Infrastructure contains data access and hashing implementation
- API contains HTTP concerns and JWT concerns

This makes the codebase easier to understand, easier to maintain, and more aligned with clean architecture principles without becoming overengineered.

## Outcome

The auth refactor is now close to a "done" state and the codebase feels much cleaner than before.

This was an important step for Ticaga because it:
- improved architectural consistency
- reduced coupling between layers
- gave me a cleaner foundation for future authenticated features
- strengthened the overall production-readiness of the backend