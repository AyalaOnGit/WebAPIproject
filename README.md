# 🛒 WebAPIShop
### **Modern RESTful API | .NET 9 | C# | Layered Architecture**

---

## 📖 Overview
**WebAPIShop** is a professional **REST Web API** built with **.NET 9** and **C#**. The project strictly adheres to **RESTful principles**, providing a standardized and scalable way to interact with data over HTTPS. It is designed with a focus on high performance, maintainability, and **Clean Code**.

---

## 🏗️ Architecture & Design Patterns

The project is structured using a **Layered Architecture** to achieve total **Separation of Concerns**:

* 📱 **Application Layer** – Handles API controllers, routing, and ensures **REST principles** are followed.
* ⚙️ **Service Layer** – Contains all **Business Logic**, facilitating communication between layers.
* 🗄️ **Repository Layer** – Manages **Data Access** logic and database communication.

### Key Technical Features:
* 💉 **Dependency Injection (DI):** Implemented across all layers to create **Decoupling** and improve system flexibility.
* ⚡ **Asynchronous Programming:** Database access is handled **Asynchronously** to free up threads and ensure maximum **Scalability**.
* 🗃️ **Entity Framework Core (ORM):** Developed using the **DB-First** approach for efficient data management.
* 📦 **DTOs & Records:** Uses **C# Records** for **Data Transfer Objects** to remove circular dependencies and decouple the Data layer from the API layers.
* 🔄 **AutoMapper:** Used for automatic and clean mapping between Database Entities and DTOs.
* ⚙️ **Configuration:** Settings are managed via `appsettings.json` to keep the code clean and environment-flexible.

---

## 📁 Project Structure

```text
├── WebAPIShop/           # Entry point, controllers, middleware
├── Services/             # Business logic implementations
├── Repository/           # Data access implementations
├── Entities/             # Domain models (Database Entities)
├── DTOs/                 # Record-based data transfer objects
├── KafkaConsumer/        # Worker service — listens to order events
├── TestProject1/         # xUnit test projects (Unit & Integration)
├── docker-compose.yml    # Full environment orchestration
└── appsettings.json      # External configuration
```

---

## 🛡️ Security & Authentication

| Feature | Description |
| :--- | :--- |
| **JWT Bearer** | Token generated in `UserService` on login/register, extracted from HttpOnly cookie and forwarded as a `Bearer` header via middleware. |
| **Role-based Auth** | `[Authorize]` for authenticated users, `[AdminOnly]` (custom `AuthorizeAttribute` with `Roles = "Admin"`) for admin-only endpoints. |
| **BCrypt Password Hashing** | Passwords are hashed using `BCrypt.Net` (`BC.HashPassword`) on register/update and verified with `BC.Verify` on login — plain-text passwords are never stored. |
| **Rate Limiting** | Sliding Window — 30 requests/minute partitioned by IP + username, with immediate rejection (no queue) to prevent abuse. |

---

## 🚀 Performance — Redis Cache

Redis distributed cache (`StackExchange.Redis`) is used in **User** and **Category** services to reduce database load:

* **Read-through:** data is served from cache when available, falling back to the DB transparently.
* **TTL:** expiration time is read from `appsettings.json` (`Redis:TTL`), defaulting to 3600 seconds.
* **Invalidation:** cache entries are removed on every write (add / update) to keep data consistent.
* **Resilience:** all Redis calls are wrapped in `try/catch` — if Redis is down the API continues serving from the DB without interruption.

---

## 📨 Messaging — Apache Kafka

New orders are published to a Kafka topic and processed asynchronously by a dedicated consumer service:

* **`KafkaProducerService`** — `IKafkaProducerService` singleton, publishes serialized `OrderDTO` to the `orders` topic (Confluent.Kafka).
* **`KafkaConsumer`** — standalone .NET Worker Service (`BackgroundService`) that subscribes to the `orders` topic and logs each incoming order event.
* Topic name and bootstrap servers are configured via `appsettings.json` / environment variables.

---

## 🛡️ Reliability & Monitoring

A robust system requires proactive monitoring and error management to ensure stability and high availability:

| Feature | Description |
| :--- | :--- |
| **Global Error Handling** | A custom **Middleware** that intercepts all exceptions globally, providing consistent API responses and preventing system crashes. |
| **NLog Integration** | Extensive implementation of **NLog** for detailed recording of system events, warnings, and error diagnostics. |
| **Traffic Monitoring** | All incoming server requests are tracked and logged into a dedicated **Rating table** for auditing, analytics, and performance monitoring. |

---

## 🐳 Docker

The full environment is orchestrated with **Docker Compose**:

| Service | Image | Port |
| :--- | :--- | :--- |
| **api** | Custom Dockerfile (.NET 9) | `8080` |
| **consumer** | Custom Dockerfile (Worker) | — |
| **sqlserver** | `mssql/server:2022-latest` | `1433` |
| **redis** | `redis:alpine` | `6379` |
| **kafka** | `apache/kafka:latest` | `9092 / 9093` |
| **kafka-ui** | `provectuslabs/kafka-ui` | `8090` |

```bash
# Run everything
docker compose up --build
```

---

## 🤖 Developer Tooling — GitHub Copilot Agents (`.github/`)

The `.github/` directory contains reusable AI agent instructions and prompts that accelerate development:

| Path | Purpose |
| :--- | :--- |
| `.github/agent/api-architect.md` | Custom agent mode — acts as an **API Architect**, guiding engineers through adding new endpoints across all layers with resiliency patterns (Polly). |
| `.github/instructions/` | Per-layer coding standards for Controllers, Services, Repository, and DTOs — automatically applied by Copilot for every file in the matching layer. |
| `.github/prompts/create-test-prompt.md` | Reusable prompt that generates xUnit unit/integration tests matching the project's AAA style, Moq conventions, and naming patterns. |
| `.github/prompts/microservices-split-plan.md` | Architecture prompt for planning a future microservices migration. |

---

## 🧪 Testing Suite

We maintain high reliability using the **xUnit** library with a comprehensive testing strategy:

* ✅ **Unit Tests:** Validating individual business logic units in isolation to ensure correctness.
* ✅ **Integration Tests:** Ensuring the entire data flow between layers and the database works seamlessly together.

---

## 🛠️ Tech Stack

| Layer | Technology |
| :--- | :--- |
| **Framework** | .NET 9, C#, ASP.NET Core |
| **ORM** | Entity Framework Core (DB-First) |
| **Auth** | JWT Bearer + BCrypt password hashing |
| **Cache** | Redis (StackExchange.Redis) |
| **Messaging** | Apache Kafka (Confluent.Kafka) |
| **Mapping** | AutoMapper |
| **Logging** | NLog |
| **Testing** | xUnit, Moq |
| **Container** | Docker, Docker Compose |
| **Dev Tooling** | GitHub Copilot Agents & custom instructions |

---

## 🚀 Getting Started

### Prerequisites
* **.NET 9 SDK**
* Docker Desktop

### Run the full stack
```bash
docker compose up --build
```

### Run locally (without Docker)
```bash
# Restore dependencies
dotnet restore

# Apply migrations / Update database
dotnet ef database update

# Run the project
dotnet run --project WebAPIShop
```

### 🧪 Run Tests

```bash
dotnet test
```

---

## 📄 License

This project is licensed under the **MIT License**.

---
**Ayala**
<small>2026</small>
