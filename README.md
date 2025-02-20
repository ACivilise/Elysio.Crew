# Elysio.Crew

A demonstration application showing how to use agents with Semantic Kernel, .NET Aspire, and Ollama with a locally running model.

## Description

The goal of this application is to show how to use agents with Semantic Kernel, Aspire and Ollama with a locally running model. It provides a practical example of integrating these technologies in a .NET application.

This is a demo application without authentication, allowing you to focus on understanding the core functionality.

## Prerequisites

- Docker Desktop installed and running
- .NET 9.0 SDK
- Node.js 22.14.0

## Getting Started

### 1. Set up PostgreSQL Password Secret

Before running the application, you need to set up the PostgreSQL password secret. Run this command in your terminal from the root of the project:

```bash
dotnet user-secrets set "postgresql-password" "your_password_here" --project "00-Orchestration/Elysio.Crew.AppHost/Elysio.Crew.AppHost.csproj"
```

Replace `your_password_here` with your desired PostgreSQL password.

### 2. Running the Application

1. Make sure Docker Desktop is running
2. Start the Aspire application:
   ```bash
   cd 00-Orchestration/Elysio.Crew.AppHost
   dotnet run
   ```

This will start all the required services including:
- The API backend
- The Next.js frontend
- PostgreSQL database
- Ollama LLM service
- Any other dependencies

The Aspire dashboard will automatically open in your browser, showing the status of all services.

### 3. Available Models

Once the application is running, you can see the available Ollama models through the Aspire dashboard. The default model used is "llama3.2", and it will be automatically downloaded when first starting the application.

To change the model:

1. Open `00-Orchestration/Elysio.Crew.AppHost/Program.cs`
2. Locate this line:
   ```csharp
   var ollama = builder.AddOllama(modelName: "llama3.2");
   ```
3. Change "llama3.2" to another supported model name
4. Restart the application to use the new model

The first time you run with a new model, it will be automatically downloaded through Docker.

### 4. Accessing the Application

After running `dotnet run`, the following endpoints will be available:

- Aspire Dashboard: Opens automatically in your browser
- Web Application: http://localhost:58300
- API: https://localhost:7056/
- PostgreSQL: Port 65534
- Ollama: Managed internally by Aspire

All services are orchestrated by Aspire and will be available automatically. You can monitor their health and access endpoints through the Aspire Dashboard.

## Architecture

The application is structured in layers:
- `00-Orchestration`: Contains the Aspire host and service defaults
- `10-Client`: Contains the Next.js frontend application
- `20-API`: Contains the backend API
- `30-Core`: Contains core business logic and helpers
- `40-Data`: Contains data access layer
- `50-Models`: Contains shared models and DTOs

## API Architecture

The backend is built using:
- .NET 9 Minimal API style for lightweight, focused endpoints
- MediatR for CQRS pattern implementation and request handling
- Semantic Kernel for AI agent integration
- Entity Framework Core for data access

## API Documentation

The API is documented using OpenAPI (Swagger) and enhanced with Scalar. When running in development mode, you can access:

1. Interactive API documentation at: `https://localhost:7056/scalar`
2. OpenAPI specification at: `https://localhost:7056/openapi/v1.json`

Scalar provides a modern, user-friendly interface for exploring and testing the API endpoints, with features like:
- Interactive request builder
- Authentication support
- Response visualization
- Code generation for multiple languages

## API Endpoints

The application provides several API endpoints for managing:
- Agents: Create and manage AI agents with different personalities and capabilities
- Rooms: Group agents and conversations in virtual spaces
- Conversations: Manage interactions between agents and users
- Messages: Handle individual messages in conversations

All endpoints are accessible without authentication for demonstration purposes and follow RESTful conventions.