# WorkSphere – Enterprise Workforce Management Platform

WorkSphere is a .NET-based workforce management platform built using a microservices architecture.

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

## 🏗️ Architecture

```text
Frontend
   ↓
API Gateway
   ↓
┌───────────────────────┐
│ Employee Service      │
│ Department Service    │
│ Auth Service          │
└───────────────────────┘
   ↓
SQL Server

Employee Service → Redis
Employee Service → RabbitMQ → Notification Service

🛠️ Technologies

Backend: C#, .NET, ASP.NET Core Web API, Entity Framework Core

Database: SQL Server

Messaging: RabbitMQ

Caching: Redis

Authentication: JWT

Gateway: YARP

Frontend: HTML, CSS, JavaScript

Containerization: Docker, Docker Compose

Version Control: Git, GitHub

🐳 Run with Docker
docker compose up --build
👨‍💻 Developer

Afan Dalvi
B.E. Computer Engineering – 2026
Pune, India



