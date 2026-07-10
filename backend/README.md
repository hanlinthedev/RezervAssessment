## Project Overview

This backend implement a mock WhatsApp Integration for SaaS booking platform used by fitness and wellness studio. In this system, each physical business location can connect one independent WhatsApp account through the Meta WhatsApp Cloud API. The backend manages location-level WhatsApp connections, sends messages through a mocked Meta client, stores message logs, processes Meta webhook status updates, and monitors connection health based on WhatsApp activity. The main goal of the project is to demonstrate backend design for a real-world integration scenario, including API design, MongoDB persistence, webhook security, idempotent message status handling, background processing, and Docker-based local setup.

---

## Tech

- **.NET 10** - Backend runtime and application framework
- **ASP.NET Core Web API** - REST API and webhook endpoints
- **MongoDB** - Document database for location connections and message logs
- **MongoDB C# Driver** - MongoDB data access
- **Docker Compose** - Local development environment for API and MongoDB
- **Swagger / OpenAPI** - API documentation and testing

---

## Feature Implemented

### WhatsApp Connection Management

- Create a WhatsApp connection for a location
- Enforce one WhatsApp connection per location
- Get a connection by location ID
- List all location connections
- Disconnect a connection
- Reconnect a disconnected connection
- Track connection states: `Active`, `Stale`, `Expired`, and `Disconnected`

### Message Sending

- Send messages through a mocked Meta WhatsApp client
- Create message logs with initial `Queued` status
- Update messages to `Sent` when the mock Meta client succeeds
- Update messages to `Failed` when the mock Meta client fails
- Reject message sending for `Expired` and `Disconnected` connections
- Allow message sending for `Active` and `Stale` connections
- Simulate Meta API failure on every 5th send request

### Message Logs

- Store message history for each location
- Retrieve message logs by location ID
- Filter message logs by status
- Return message logs in reverse chronological order

### Meta Webhook Integration

- Verify Meta webhook setup using `GET /webhook/meta`
- Receive Meta webhook status updates using `POST /webhook/meta`
- Validate webhook requests using `X-Hub-Signature-256`
- Reject invalid webhook signatures with `403 Forbidden`
- Process webhook status updates idempotently
- Prevent invalid message status regression, such as `Read` back to `Delivered`

### Background Connection Health Monitoring

- Run a background job to monitor connection health
- Automatically update connections to `Stale` when `lastActivityAt` is older than 8 days
- Automatically update connections to `Expired` when `lastActivityAt` is older than 12 days
- Keep manually `Disconnected` connections unchanged
- Use MongoDB bulk updates for better performance

---

## Architecture

The backend follows a clean architecture style with clear separation between API, application logic, domain rules, and infrastructure concerns.

```txt
Rezerv.WhatsApp.Api
  * ASP.NET Core controllers
  * HTTP request and response handling
  * Swagger configuration
  * Webhook raw body and signature handling

Rezerv.WhatsApp.Application
  * Application services
  * DTOs
  * Result pattern for expected business errors
  * Interfaces for repositories and external services

Rezerv.WhatsApp.Domain
  * Core entities
  * Enums
  * Message lifecycle

Rezerv.WhatsApp.Infrastructure
  * MongoDB repositories
  * MongoDB indexes
  * MongoDB bulk updates for connection health state changes
  * Mock Meta WhatsApp client
  * Webhook signature validator
  * Background health monitoring job
  * Seed data

Dependency Direction
  * API -> Application, Infrastructure
  * Application -> Domain
  * Infrastructure -> Application, Domain
  * Domain -> no deps
```

---

## Database Schema

The backend stores data in two main collections:
`location_connections` and `message_logs`

### LocationConnection

#### Schema

```json
{
	"_id": "string",
	"locationId": "string",
	"phoneNumber": "string",
	"displayName": "string",
	"connectionState": "Active | Stale | Expired | Disconnected",
	"lastActivityAt": "DateTime UTC",
	"connectedAt": "DateTime UTC",
	"updatedAt": "DateTime UTC"
}
```

#### Indexs

- **locationId** - unique index on locationId
- **connectionstate + lastActivityAt** - for background connection health monitoring

### MessageLog

```json
{
	"_id": "string",
	"locationId": "string",
	"recipientPhone": "string",
	"messageContent": "string",
	"metaMessageId": "string | null",
	"status": "Queued | Sent | Delivered | Read | Failed",
	"createdAt": "DateTime UTC",
	"sentAt": "DateTime UTC | null",
	"deliveredAt": "DateTime UTC | null",
	"readAt": "DateTime UTC | null",
	"failedAt": "DateTime UTC | null",
	"failureReason": "string | null"
}
```

#### Indexs

- **metaMessageId** - Index on metaMessageId
- **locationId + CreatedAt** - For GetMessageByLocationId and order by desc
- **locationId + status + CreatedAt** - For GetMessageByLocationId filter by status and order by desc

