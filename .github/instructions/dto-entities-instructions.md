# DTOs / Entities (`DTOs`, `Entities`)

Responsibilities
- DTOs: transport objects for API input/output; contain validation attributes only and minimal logic.
- Entities: EF Core persistence models representing DB schema and navigation properties.
- Keep mapping between DTOs and Entities explicit (use `AutoMapper` profiles or mapping methods).

Naming Conventions
- DTO classes should end with `DTO` (e.g., `OrderDTO`, `OrderItemDTO`).
- Entity classes use domain nouns (e.g., `Order`, `OrderItem`).
- Property names follow PascalCase and map closely to DB/JSON fields.

Dependencies
- DTOs consumed by Controllers and Services; should not depend on Repositories or DbContext.
- Entities consumed by Repositories and Services; avoid referencing DTOs in entity classes.
- Keep mapping configurations in a dedicated `AutoMapping` profile.

Error Handling
- Validate DTOs at the controller boundary using model binding and annotations.
- DTOs should not throw domain exceptions; validation failures should result in `BadRequest` responses from controllers.
- Entities should not contain transport-layer validation logic.

Testing Guidance
- Test mapping profiles to ensure DTO ? Entity conversions are correct.
- Keep DTOs lightweight to simplify testing and serialization behavior.
