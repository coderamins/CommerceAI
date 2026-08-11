\# CommerceAI



CommerceAI is a production-oriented e-commerce backend built with \*\*.NET 9\*\*, PostgreSQL, Docker, and modern backend engineering practices.



The project is being developed as a hands-on learning project with two goals:



1\. Build a realistic, maintainable backend.

2\. Practice the technologies and engineering practices expected from a Mid/Senior .NET Backend Developer.



The project will progressively incorporate \*\*AI/LLM capabilities\*\* as first-class features rather than treating AI as an isolated add-on.



\---



\# Tech Stack



\## Backend



\* .NET 9

\* ASP.NET Core Web API

\* Entity Framework Core

\* PostgreSQL

\* MediatR

\* FluentValidation



\## Architecture



\* Clean Architecture

\* CQRS

\* Domain-Driven Design concepts

\* Dependency Injection

\* Repository Pattern where appropriate

\* Feature-oriented organization



\## Infrastructure



\* Docker

\* Docker Compose

\* Redis

\* RabbitMQ

\* Nginx



\## AI



Planned:



\* LLM APIs

\* Structured Outputs

\* Tool / Function Calling

\* Streaming

\* Embeddings

\* pgvector

\* Semantic Search

\* RAG

\* AI-powered recommendations



\## DevOps



Planned:



\* Git

\* GitHub

\* GitHub Actions

\* CI

\* CD

\* Docker-based deployment

\* VPS deployment

\* Environment-based configuration

\* Secrets management



\## Testing



Planned:



\* Unit Tests

\* Integration Tests

\* API Tests

\* Testcontainers



\---



\# Architecture



The project currently follows a Clean Architecture approach.



```text

&#x20;                   ┌─────────────────────┐

&#x20;                   │     CommerceAI      │

&#x20;                   └──────────┬──────────┘

&#x20;                              │

&#x20;             ┌────────────────┼────────────────┐

&#x20;             │                │                │

&#x20;             ▼                ▼                ▼

&#x20;          API          Application          Domain

&#x20;             │                │                ▲

&#x20;             │                │                │

&#x20;             └───────────────►│◄───────────────┘

&#x20;                              │

&#x20;                              ▼

&#x20;                      Infrastructure

&#x20;                              │

&#x20;                 ┌────────────┼────────────┐

&#x20;                 ▼            ▼            ▼

&#x20;            PostgreSQL      Redis       RabbitMQ

```



The dependency direction is intentional:



```text

API

&#x20;↓

Application

&#x20;↓

Domain



Infrastructure

&#x20;↓

Application / Domain

```



The Domain layer should remain independent from infrastructure technologies such as EF Core, PostgreSQL, Redis, or external AI providers.



\---



\# Project Structure



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



\---



\# Development Roadmap



\## Phase 1 — Foundation



\* \[x] Create .NET 9 solution

\* \[x] Establish Clean Architecture structure

\* \[x] Create Domain entities

\* \[x] Introduce Value Objects

\* \[x] Configure PostgreSQL

\* \[x] Run PostgreSQL using Docker

\* \[x] Configure EF Core

\* \[x] Create Entity configurations

\* \[x] Create Repository abstraction

\* \[x] Implement PostgreSQL repository

\* \[x] Introduce CQRS

\* \[x] Introduce MediatR

\* \[x] Introduce FluentValidation

\* \[x] Create CreateProduct command

\* \[x] Create validation pipeline



\### Next



\* \[ ] Global Exception Handling

\* \[ ] ProblemDetails

\* \[ ] Complete CreateProduct endpoint

\* \[ ] GetProduct query

\* \[ ] GetProducts query

\* \[ ] UpdateProduct command

\* \[ ] DeleteProduct command

\* \[ ] API documentation with Swagger

\* \[ ] Add initial unit tests



\---



\# Phase 2 — Production Backend



\## Authentication



\* \[ ] User registration

\* \[ ] Login

\* \[ ] Password hashing

\* \[ ] JWT authentication

\* \[ ] Refresh tokens

\* \[ ] Role-based authorization

\* \[ ] Permission-based authorization



\## Reliability



\* \[ ] Global exception handling

\* \[ ] ProblemDetails

\* \[ ] Request validation

