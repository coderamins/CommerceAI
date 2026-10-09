# CommerceAI

CommerceAI is a production-oriented e-commerce backend built with **.NET 9, ASP.NET Core, PostgreSQL, and modern backend engineering practices**.

This project serves two purposes:

1. Build a realistic, maintainable backend using production-oriented engineering principles.
2. Develop the skills expected of a Mid/Senior .NET Backend Developer through hands-on implementation, testing, deployment, and interview preparation.

AI/LLM capabilities will eventually become first-class features of the application rather than an isolated add-on.

---

## Tech Stack

### Backend

* .NET 9
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* MediatR
* FluentValidation

### Architecture

* Clean Architecture
* CQRS
* Domain-Driven Design concepts
* Dependency Injection
* Repository Pattern
* Separation of Concerns

### Testing

* xUnit
* ASP.NET Core `WebApplicationFactory`
* Testcontainers for PostgreSQL
* Respawn for database cleanup between integration tests

### Infrastructure and DevOps

* Docker
* Git and GitHub
* GitHub Actions
* PostgreSQL
* Planned: Redis, RabbitMQ, Nginx, VPS deployment

### AI/LLM — Planned

* LLM APIs
* Structured Outputs
* Tool/Function Calling
* Streaming
* Embeddings
* pgvector
* Semantic Search
* Retrieval-Augmented Generation (RAG)
* AI-powered recommendations

---

## Architecture

CommerceAI follows Clean Architecture principles, with dependencies directed toward the Domain and Application layers.

```text
CommerceAI
│
├── CommerceAI.API
│   └── HTTP endpoints, request/response contracts,
│       exception handling, API configuration
│
├── CommerceAI.Application
│   └── Commands, queries, handlers, validators,
│       application interfaces
│
├── CommerceAI.Domain
│   └── Entities, value objects, business rules
│
└── CommerceAI.Infrastructure
    └── EF Core, PostgreSQL, repository implementations
```

### Dependency direction

```text
API ───────────────► Application
                         │
                         ▼
                       Domain

Infrastructure ────► Application / Domain
```

The Domain layer should remain independent of infrastructure technologies such as EF Core, PostgreSQL, Redis, RabbitMQ, and external AI providers.

### Current Product request flow

```text
HTTP Request
     │
     ▼
ProductsController
     │
     ▼
MediatR
     │
     ▼
Validation Pipeline
     │
     ▼
Command / Query Handler
     │
     ▼
IProductRepository
     │
     ▼
EF Core
     │
     ▼
PostgreSQL
```

---

## Project Structure

```text
CommerceAI/
│
├── src/
│   ├── CommerceAI.API/
│   ├── CommerceAI.Application/
│   ├── CommerceAI.Domain/
│   └── CommerceAI.Infrastructure/
│
├── tests/
│   ├── CommerceAI.UnitTests/
│   └── CommerceAI.IntegrationTests/
│
├── docker-compose.yml
├── README.md
└── CommerceAI.sln
```

---

# Development Roadmap

## Phase 1 — Foundation and Product CRUD

### Solution and persistence

* [x] Create .NET 9 solution
* [x] Establish Clean Architecture structure
* [x] Create Domain entities
* [x] Introduce Value Objects
* [x] Configure PostgreSQL
* [x] Run PostgreSQL using Docker
* [x] Configure EF Core
* [x] Create entity configurations
* [x] Create repository abstraction
* [x] Implement PostgreSQL repository
* [x] Configure database migrations

### Application architecture

* [x] Introduce CQRS
* [x] Introduce MediatR
* [x] Introduce FluentValidation
* [x] Create validation pipeline
* [x] Separate API request contracts from application commands

### Product endpoints

* [x] Create product
* [x] Get product by ID
* [x] List products
* [x] Implement pagination
* [x] Implement filtering
* [x] Implement sorting
* [x] Implement product update
* [x] Implement product deletion
* [x] Configure Swagger API documentation

### API reliability

* [x] Global exception handling
* [x] Standardized ProblemDetails responses
* [x] Validation errors mapped to HTTP 400
* [x] Missing resources mapped to HTTP 404

### Remaining foundation work

* [ ] Review and expand unit tests
* [ ] Review API contract consistency and error responses
* [ ] Add API endpoint documentation and examples where needed

---

## Phase 2 — Production Backend

### Authentication and authorization

* [ ] User registration
* [ ] Login
* [ ] Secure password hashing
* [ ] JWT authentication
* [ ] Refresh-token lifecycle and rotation
* [ ] Role-based authorization
* [ ] Permission-based authorization
* [ ] Authentication and authorization integration tests

### Reliability and resilience

* [ ] Structured logging
* [ ] Health checks
* [ ] Liveness and readiness checks
* [ ] Rate limiting
* [ ] Request timeouts
* [ ] Retry policies where appropriate
* [ ] Resilience policies for external dependencies
* [ ] Consistent error handling conventions

### Data and concurrency

