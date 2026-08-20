# CommerceAI

CommerceAI is a production-oriented e-commerce backend built with **.NET 9**, PostgreSQL, Docker, and modern backend engineering practices.

The project is being developed as a hands-on learning project with two goals:

1. Build a realistic, maintainable backend.
2. Practice the technologies and engineering practices expected from a Mid/Senior .NET Backend Developer.

The project will progressively incorporate **AI/LLM capabilities** as first-class features rather than treating AI as an isolated add-on.

---

# Tech Stack

## Backend

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- MediatR
- FluentValidation

## Architecture

- Clean Architecture
- CQRS
- Domain-Driven Design concepts
- Dependency Injection
- Repository Pattern where appropriate
- Feature-oriented organization

## Infrastructure

- Docker
- Docker Compose
- Redis
- RabbitMQ
- Nginx

## AI

Planned:

- LLM APIs
- Structured Outputs
- Tool / Function Calling
- Streaming
- Embeddings
- pgvector
- Semantic Search
- RAG
- AI-powered recommendations

## DevOps

Planned:

- Git
- GitHub
- GitHub Actions
- CI
- CD
- Docker-based deployment
- VPS deployment
- Environment-based configuration
- Secrets management

## Testing

Planned:

- Unit Tests
- Integration Tests
- API Tests
- Testcontainers

---

# Architecture

The project currently follows a Clean Architecture approach.

```text
                    ┌─────────────────────┐
                    │     CommerceAI      │
                    └──────────┬──────────┘
                               │
              ┌────────────────┼────────────────┐
              │                │                │
              ▼                ▼                ▼
           API          Application          Domain
              │                │                ▲
              │                │                │
              └───────────────►│◄───────────────┘
                               │
                               ▼
                       Infrastructure
                               │
                  ┌────────────┼────────────┐
                  ▼            ▼            ▼
             PostgreSQL      Redis       RabbitMQ
```

The dependency direction is intentional:

```text
API
 ↓
Application
 ↓
Domain

Infrastructure
 ↓
Application / Domain
```

The Domain layer should remain independent from infrastructure technologies such as EF Core, PostgreSQL, Redis, or external AI providers.

---

# Project Structure

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

## Phase 1 — Foundation

### Completed
- [x] Create .NET 9 solution
- [x] Establish Clean Architecture structure
- [x] Create Domain entities
- [x] Introduce Value Objects
- [x] Configure PostgreSQL
- [x] Run PostgreSQL using Docker
- [x] Configure EF Core
- [x] Create Entity configurations
- [x] Create Repository abstraction
- [x] Implement PostgreSQL repository
- [x] Introduce CQRS
- [x] Introduce MediatR
- [x] Introduce FluentValidation
- [x] Create CreateProduct command
- [x] Create validation pipeline
- [x] Global Exception Handling
- [x] ProblemDetails
- [x] Complete CreateProduct endpoint
- [x] GetProduct query
- [x] GetProducts query
- [x] Pagination
- [x] Filtering
- [x] Sorting
- [x] No-tracking queries
- [x] API documentation with Swagger

### Testing
- [x] Create Unit Test project
- [x] Validator unit tests
- [x] Create Integration Test project
- [x] Configure Testcontainers
- [x] PostgreSQL integration tests
- [x] Run integration tests against a real PostgreSQL container

### Git & CI
- [x] Create GitHub repository
- [x] Feature branch workflow
- [x] Pull Requests
- [x] GitHub Actions
- [x] Automated restore
- [x] Automated build
- [x] Automated unit tests
- [x] Automated integration tests

### Next

- [ ] Update Product
- [ ] Delete Product
- [ ] Improve API integration tests
- [ ] Test GetProduct query
- [ ] Test GetProducts query
- [ ] Test CreateProduct handler
- [ ] Test missing product scenarios
- [ ] API documentation with Swagger
- [ ] Code coverage

---

# Phase 2 — Production Backend

## Authentication

