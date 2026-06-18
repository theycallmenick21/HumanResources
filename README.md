# 🏢 Human Resources Transactional System

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![MySQL](https://img.shields.io/badge/mysql-%2300f.svg?style=for-the-badge&logo=mysql&logoColor=white)
![Architecture](https://img.shields.io/badge/Architecture-DDD-brightgreen?style=for-the-badge)

A robust, client-server transactional application built with C# and .NET. This project simulates a complete Human Resources management system, utilizing TCP Sockets for real-time bidirectional communication and strictly adhering to Domain-Driven Design (DDD) principles.

Developed as a core project for the **Persistencia y Datos Transaccionales** (Persistence and Transactional Data) university course.

## 🚀 Features

* **TCP Socket Communication:** Custom built `SocketServer` and `SocketClient` allowing multiple concurrent connections via structured message passing.
* **Domain-Driven Design (DDD):** Highly decoupled architecture separating Domain, Application, Infrastructure, and Presentation layers.
* **Dependency Injection:** Centralized service container managing `Scoped` database contexts and repository lifecycles for concurrent requests.
* **Entity Framework Core (Code-First):** Automated database migrations and object-relational mapping using Fluent API for clean domain entities.
* **Data Transfer Objects (DTOs):** Secure and optimized data payload exchanges formatted in JSON between the client and server.

## 🏗️ Architecture Layers

This repository is structured into distinct projects to enforce separation of concerns:

1. **`HumanResources.Domain`**: The core of the system. Contains pure C# Entities (Employee, Department, City, etc.) and Repository Interfaces. Agnostic to any database technology.
2. **`HumanResources.Application`**: Business logic layer. Contains Services that coordinate data flow using injected interfaces.
3. **`HumanResources.Infrastructure`**: Database interaction layer. Implements generic and specific repositories using EF Core, Fluent API configurations, and the MySQL database context.
4. **`HumanResources.Shared`**: Contains Data Transfer Objects (DTOs) ensuring both client and server understand the same communication payload structures.
5. **`HumanResources.SocketServer`**: The composition root. Configures dependency injection, listens for incoming TCP connections, and processes JSON payloads.
6. **`HumanResources.SocketClient`**: The interactive console UI sending requests to the server and displaying serialized responses.

## 🛠️ Technologies & Tools

* **Language:** C#
* **Framework:** .NET (Console Application)
* **ORM:** Entity Framework Core (`Pomelo.EntityFrameworkCore.MySql`)
* **Database:** MySQL
* **Serialization:** `System.Text.Json`

## ⚙️ Setup and Installation

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download) (or your current version)
* A running instance of MySQL.

### Configuration
1. Clone this repository:
   ```bash
   git clone https://github.com/theycallmenick21/HumanResources.git
2. Navigate to the `Infrastructure` project and configure your connection string safely (e.g., via Environment Variables or appsettings.json, avoid hardcoding credentials in production).
3. Apply database migrations to generate your MySQL schema:
   ```bash
    dotnet ef database update --project HumanResources.Infrastructure --startup-project HumanResources.SocketServer

### Running the System
1. Open a terminal and start the Server:
   ```bash
    cd HumanResources.SocketServer
    dotnet run
2. Open a separate terminal and start the Client:
   ```bash
    cd HumanResources.SocketClient
    dotnet run
3. Use the console menu in the client terminal to interact with the database in real-time.
