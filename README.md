# WorkSphere – Enterprise Workforce Management Platform

WorkSphere is a .NET-based workforce management platform built using a microservices architecture.

The application demonstrates employee and department management, JWT authentication, API Gateway routing, Redis caching, RabbitMQ event messaging, centralized exception handling, and containerized deployment using Docker Compose.

---

## 🚀 Features

- Employee Management
- Department Management
- JWT Authentication
- API Gateway using YARP
- Redis Caching
- RabbitMQ Event Messaging
- Notification Service
- SQL Server with Entity Framework Core
- Docker & Docker Compose
- HTML, CSS & JavaScript Frontend
- Centralized Exception Handling
- REST-based Inter-Service Communication
- Microservices Architecture

---

## 🏛️ System Architecture

WorkSphere follows a microservices architecture where individual services are independently containerized and communicate through REST APIs and RabbitMQ messaging.

```mermaid
flowchart TD

    Client[User / Browser]

    Frontend[WorkSphere Frontend<br/>HTML CSS JavaScript]

    Gateway[API Gateway<br/>YARP]

    Auth[Auth Service<br/>JWT Authentication]

    Employee[Employee Service<br/>ASP.NET Core Web API]

    Department[Department Service<br/>ASP.NET Core Web API]

    Notification[Notification Service]

    SQL[(SQL Server)]

    Redis[(Redis Cache)]

    RabbitMQ[(RabbitMQ)]

    Client --> Frontend

    Frontend --> Gateway
    Frontend --> Auth

    Gateway --> Employee
    Gateway --> Department

    Employee --> SQL
    Department --> SQL

    Employee --> Redis

    Employee --> RabbitMQ
    RabbitMQ --> Notification

    Employee -->|REST API| Department
