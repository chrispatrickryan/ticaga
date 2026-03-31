## 2026-03-30 – Basic Authentication (Registration + Login)

### Decision
Implemented basic authentication using email and password, with user-provided display names during registration.

### Scope
- User registration endpoint
- User login endpoint
- Email + password credential model
- Display name captured at registration

### Reasoning
- Email provides a unique, stable identifier for authentication
- Display name is user-facing and can be changed independently in the future
- Separating identity (email) from presentation (display name) aligns with common industry patterns

### Tradeoffs
- Did not implement external providers (Google, GitHub, etc.) to keep initial scope small
- Did not introduce ASP.NET Identity to avoid framework overhead and better understand core auth mechanics

### Design Notes
- Login is restricted to email + password (not display name) to avoid ambiguity and ensure uniqueness
- Endpoints follow vertical slice architecture, keeping request/response models and logic localized
- Validation kept explicit and simple to maintain readability and control