- [ ] User registration
- [ ] Login
- [ ] Password hashing
- [ ] JWT authentication
- [ ] Refresh tokens
- [ ] Role-based authorization
- [ ] Permission-based authorization

## Reliability

- [x] Global exception handling
- [x] ProblemDetails
- [x] Request validation
- [ ] Structured logging
- [ ] Health checks
- [ ] Retry policies
- [ ] Timeout policies
- [ ] Rate limiting

## Data

- [x] Pagination
- [x] Filtering
- [x] Sorting
- [x] Optimized queries
- [x] No-tracking queries
- [ ] Database indexes
- [ ] Concurrency handling

---

# Phase 3 — Distributed Components

## Redis

- [ ] Redis integration
- [ ] Distributed caching
- [ ] Cache-aside pattern
- [ ] Cache invalidation
- [ ] Distributed locking concepts

## RabbitMQ

- [ ] Message publishing
- [ ] Message consumers
- [ ] Event-driven communication
- [ ] Retry handling
- [ ] Dead-letter queues
- [ ] Idempotent consumers
- [ ] Outbox Pattern

---

# Phase 4 — AI / LLM

AI will be treated as a first-class capability of CommerceAI.

## LLM Integration

- [ ] Introduce AI abstraction
- [ ] Implement LLM provider
- [ ] Prompt management
- [ ] Structured outputs
- [ ] Token usage tracking
- [ ] Cost tracking
- [ ] Retry and timeout handling
- [ ] AI response caching

## AI Product Features

- [ ] AI product description generation
- [ ] AI product categorization
- [ ] Review summarization
- [ ] AI-powered product assistant
- [ ] Product recommendation
- [ ] Natural-language product search

## Embeddings

- [ ] Generate product embeddings
- [ ] Store embeddings in PostgreSQL
- [ ] Configure pgvector
- [ ] Semantic search
- [ ] Hybrid search

## RAG

- [ ] Document ingestion
- [ ] Chunking
- [ ] Embedding pipeline
- [ ] Retrieval
- [ ] Context construction
- [ ] RAG-based product assistant
- [ ] Evaluate retrieval quality

## Advanced AI

- [ ] Tool / Function Calling
- [ ] Streaming responses
- [ ] Conversation history
- [ ] AI memory concepts
- [ ] Agentic workflows
- [ ] Guardrails

---

# Phase 5 — Testing

## Unit Tests

- [ ] Domain tests
- [ ] Value Object tests
- [ ] Command handler tests
- [x] Validator tests
- [ ] Business rule tests

## Integration Tests

- [x] PostgreSQL integration tests
- [x] Repository tests
- [ ] API integration tests
- [x] Testcontainers
- [ ] RabbitMQ integration tests
- [ ] Redis integration tests

## Quality

- [ ] Code coverage
- [ ] Static analysis
- [ ] Architecture tests
- [ ] Performance tests

---

# Phase 6 — Docker & Containerization

- [x] PostgreSQL Docker container
- [ ] API Dockerfile
- [ ] Multi-stage Docker build
- [ ] Run API with Docker
- [ ] Docker Compose development environment
- [ ] Environment-specific configuration
- [ ] Container health checks
- [ ] Non-root containers
- [ ] Image optimization

## Target architecture

```text
Docker Compose

├── API
├── PostgreSQL
├── Redis
└── RabbitMQ
```

---

# Phase 7 — CI/CD

The CI/CD pipeline will be built incrementally.

## Continuous Integration

- [X] GitHub repository
- [X] Branch strategy
- [X] Pull Requests
- [X] GitHub Actions
- [X] Restore dependencies
- [X] Build solution
- [X] Run unit tests
- [X] Run integration tests
- [ ] Generate test reports
- [ ] Code coverage
- [ ] Docker image build
- [ ] Docker image validation

## Current CI

```text
Pull Request
     │
     ▼
GitHub Actions
     │
     ├── Restore
     ├── Build
     ├── Unit Tests
     └── Integration Tests
              │
              ▼
         Testcontainers
              │
              ▼
         PostgreSQL
     │
     ▼
   PASS
```

