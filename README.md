# SmartWater API

Backend API for managing water infrastructure — pressure monitoring, hydrant digitalization, and automated billing.

Built for the Muralis technical hiring challenge using .NET 10 and Clean Architecture.

---

## Technologies

| Layer | Technology |
|---|---|
| Runtime | .NET 10, ASP.NET Core 10 |
| Database | SQL Server 2022 (Docker) |
| ORM | Entity Framework Core 10 |
| Auth | JWT Bearer (System.IdentityModel.Tokens.Jwt) |
| Validation | FluentValidation 11 |
| API Docs | Swagger / Swashbuckle |
| Password | BCrypt.Net-Next (work factor 12) |
| Tests | xUnit + FluentAssertions |

---

## Architecture

```
SmartWater/
├── SmartWater.API          # Controllers, DTOs, Validators, Middleware
├── SmartWater.Application  # Services, Interfaces, Models, Settings
├── SmartWater.Domain       # Entities with business rules (rich domain)
├── SmartWater.Infrastructure  # EF Core, Repositories, Migrations
└── SmartWater.Tests        # Unit tests (xUnit + FluentAssertions)
```

**Why this architecture?**
Clean Architecture separates business rules from infrastructure without the overhead of CQRS or MediatR. Each layer depends only on what it needs. SOLID is applied naturally: single responsibility per class, interfaces at every boundary, dependency injection throughout.

---

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

---

## Setup and Run

### 1. Start SQL Server

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SmartWater@2024" \
  -p 1433:1433 --name smartwater-sql \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Apply migrations

```bash
dotnet ef database update \
  --project SmartWater.Infrastructure \
  --startup-project SmartWater.API
```

### 3. Run the API

```bash
dotnet run --project SmartWater.API
```

Swagger UI: `http://localhost:{port}/swagger`

---

## How to Test with Swagger

1. Open Swagger at `/swagger`
2. Register a user: `POST /api/auth/register`
3. Login: `POST /api/auth/login` — copy the token from the response
4. Click **Authorize** (top right), enter `Bearer {your_token}`
5. All protected endpoints are now available

---

## API Endpoints

### Auth
| Method | Route | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login and receive JWT token |

### Pressure Monitoring
| Method | Route | Description |
|---|---|---|
| POST | `/api/pressure` | Add pressure reading (auto-generates alert if variation >= 5 mca) |
| GET | `/api/pressure/history/{sector}` | Get history for a sector |
| GET | `/api/pressure/dashboard` | Today's summary: readings, alerts, average pressure |

### Hydrants
| Method | Route | Description |
|---|---|---|
| POST | `/api/hydrants` | Create hydrant |
| GET | `/api/hydrants` | List all hydrants |
| GET | `/api/hydrants/{id}` | Get hydrant by id |
| POST | `/api/hydrants/{id}/inspections` | Register inspection |
| GET | `/api/hydrants/{id}/inspections` | List inspections for a hydrant |

### Billing
| Method | Route | Description |
|---|---|---|
| POST | `/api/billing` | Create invoice |
| GET | `/api/billing` | List all invoices |
| GET | `/api/billing/pending` | List pending invoices |
| GET | `/api/billing/{id}` | Get invoice by id |
| POST | `/api/billing/{id}/process` | Process invoice (Pending → Processed) |
| POST | `/api/billing/{id}/send` | Mark as sent (Processed → Sent) |

---

## Example Requests

### Register
```json
POST /api/auth/register
{
  "username": "operator1",
  "password": "SecurePass1"
}
```

### Add Pressure Reading
```json
POST /api/pressure
Authorization: Bearer {token}
{
  "sector": "Zone-A",
  "value": 42.5
}
```
If the previous reading in `Zone-A` was `37.0`, a variation of `5.5 >= 5` triggers an alert automatically.

### Create Invoice
```json
POST /api/billing
Authorization: Bearer {token}
{
  "customer": "Residence Block 7",
  "amount": 89.90
}
```

### Process Invoice
```json
POST /api/billing/1/process
Authorization: Bearer {token}
```
Response includes `processingStartedAt` and `processedAt` timestamps.

---

## Business Rules

### Pressure Monitoring (Problem C)
- Every pressure reading is persisted with its sector and timestamp
- Valid operational range: **15 mca to 30 mca**
- If the variation from the previous reading in the same sector is **>= 5 mca AND within the valid range**, an `Alert` is created automatically
- Readings outside the valid range are considered invalid and do not trigger variation alerts
- The dashboard shows: total readings today, total alerts today, last critical sector, average pressure today

### Hydrant Module (Problem A)
- Each hydrant has a unique code, location, and status (`Active`, `Inactive`, `UnderMaintenance`)
- Inspections record pressure, flow rate, inspector name, and optional notes
- Pressure in inspections must be > 0; flow rate must be >= 0

### Billing Module (Problem B)
- Invoice lifecycle: `Pending → Processing → Processed → Sent`
- `Error` status is available for failed processing
- `MarkAsSent()` is only allowed after the invoice reaches `Processed`
- All status transitions are enforced in the domain entity — no business logic in the controller

---

## Running Tests

```bash
dotnet test SmartWater.Tests
```

---

## Deployment Checklist

- [ ] Docker SQL Server running on port 1433
- [ ] `appsettings.json` connection string configured
- [ ] `Jwt:Key` set (minimum 32 characters)
- [ ] Migration applied: `dotnet ef database update ...`
- [ ] API running: Swagger accessible at `/swagger`
- [ ] Register a user and confirm JWT authorization works in Swagger

---

## How to Explain This Project in an Interview

### Pressure Monitoring
> "Every sector reading is compared to the last one. If the variation is 5 mca or more, the system creates an alert automatically — no manual intervention needed. The dashboard aggregates today's data: total readings, alerts, and average pressure per sector."

### Preventive Maintenance Value
> "Instead of waiting for a failure, operators can see pressure trends per sector and respond before a pipe bursts. The alert system gives real-time visibility without building a complex event pipeline."

### Hydrant Digitalization
> "Paper inspection sheets are replaced by an API. Each inspection has a timestamp, the inspector's name, pressure and flow rate. Historical data becomes available for audits and analysis immediately."

### Billing Workflow
> "The invoice lifecycle (`Pending → Processing → Processed → Sent`) models the real business process the company already has. The API makes each step explicit and auditable, with timestamps for each transition. The pending list endpoint solves the bottleneck of finding which invoices need action."

### Architecture Decision
> "I chose Clean Architecture without CQRS or MediatR because the system is CRUD-oriented. Adding those patterns would increase complexity without adding value at this scale. The layers are thin enough to understand in a code review but structured enough to scale if needed."
