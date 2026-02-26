# Microservices Extraction Prompt

Goal
- Help an AI extract a specific bounded context/module from the existing monolithic WebAPIShop into a standalone microservice while preserving the project's coding standards and test style.

Context (what to read first)
- Open `.github/instructions/*` to apply repository-specific conventions (Controllers, Services, Repository, DTOs/Entities).
- Inspect the target code in `WebAPIShop/Controllers`, `Services`, `Repository`, `DTOs`, and `Entities` to discover method signatures, DTOs, EF models, and tests.

Step-by-step extraction prompt (for the AI agent)
1. Identify the bounded context
   - Locate controllers, services, repositories, DTOs, and entities related to the target (e.g., Orders).
   - Find all public entry points (controller actions and service public methods) and the tests that reference them.
2. Create a new service skeleton
   - Create a new solution folder `services/{target}` or `{target}Service` with projects: `API` (Web API), `Domain/Entities`, `Repository` (EF Core), `Services` (business logic), `DTOs`, and `Tests`.
   - Copy relevant DTOs and Entities into the new service, update namespaces and project references.
   - Follow naming conventions from `.github/instructions/*` (Controller suffix, Service suffix, Async suffix for async methods).
3. Define contracts and API surface
   - Produce an OpenAPI (Swagger) contract for the public controller surface of the new microservice. Keep versioned route templates (e.g., `/api/v1/orders`).
   - Create minimal DTOs solely for the contract; avoid leaking internal EF models.
4. Migrate data model and DB strategy
   - Create a new EF Core `DbContext` inside the service and migrate only the required entity sets.
   - Each microservice must own its database. Add migrations and SQL scripts for initial schema.
   - For initial cutover, use dual-write or event-driven synchronization from the monolith to keep data consistent.
5. Implement business logic and repository
   - Move service logic from monolith `Services/OrderService.cs` into new service `Services` project, keeping method names and signatures where possible.
   - Implement repository interfaces in the service, inject new `DbContext` via DI, and keep async naming conventions and cancellation tokens where appropriate.
6. Communication and integration
   - For synchronous needs, expose REST endpoints (or gRPC if low-latency internal calls are required). Document choice in the commit message.
   - Implement event publication for domain events (e.g., `OrderCreated`, `OrderUpdated`) using a message broker (RabbitMQ/Kafka). Create an `Events` project or folder with event DTOs.
   - Update the monolith to publish the same events (or send to a translator) until monolith is removed.
7. Tests and verification
   - Port or recreate unit tests into the new Tests project following the repository style: xUnit, Moq, Moq.EntityFrameworkCore, AAA pattern, method naming `Method_Scenario_Result`.
   - Add integration tests that exercise the service API and DB using `DatabaseFixture` or an in-memory/SQLite DB as used in the repo.
   - Add contract tests (consumer-driven) for any other service that depends on this service.
8. Cutover & cleanup
   - Deploy the new service behind API Gateway and route subset of traffic (strangler). Monitor metrics and logs.
   - Replace monolith handler with calls to the new service (or remove duplicated logic) once parity is validated.
   - Remove moved code from monolith and update CI to build the new service.

Checklist for each extraction (must pass before merge)
- [ ] New service builds and passes unit tests.
- [ ] OpenAPI contract created and versioned; contract tests exist for clients.
- [ ] Database migrations included and can be run in CI.
- [ ] Event contracts (if any) published and the monolith and new service can both publish/consume during migration.
- [ ] CI/CD pipeline added to build, test, and publish Docker images.
- [ ] Health endpoint, readiness/liveness checks, and basic metrics/tracing are implemented.
- [ ] Logging format and error handling follow `.github/instructions/*` guidelines.
- [ ] Backward compatibility validated (API Gateway routing + tests).

Operational recommendations
- Containerize and run services on Kubernetes or a container orchestrator. Use a centralized log/trace system (OpenTelemetry + ELK/Jaeger).
- Use JWT/OAuth2 for external requests and mTLS or short-lived tokens for service-to-service auth.
- Roll migrations and releases with blue/green or canary deployments.

How to run this prompt
- Replace `{target}` with the bounded context name (e.g., `Order`).
- The AI should open code files in the monolith to enumerate usages before changing files.
- Make small, incremental commits that each do one thing (skeleton, DTOs, API, service logic, events, tests). Include clear PR descriptions describing the migration step.

If you need to extract a specific module now, say which module (e.g., `Order`) and I will produce the concrete file-by-file extraction plan and the exact set of code edits and tests required.