* [x] Pagination
* [x] Filtering
* [x] Sorting
* [ ] Query optimization
* [ ] No-tracking queries where appropriate
* [ ] Database indexes
* [ ] Optimistic concurrency using PostgreSQL `xmin`
* [ ] Map concurrency conflicts to HTTP 409
* [ ] Integration tests for stale updates
* [ ] Review transaction boundaries and isolation levels

---

## Phase 3 — Testing and Code Quality

### Integration testing

* [x] Configure xUnit integration tests
* [x] Use `WebApplicationFactory<Program>`
* [x] Run PostgreSQL through Testcontainers
* [x] Apply migrations to the test database
* [x] Reset database state between tests using Respawn
* [x] Test product retrieval and listing
* [x] Test pagination, filtering, and sorting
* [x] Test product update and deletion scenarios
* [ ] Expand test coverage for edge cases
* [ ] Add repository-specific integration tests where valuable

### Unit testing

* [ ] Domain entity tests
* [ ] Value Object tests
* [ ] Command handler tests
* [ ] Validator tests
* [ ] Business rule tests

### Quality and performance

* [ ] Code coverage reporting
* [ ] Static analysis
* [ ] Architecture tests
* [ ] Performance testing
* [ ] Database query performance analysis

---

## Phase 4 — Docker and Containerization

* [x] Run PostgreSQL in Docker
* [ ] Create API Dockerfile
* [ ] Implement multi-stage Docker build
* [ ] Run API in a container
* [ ] Configure development environment with Docker Compose
* [ ] Configure environment-specific settings
* [ ] Add container health checks
* [ ] Run containers as non-root users where appropriate
* [ ] Optimize image size and build caching
* [ ] Configure persistent database storage
* [ ] Document local development setup

### Target development environment

```text
Docker Compose
│
├── CommerceAI API
├── PostgreSQL
├── Redis
└── RabbitMQ
```

Redis and RabbitMQ will be introduced when their use cases are implemented.

---

## Phase 5 — CI/CD and Deployment

### Continuous Integration

* [x] Create GitHub repository
* [x] Configure GitHub Actions workflow
* [x] Verify successful CI build
* [ ] Confirm automated test execution in CI
* [ ] Run integration tests in CI using Testcontainers
* [ ] Publish test results
* [ ] Add code coverage reporting
* [ ] Build and validate API Docker image
* [ ] Establish and document branch strategy
* [ ] Practice Pull Request reviews
* [ ] Add branch protection rules where appropriate

### Continuous Deployment

* [ ] Prepare production configuration
* [ ] Configure GitHub Environments
* [ ] Configure GitHub Secrets
* [ ] Set up a container registry
* [ ] Provision a Linux VPS
* [ ] Configure SSH-based deployment
* [ ] Deploy CommerceAI to the VPS
* [ ] Configure HTTPS and reverse proxy
* [ ] Apply database migrations safely during deployment
* [ ] Add post-deployment health checks
* [ ] Design rollback procedures
* [ ] Explore zero/minimal-downtime deployment

### Target pipeline

```text
Pull Request
     │
     ▼
GitHub Actions
     │
     ├── Restore
     ├── Build
     ├── Unit Tests
     ├── Integration Tests
     ├── Test Reports
     └── Docker Build
             │
             ▼
          CI Pass
```

```text
Merge to main
     │
     ▼
Build and Test
     │
     ▼
Build Docker Image
     │
     ▼
Push to Registry
     │
     ▼
Deploy to VPS
     │
     ├── Apply migrations
     ├── Restart application
     └── Verify health
```

---

## Phase 6 — Distributed Components

### Redis

* [ ] Integrate Redis
* [ ] Implement distributed caching
* [ ] Apply cache-aside pattern
* [ ] Design cache invalidation
* [ ] Understand cache consistency
* [ ] Explore distributed locking and its limitations

### RabbitMQ

* [ ] Integrate RabbitMQ
* [ ] Publish messages
* [ ] Implement message consumers
* [ ] Introduce domain and integration events
* [ ] Implement retry handling
* [ ] Configure dead-letter queues
* [ ] Implement idempotent consumers
* [ ] Implement the Outbox Pattern
* [ ] Test message delivery and failure scenarios

### Distributed systems concepts

* [ ] Event-driven architecture
* [ ] Eventual consistency
* [ ] At-least-once delivery
* [ ] Idempotency
* [ ] Retry and backoff
* [ ] Circuit breakers
* [ ] Transactional messaging

---

## Phase 7 — AI and LLM Integration

AI capabilities will be added incrementally, with an emphasis on reliability, security, evaluation, and cost control.

### LLM integration

* [ ] Introduce an AI provider abstraction
* [ ] Integrate an LLM provider
* [ ] Manage prompts and versions
* [ ] Implement structured outputs
* [ ] Implement tool/function calling
* [ ] Handle streaming responses
* [ ] Track token usage and cost
* [ ] Configure timeouts and retry policies
* [ ] Cache suitable AI responses
* [ ] Handle provider errors and rate limits

