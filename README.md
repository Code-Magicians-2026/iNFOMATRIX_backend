# Infomatrix Backend: Quest4Kid
**Quest4Kid** is a comprehensive ecosystem for family learning and upbringing that transforms a child's everyday chores into an exciting RPG adventure through AI-driven arbitration.
## Concept & Idea
The core challenge in modern parenting is the friction that arises during the verification of household tasks. **Quest4Kid** solves this by introducing the **AI Quest Inspector**:
* **Child:** Gains a gaming experience, receives heroic feedback, and earns digital rewards.
* **Parents:** Benefit from automated monitoring and objective quality assessment without arguments.
* **AI (The Oracle):** Acts as an impartial judge, analyzing real photos of completed tasks.
---
## Architecture Overview
The project is built on **Clean Architecture** principles and is split into two key microservices to ensure scalability and isolation of AI logic.
### Solution Structure:
* **Infomatrix.Core** (The Heart):
    * `Domain`: Entities (Quest, Reward, Child, Parent), Value Objects, and business rules.
    * `Application`: CQRS (MediatR), FluentValidation, and quest orchestration logic.
    * `Infrastructure`: PostgreSQL (EF Core), Supabase Auth integration, and HttpClient for AI communication.
    * `Api`: REST Endpoints and HTTP composition.
* **Infomatrix.AI** (The Brain):
    * A dedicated microservice powered by **Semantic Kernel**.
    * Processes Vision requests (Azure OpenAI) to compare "Before/After" photos.
    * Implements fault tolerance (Exponential Backoff Retry) for stable LLM communication.
---
## Technology Stack
* **Runtime:** .NET 10 (C# 14)
* **Database:** PostgreSQL (Npgsql) + EF Core
* **Auth & Realtime:** Supabase
* **AI Engine:** Microsoft Semantic Kernel + Azure OpenAI (Vision)
* **Logging:** Serilog
* **Testing:** xUnit, WebApplicationFactory, Moq
---
## Key Data Flow (Quest Verification)
1. **Submission:** The child uploads a photo of the completed quest via `Core.Api`.
2. **Orchestration:** The Core service prepares the context (task description + photos) and calls `Infomatrix.AI`.
3. **Vision Analysis:** The AI service analyzes visual changes, verifies prompt compliance, and returns a structured JSON.
4. **Reward:** The Core service awards XP/coins, sends a push notification to the child, and a report to the parents.
---
## Local Setup
### Prerequisites
* .NET 10 SDK
* PostgreSQL instance
* Azure OpenAI Deployment
### Configuration
Configure `appsettings.json` in the respective services:
**Infomatrix.Core.Api:**
```json
{
  "Database": { "ConnectionString": "..." },
  "Supabase": { "Url": "...", "Key": "..." },
  "AiService": { "BaseAddress": "http://localhost:5053" }
}
```
**Infomatrix.AI:**
```json
{
  "AI": {
    "DeploymentName": "gpt-4o",
    "Endpoint": "https://your-resource.openai.azure.com/",
    "ApiKey": "YOUR_KEY"
  }
}
```
### Running
Start both services in separate terminals:
```bash
# AI Service
dotnet run --project src/Services/Infomatrix.AI/Infomatrix.AI/Infomatrix.AI.csproj
# Core API
dotnet run --project src/Services/Infomatrix.Core/Infomatrix.Core.Api/Infomatrix.Core.Api.csproj
```
---
## Testing
Run tests (including AI service integration tests and retry logic):
```bash
dotnet test src/Tests/Infomatrix.AI.Tests/Infomatrix.AI.Tests.csproj
```