\* \[ ] Structured logging

\* \[ ] Health checks

\* \[ ] Retry policies

\* \[ ] Timeout policies

\* \[ ] Rate limiting



\## Data



\* \[ ] Pagination

\* \[ ] Filtering

\* \[ ] Sorting

\* \[ ] Optimized queries

\* \[ ] No-tracking queries

\* \[ ] Database indexes

\* \[ ] Concurrency handling



\---



\# Phase 3 — Distributed Components



\## Redis



\* \[ ] Redis integration

\* \[ ] Distributed caching

\* \[ ] Cache-aside pattern

\* \[ ] Cache invalidation

\* \[ ] Distributed locking concepts



\## RabbitMQ



\* \[ ] Message publishing

\* \[ ] Message consumers

\* \[ ] Event-driven communication

\* \[ ] Retry handling

\* \[ ] Dead-letter queues

\* \[ ] Idempotent consumers

\* \[ ] Outbox Pattern



\---



\# Phase 4 — AI / LLM



AI will be treated as a first-class capability of CommerceAI.



\## LLM Integration



\* \[ ] Introduce AI abstraction

\* \[ ] Implement LLM provider

\* \[ ] Prompt management

\* \[ ] Structured outputs

\* \[ ] Token usage tracking

\* \[ ] Cost tracking

\* \[ ] Retry and timeout handling

\* \[ ] AI response caching



\## AI Product Features



\* \[ ] AI product description generation

\* \[ ] AI product categorization

\* \[ ] Review summarization

\* \[ ] AI-powered product assistant

\* \[ ] Product recommendation

\* \[ ] Natural-language product search



\## Embeddings



\* \[ ] Generate product embeddings

\* \[ ] Store embeddings in PostgreSQL

\* \[ ] Configure pgvector

\* \[ ] Semantic search

\* \[ ] Hybrid search



\## RAG



\* \[ ] Document ingestion

\* \[ ] Chunking

\* \[ ] Embedding pipeline

\* \[ ] Retrieval

\* \[ ] Context construction

\* \[ ] RAG-based product assistant

\* \[ ] Evaluate retrieval quality



\## Advanced AI



\* \[ ] Tool / Function Calling

\* \[ ] Streaming responses

\* \[ ] Conversation history

\* \[ ] AI memory concepts

\* \[ ] Agentic workflows

\* \[ ] Guardrails



\---



\# Phase 5 — Testing



\## Unit Tests



\* \[ ] Domain tests

\* \[ ] Value Object tests

\* \[ ] Command handler tests

\* \[ ] Validator tests

\* \[ ] Business rule tests



\## Integration Tests



\* \[ ] PostgreSQL integration tests

\* \[ ] Repository tests

\* \[ ] API integration tests

\* \[ ] Testcontainers

\* \[ ] RabbitMQ integration tests

\* \[ ] Redis integration tests



\## Quality



\* \[ ] Code coverage

\* \[ ] Static analysis

\* \[ ] Architecture tests

\* \[ ] Performance tests



\---



\# Phase 6 — Docker \& Containerization



\* \[x] PostgreSQL Docker container

\* \[ ] API Dockerfile

\* \[ ] Multi-stage Docker build

\* \[ ] Run API with Docker

\* \[ ] Docker Compose development environment

\* \[ ] Environment-specific configuration

\* \[ ] Container health checks

\* \[ ] Non-root containers

\* \[ ] Image optimization



\### Target architecture



```text

Docker Compose



├── API

├── PostgreSQL

├── Redis

└── RabbitMQ

```



\---



\# Phase 7 — CI/CD



The CI/CD pipeline will be built incrementally.



\## Continuous Integration



\* \[ ] GitHub repository

\* \[ ] Branch strategy

\* \[ ] Pull Requests

\* \[ ] GitHub Actions

\* \[ ] Restore dependencies

\* \[ ] Build solution

\* \[ ] Run unit tests

\* \[ ] Run integration tests

\* \[ ] Generate test reports

\* \[ ] Code coverage

\* \[ ] Docker image build

\* \[ ] Docker image validation



\### Target