### AI-powered product features

* [ ] Generate product descriptions
* [ ] Categorize products
* [ ] Summarize customer reviews
* [ ] Build a product assistant
* [ ] Recommend relevant products
* [ ] Support natural-language product search

### Embeddings and semantic search

* [ ] Generate product embeddings
* [ ] Store embeddings in PostgreSQL
* [ ] Configure pgvector
* [ ] Implement semantic search
* [ ] Explore hybrid search
* [ ] Evaluate search relevance

### Retrieval-Augmented Generation (RAG)

* [ ] Build document ingestion
* [ ] Implement chunking
* [ ] Generate and store embeddings
* [ ] Implement retrieval
* [ ] Construct grounded context
* [ ] Build a RAG-based product assistant
* [ ] Evaluate retrieval and answer quality
* [ ] Reduce hallucination and prompt-injection risks

### Advanced AI engineering

* [ ] Conversation history
* [ ] AI memory concepts
* [ ] Agentic workflows
* [ ] Tool execution safeguards
* [ ] Guardrails
* [ ] AI observability
* [ ] Cost and latency optimization

---

## Phase 8 — Observability and Operations

* [ ] Structured logging
* [ ] Correlation IDs
* [ ] Request tracing
* [ ] Metrics
* [ ] Liveness and readiness checks
* [ ] OpenTelemetry
* [ ] Distributed tracing
* [ ] Error monitoring
* [ ] Dashboards and alerts
* [ ] Production troubleshooting guide

---

# Git Workflow

Git is part of the learning process, not just a place to store the code.

Example branch strategy:

```text
main
│
├── feature/product-crud
├── feature/optimistic-concurrency
├── feature/authentication
├── feature/ai-product-description
└── fix/product-validation
```

The intended workflow is:

1. Create a focused feature or fix branch.
2. Implement the change and tests.
3. Commit using Conventional Commits.
4. Open a Pull Request.
5. Review the changes and verify CI.
6. Merge after successful checks.

---

## Commit Convention

CommerceAI uses the Conventional Commits style.

Examples:

```text
feat: add product creation command
feat: add product update endpoint
feat: add semantic product search
fix: handle missing product during deletion
test: add product update integration tests
refactor: extract product repository
docs: update project roadmap
ci: run integration tests in GitHub Actions
chore: update dependencies
```

---

# Engineering Principles

Throughout the project, we will practice:

* SOLID
* Separation of Concerns
* Dependency Inversion
* Clean Architecture
* Domain-Driven Design concepts
* CQRS
* Idempotency
* Resilience
* Observability
* Security
* Performance
* Testability
* Automation

Patterns will be introduced to solve actual problems, not merely because they exist.

For every architectural decision, ask:

> What problem does this solve?

> What are the trade-offs?

> When would a simpler approach be better?

---

# Interview Preparation

CommerceAI is also a practical environment for Mid/Senior .NET interview preparation.

### .NET and ASP.NET Core

* Dependency Injection
* Middleware and exception handling
* Filters
* Configuration and Options Pattern
* Hosted Services
* CancellationToken
* Async/Await
* Memory management
* Performance and allocations

### Architecture

* Clean Architecture
* CQRS
* Domain-Driven Design concepts
* Repository Pattern
* Unit of Work
* Modular Monolith
* Microservices
* Event-driven architecture
* Architectural trade-offs

### Databases and EF Core

* PostgreSQL
* Indexes
* Transactions
* Isolation levels
* Query optimization
* Optimistic concurrency
* EF Core change tracking
* Migrations and schema evolution

### Distributed Systems

* Redis
* RabbitMQ
* Outbox Pattern
* Idempotency
* Retries and circuit breakers
* Eventual consistency
* Message delivery guarantees

### DevOps

* Docker
* CI/CD
* GitHub Actions
* Linux
* Nginx
* HTTPS
* VPS deployment
* Secrets management
* Monitoring

### AI Engineering

* LLM APIs
* Prompt engineering
* Structured outputs
* Tool calling
* Embeddings
* Vector search
* RAG
* AI reliability
* AI evaluation and cost optimization

---

# Current Status

**Current phase:** Foundation — Product CRUD and integration testing

**Completed milestone:** Core Product API with PostgreSQL persistence and automated integration tests.

**Current focus:** Verify the Product update/delete flows, then implement and test optimistic concurrency.

```text
Product CRUD
     │
     ▼
Integration Tests
     │
     ▼
Optimistic Concurrency
     │
     ▼
Authentication and Authorization
     │
     ▼
Dockerize the API
     │
     ▼
CI/CD and VPS Deployment
     │
     ▼
Distributed Components
     │
     ▼
AI/LLM Features
```

--- 

# Learning Philosophy

CommerceAI is developed incrementally.

Each feature should introduce real engineering concepts and, wherever practical, include tests, automation, operational considerations, and documentation.

The goal is not merely to make the application work.

The goal is to understand **why it works, how it can fail, what trade-offs it makes, and how to operate it in production**.
