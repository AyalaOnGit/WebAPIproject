# Repository / Data Access (`Repository`)

Responsibilities
- Encapsulate EF Core `db_shopContext` interactions: queries, persistence, and mapping to entities.
- Provide a concise API for CRUD operations and queries specific to the domain.
- Do not implement business rules or HTTP concerns.

Naming Conventions
- Interfaces should be prefixed with `I` (e.g., `IOrderRepository`) and concrete implementations suffixed with `Repository` (e.g., `OrderRepository`).
- Async DB methods MUST end with `Async` (e.g., `GetByIdAsync`, `AddAsync`, `SaveChangesAsync`).
- Return entity types or domain-level DTOs; avoid returning controller DTOs from repositories.

Dependencies
- Repositories should depend only on `Entities`, `db_shopContext`, and infrastructure components like `ILogger`.
- Inject `db_shopContext` via constructor DI.
- Use `AsNoTracking()` for read-only queries where appropriate.

Error Handling
- Let EF Core exceptions bubble up for middleware to translate, unless implementing retries or specific DB error translations.
- When catching and rethrowing, preserve the original exception as InnerException and add contextual message.
- Never swallow exceptions or return null silently for operations that failed; prefer returning clear `Result` objects if needed.

Testing Guidance
- Unit test repositories with an in-memory or SQLite test DB for realistic behavior.
- For unit tests against mocked `DbSet`, use helper extensions that correctly mock `IQueryable` behaviors.
