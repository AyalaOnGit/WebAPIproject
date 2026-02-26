# Services / Business Logic (`Services` / `Servers`)

Responsibilities
- Implement and enforce business rules, coordinate multiple repositories, and manage transactions and cross-cutting concerns.
- Transform between domain entities and DTOs (use `AutoMapper` or explicit mapping).
- Do not perform HTTP concerns, controller-level responsibilities, or direct DbContext management.

Naming Conventions
- Interfaces prefixed with `I` and implementations suffixed with `Service` (e.g., `IOrderService` / `OrderService`).
- Public async methods MUST end with `Async` (e.g., `AddOrderAsync`, `GetOrdersAsync`).
- Method names should describe behavior (e.g., `CalculateOrderTotalAsync`) rather than transport layer intent.

Dependencies
- Services may depend on repository interfaces, other services, `IMapper`, and `ILogger<T>` via constructor DI.
- Services SHOULD NOT instantiate repositories or DbContext directly.
- Accept `CancellationToken` for public async methods when appropriate.

Error Handling
- Validate inputs and throw domain-specific exceptions (e.g., `NotFoundException`, `ValidationException`) to be handled by upper layers or middleware.
- Avoid swallowing exceptions; catch only when handling recoverable scenarios and wrap exceptions with contextual information when rethrowing.
- Log important errors at `Error` level and informational flow at `Information` level.

Testing Guidance
- Unit test services by mocking repositories and other dependent services.
- Prefer testing business logic in isolation; use integration tests for transaction/Db behavior.