## Target CI

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
     ├── Coverage
     ├── Security Checks
     └── Docker Build
     │
     ▼
   PASS
```

## Continuous Deployment

Eventually:

```text
main
 │
 ▼
GitHub Actions
 │
 ├── Build
 ├── Test
 ├── Build Docker Image
 ├── Push Image
 │
 ▼
Production VPS
 │
 ├── Pull Image
 ├── Run migrations
 ├── Restart containers
 └── Health Check
```

Planned:

- [ ] Production environment
- [ ] GitHub Environments
- [ ] GitHub Secrets
- [ ] Container Registry
- [ ] SSH deployment
- [ ] VPS deployment
- [ ] Zero/minimal downtime deployment
- [ ] Rollback strategy

---

# Phase 8 — Observability

- [ ] Structured logging
- [ ] Correlation IDs
- [ ] Request tracing
- [ ] Metrics
- [ ] Health checks
- [ ] Readiness checks
- [ ] Liveness checks
- [ ] OpenTelemetry
- [ ] Distributed tracing
- [ ] Error monitoring

---

# Git Workflow

We will use Git as part of the learning process rather than only as a place to store the code.

Example branches:

```text
main
 │
 ├── feature/create-product
 ├── feature/get-products
 ├── feature/authentication
 ├── feature/ai-product-description
 ├── feature/vector-search
 └── fix/product-validation
```

Pull Requests will eventually trigger CI automatically.

---

# Commit Convention

We will use Conventional Commits.

Examples:

```text
feat: add product creation command
feat: add semantic product search
fix: handle invalid product price
refactor: extract product repository
test: add product handler tests
docs: update deployment guide
ci: add build workflow
chore: update dependencies
```

---

# Engineering Principles

Throughout the project we will intentionally practice:

- SOLID
- Separation of Concerns
- Dependency Inversion
- Clean Architecture
- Domain-driven design concepts
- CQRS
- Idempotency
- Resilience
- Observability
- Security
- Performance
- Testability
- Automation

Patterns will not be introduced just because they exist.

For every architectural pattern we will ask:

> What problem does this solve?

and:

> When should we NOT use it?

---

# Interview Preparation

This project is also an interview preparation environment.

Topics that will be covered include:

## .NET

- Dependency Injection
- Middleware
- Filters
- Configuration
- Options Pattern
- Hosted Services
- CancellationToken
- Async/Await
- Memory management
- Performance

## Architecture

- Clean Architecture
- CQRS
- DDD concepts
- Repository Pattern
- Unit of Work
- Modular Monolith
- Microservices
- Event-driven architecture

## Databases

- PostgreSQL
- Indexes
- Transactions
- Isolation levels
- Query optimization
- Concurrency
- EF Core internals

## Distributed Systems

- Redis
- RabbitMQ
- Outbox Pattern
- Idempotency
- Retry
- Circuit Breaker
- Eventual consistency

## DevOps

- Docker
- CI/CD
- GitHub Actions
- Linux
- Nginx
- HTTPS
- VPS deployment
- Secrets
- Monitoring

## AI Engineering

- LLM APIs
- Prompt engineering
- Structured output
- Tool calling
- Embeddings
- Vector databases
- RAG
- AI reliability
- AI cost optimization

---

# Current Status

**Current phase:** Foundation

**Current focus:** Automated testing and integration testing

Current application flow:

```text
API
 ↓
MediatR
 ↓
Validation Pipeline
 ↓
Handler
 ↓
Repository
 ↓
EF Core
 ↓
PostgreSQL
```

--- 

# Learning Philosophy

CommerceAI is intentionally developed incrementally.

We will not build every feature at once.

Each feature should introduce one or more real engineering concepts, and whenever possible the implementation will be tested, containerized, automated, and eventually deployed.

The goal is not just to make the application work.

The goal is to understand **why it works, how it can fail, and how to operate it in production.**