```text

Pull Request

&#x20;    │

&#x20;    ▼

GitHub Actions

&#x20;    │

&#x20;    ├── Restore

&#x20;    ├── Build

&#x20;    ├── Test

&#x20;    ├── Coverage

&#x20;    └── Docker Build

&#x20;    │

&#x20;    ▼

&#x20;  PASS

```



\## Continuous Deployment



Eventually:



```text

main

&#x20;│

&#x20;▼

GitHub Actions

&#x20;│

&#x20;├── Build

&#x20;├── Test

&#x20;├── Build Docker Image

&#x20;├── Push Image

&#x20;│

&#x20;▼

Production VPS

&#x20;│

&#x20;├── Pull Image

&#x20;├── Run migrations

&#x20;├── Restart containers

&#x20;└── Health Check

```



Planned:



\* \[ ] Production environment

\* \[ ] GitHub Environments

\* \[ ] GitHub Secrets

\* \[ ] Container Registry

\* \[ ] SSH deployment

\* \[ ] VPS deployment

\* \[ ] Zero/minimal downtime deployment

\* \[ ] Rollback strategy



\---



\# Phase 8 — Observability



\* \[ ] Structured logging

\* \[ ] Correlation IDs

\* \[ ] Request tracing

\* \[ ] Metrics

\* \[ ] Health checks

\* \[ ] Readiness checks

\* \[ ] Liveness checks

\* \[ ] OpenTelemetry

\* \[ ] Distributed tracing

\* \[ ] Error monitoring



\---



\# Git Workflow



We will use Git as part of the learning process rather than only as a place to store the code.



Example branches:



```text

main

&#x20;│

&#x20;├── feature/create-product

&#x20;├── feature/authentication

&#x20;├── feature/ai-product-description

&#x20;├── feature/vector-search

&#x20;└── fix/product-validation

```



Pull Requests will eventually trigger CI automatically.



\---



\# Commit Convention



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



\---



\# Engineering Principles



Throughout the project we will intentionally practice:



\* SOLID

\* Separation of Concerns

\* Dependency Inversion

\* Clean Architecture

\* Domain-driven design concepts

\* CQRS

\* Idempotency

\* Resilience

\* Observability

\* Security

\* Performance

\* Testability

\* Automation



Patterns will not be introduced just because they exist.



For every architectural pattern we will ask:



> What problem does this solve?



and:



> When should we NOT use it?



\---



\# Interview Preparation



This project is also an interview preparation environment.



Topics that will be covered include:



\## .NET



\* Dependency Injection

\* Middleware

\* Filters

\* Configuration

\* Options Pattern

\* Hosted Services

\* CancellationToken

\* Async/Await

\* Memory management

\* Performance



\## Architecture



\* Clean Architecture

\* CQRS

\* DDD concepts

\* Repository Pattern

\* Unit of Work

\* Modular Monolith

\* Microservices

\* Event-driven architecture



\## Databases



\* PostgreSQL

\* Indexes

\* Transactions

\* Isolation levels

\* Query optimization

\* Concurrency

\* EF Core internals



\## Distributed Systems



\* Redis

\* RabbitMQ

\* Outbox Pattern

\* Idempotency

\* Retry

\* Circuit Breaker

\* Eventual consistency



\## DevOps



\* Docker

\* CI/CD

\* GitHub Actions

\* Linux

\* Nginx

\* HTTPS

\* VPS deployment

\* Secrets

\* Monitoring



\## AI Engineering



\* LLM APIs

\* Prompt engineering

\* Structured output

\* Tool calling

\* Embeddings

\* Vector databases

\* RAG

\* AI reliability

\* AI cost optimization



\---



\# Current Status



\*\*Current phase:\*\* Foundation



\*\*Current focus:\*\* Product creation flow



```text

API

&#x20;↓

MediatR

&#x20;↓

Validation Pipeline

&#x20;↓

Handler

&#x20;↓

Repository

&#x20;↓

EF Core

&#x20;↓

PostgreSQL

```



\---



\# Learning Philosophy



CommerceAI is intentionally developed incrementally.



We will not build every feature at once.



Each feature should introduce one or more real engineering concepts, and whenever possible the implementation will be tested, containerized, automated, and eventually deployed.



The goal is not just to make the application work.



The goal is to understand \*\*why it works, how it can fail, and how to operate it in production.\*\*



