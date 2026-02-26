# API / Controllers (`WebAPIShop/Controllers`)

Responsibilities
- Accept HTTP requests, validate inputs, map request DTOs to service calls, and return appropriate HTTP responses.
- Translate service results into `ActionResult<T>` and status codes (e.g., `200`, `201`, `400`, `404`).
- Do not contain business logic, direct database access, or long-running operations.

Naming Conventions
- Controller classes SHOULD be suffixed with `Controller` (e.g., `OrdersController`).
- Action methods SHOULD use clear verbs and may include `Async` for async methods (e.g., `GetOrderByIdAsync`, `CreateOrderAsync`).
- Use `ActionResult<T>` or `IActionResult` as return types for endpoints.
- Route templates should be explicit (e.g., `[Route("api/[controller]")]`).

Dependencies
- Controllers MUST depend only on service interfaces (inject via constructor DI), e.g., `IOrderService`, `IUserService` from the `Services` project.
- Use `ILogger<T>` via DI for logging.
- Do NOT instantiate `db_shopContext` or repositories directly.

Error Handling
- Delegate domain errors to services. Return HTTP-friendly responses for expected errors (e.g., validation ? `BadRequest`, not found ? `NotFound`).
- Avoid catching broad exceptions in controllers; rely on centralized error middleware (e.g., `ErrorHandlingMiddleware`) to convert unexpected exceptions into `5xx` responses.
- If catching specific exceptions to translate to HTTP codes, rethrow or wrap only when adding context.

Testing Guidance
- Unit test controllers by mocking service interfaces and asserting returned `ActionResult` and status codes.
- Avoid trying to test EF/DB behavior from controllers; those belong to repository/integration tests.