---

## Seed Data

The backend automatically seed sample data when the MongoDB database is empty. This make the API testable immediately after running Docker Compose.

Seeder Implementation inside `Rezerv.WhatsApp.Infrastructure/Seed/DatabaseSeeder.cs`

### Sample Data

#### `location_connections`

| Location ID       | Phone Number    | Display Name            | State     | Last Activity   |
| ----------------- | --------------- | ----------------------- | --------- | --------------- |
| `loc-active-001`  | `+959111111111` | `Yangon Active Studio`  | `Active`  | Recent activity |
| `loc-stale-001`   | `+959222222222` | `Mandalay Stale Studio` | `Stale`   | 9 days ago      |
| `loc-expired-001` | `+959333333333` | `Bago Expired Studio`   | `Expired` | 14 days ago     |

#### `message_logs`

| Location ID      | Recipient Phone | Message Content           | Status      |
| ---------------- | --------------- | ------------------------- | ----------- |
| `loc-active-001` | `+959900000001` | `Your booking is queued.` | `Queued`    |
| `loc-active-001` | `+959900000003` | `Your class starts soon.` | `Delivered` |
| `loc-active-001` | `+959900000004` | `Thanks for visiting.`    | `Read`      |

To reset the database and run the seed process again:

```bash
docker compose down -v
docker compose up --build
```

---

## How To Run

### Prerequisites

- Docker
- Docker compose

### Start

```bash
docker compose up --build
```

| Service | URL                             |
| ------- | ------------------------------- |
| API     | `http://localhost:8080`         |
| MongoDB | `localhost:27017`               |
| Swagger | `http://localhost:8080/swagger` |

### Stop

```bash
docker compose down
```

---

## Swagger API

Swagger UI is available at: `http://localhost:8080/swagger` after `docker compose up -d`.

### Available Endpoints

Complete Request and Response for each requests are available on **[Swagger UI](http://localhost:8080/swagger).**

#### Connections

| Method   | Endpoint                                  | Description                                 |
| -------- | ----------------------------------------- | ------------------------------------------- |
| `POST`   | `/api/connections`                        | Create a WhatsApp connection for a location |
| `GET`    | `/api/connections`                        | List all location connections               |
| `GET`    | `/api/connections/{locationId}`           | Get connection by location ID               |
| `DELETE` | `/api/connections/{locationId}`           | Disconnect a connection                     |
| `POST`   | `/api/connections/{locationId}/reconnect` | Reconnect a disconnected connection         |

#### Messages

| Method | Endpoint                                 | Description                         |
| ------ | ---------------------------------------- | ----------------------------------- |
| `POST` | `/api/messages/send`                     | Send a WhatsApp message             |
| `GET`  | `/api/messages/{locationId}`             | Get message logs for a location     |
| `GET`  | `/api/messages/{locationId}?status=Sent` | Get message logs filtered by status |

#### Meta Webhook

| Method | Endpoint        | Description                         |
| ------ | --------------- | ----------------------------------- |
| `GET`  | `/webhook/meta` | Verify webhook setup challenge      |
| `POST` | `/webhook/meta` | Receive Meta message status updates |

---

## Assumptions and Trade-offs

### Assumptions

- The backend stores all timestamps in UTC, and timezone conversion is expected to be handled by the frontend.
- Reconnecting a disconnected connection updates `lastActivityAt` and `connectedAt` because reconnect is treated as a successful reactivation of the WhatsApp connection.
- Webhook `deliveredAt`, `readAt`, and `failedAt` values are based on the timestamp from the Meta webhook payload instead of the server receive time.
- Unknown webhook message IDs are ignored instead of returning an error, because Meta may retry or send updates for messages that are no longer available in the system.
- Duplicate or out-of-order webhook status updates are ignored to keep message state changes idempotent.
- Webhook Processing is cancelled after 4 seconds to enforce response within 5 seconds.
- The background health monitor runs every 1 minute for easier local testing. In production, this interval would likely be longer.

### Trade-offs

- The Meta WhatsApp client is mocked instead of calling the real Meta Graph API, so the project can be tested without external credentials.
- Webhook processing is done inside the API request after signature validation. For a production system, the endpoint could acknowledge quickly and publish the payload to a queue for asynchronous processing.
- The background health monitor runs inside the API process to keep the assessment simple. In a larger system, it could be moved to a separate worker service.
- Connection health updates use MongoDB bulk update filters instead of loading every connection into application memory.
- MongoDB is accessed through repositories instead of exposing MongoDB queries directly to application services.
- Unit tests are not included in this submission, but lifecycle rules and service boundaries are structured so they can be tested later.
- Also Credentials are hardcoded inside `docker-compose.yml` for assessment purpose.
