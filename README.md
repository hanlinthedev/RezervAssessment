# Rezerv WhatsApp Integration Assessment

## Documentations

- [Backend README](https://github.com/hanlinthedev/RezervAssessment/blob/main/backend/README.md)
- [Frontend README](https://github.com/hanlinthedev/RezervAssessment/blob/main/frontend/README.md)

---

## Project Overview

This project is a full-stack implementation of the Rezerv Engineering Assessment for a mock WhatsApp integration.

The system simulates how a SaaS booking platform for fitness and wellness studios could manage WhatsApp connections for multiple physical business locations. Each location has its own independent WhatsApp connection, which can be active, stale, expired, or disconnected based on business rules.

The backend is built with .NET and MongoDB. It manages location connections, sends test messages through a mocked Meta WhatsApp client, stores message logs, processes Meta webhook delivery status updates, validates webhook signatures, and runs a background job to update connection health.

The frontend is built with React and TypeScript. It provides an operator dashboard to view connection states, create new connections, disconnect or reconnect locations, send test messages, and view message delivery lifecycle logs.

The project includes Docker Compose support so the backend, frontend, and MongoDB database can be started together with a single command.

### Tech Stack

#### Backend

- **.NET 10** - Backend runtime and application framework
- **ASP.NET Core Web API** - REST API and webhook endpoints
- **MongoDB** - Database for location connections and message logs
- **MongoDB C# Driver** - MongoDB data access
- **Swagger / OpenAPI** - API documentation and testing
- **Hosted Background Service** - Periodic connection health monitoring

#### Frontend

- **React** - Frontend UI framework
- **TypeScript** - Type-safe frontend development
- **Vite** - Frontend build tool and development server
- **Tailwind CSS** - Utility-first styling
- **shadcn/ui** - Reusable UI components
- **React Query** - Server-state management, caching, and mutations
- **React Router** - Client-side routing
- **Axios** - HTTP client
- **Sonner** - Toast notifications

#### Infrastructure

- **Docker Compose** - Runs backend, frontend, and MongoDB together
- **Docker** - Containerized backend and frontend builds
- **Nginx** - Serves the production frontend build

---

### Features Summary

#### Location Connection Management

- Create a WhatsApp connection for a physical business location
- Enforce one connection per location
- View all location connections from the dashboard
- View full connection details by location
- Disconnect and reconnect location connections
- Track connection state:
  - Active
  - Stale
  - Expired
  - Disconnected

#### Connection Health Monitoring

- Tracks `lastActivityAt` for each WhatsApp connection
- Marks connections as `Stale` when WhatsApp activity is older than 8 days
- Marks connections as `Expired` when WhatsApp activity is older than 12 days
- Runs a backend background service to refresh connection health automatically
- Shows human-readable last activity information in the frontend

#### Message Sending

- Sends test WhatsApp messages from a selected location connection to a customer or recipient phone number
- Allows sending from active and stale connections
- Blocks sending from expired and disconnected connections
- Creates message logs for each send attempt
- Uses a mocked Meta WhatsApp client
- Simulates Meta API failure on every 5th send attempt

#### Message Logs and Lifecycle

- Stores message delivery history per location
- Supports message statuses:
  - Queued
  - Sent
  - Delivered
  - Read
  - Failed
- Prevents invalid message status regression
- Supports filtering message logs by status
- Displays lifecycle timestamps in the frontend:
  - Created
  - Sent
  - Delivered
  - Read
  - Failed

#### Meta Webhook Handling

- Supports Meta webhook verification using `hub.verify_token`
- Validates webhook POST requests using `X-Hub-Signature-256`
- Rejects invalid webhook signatures with `403`
- Processes delivery status updates from Meta webhook payloads
- Handles duplicate webhook events idempotently
- Returns `200 OK` for valid webhook requests even if internal processing fails

#### Frontend Dashboard

- Operator dashboard for viewing all WhatsApp location connections
- Colored status badges for connection states
- Summary cards for connection counts
- State filtering
- Create connection form
- Disconnect and reconnect actions
- Connection detail page
- Send message form
- Message log table with auto-refresh
- Toast feedback for success, failure, validation errors, and business rule errors

---

### Architecture Overview

The project is split into three main parts:

```txt
Frontend React App
        |
        | HTTP requests
        v
ASP.NET Core Backend API
        |
        | MongoDB Driver
        v
MongoDB Database
```

Backend contains the core business logic,webhook handling, message lifecycle processing, and background connection monitoring.
The frontend provides the operator dashboard, connection management screens, send message form, and message log views.
Mongo DB stores location connections and message logs.

For Detailed implementation, see:

- [Backend README](https://github.com/hanlinthedev/RezervAssessment/blob/main/backend/README.md)
- [Frontend README](https://github.com/hanlinthedev/RezervAssessment/blob/main/frontend/README.md)

### Repository Structure

```txt
.
├── backend/
│   ├── Rezerv.WhatsApp.Api/
│   ├── Rezerv.WhatsApp.Application/
│   ├── Rezerv.WhatsApp.Domain/
│   ├── Rezerv.WhatsApp.Infrastructure/
│   ├── Dockerfile
│   └── README.md
│
├── frontend/
│   ├── src/
│   ├── Dockerfile
│   ├── nginx.conf
│   ├── package.json
│   └── README.md
│
├── docker-compose.yml
├── .gitignore
└── README.md
```

#### Main Directories

- **backend** - Contains the .NET backend project with API, application logic, domain models, and infrastructure code.
- **frontend** - Contains the React frontend project with UI components, pages, and API integration
- **docker-compose.yml** - Defines the services for backend, frontend, and MongoDB to run together in Docker containers.

---

### How to Run with Docker Compose

#### Prerequisites

- Docker
- Docker Compose

#### Start the Application

```bash
docker compose up --build
```

#### Access the Application

- Frontend: [http://localhost:5173](http://localhost:5173)
- Backend Swagger API: [http://localhost:8080/swagger](http://localhost:8080/swagger)
- MongoDB Compass: [mongodb://rezerv:rezerv_password@localhost:27017/rezerv_whatsapp?authSource=admin](mongodb://rezerv:rezerv_password@localhost:27017/rezerv_whatsapp?authSource=admin)

#### Stop the Application

```bash
docker compose down
```

#### Reset the Database And Seed Data

```bash
docker compose down -v
docker compose up --build
```

---

### API Endpoints

| Method   | Endpoint                                  | Description             |
| -------- | ----------------------------------------- | ----------------------- |
| `GET`    | `/api/connections`                        | List all connections    |
| `POST`   | `/api/connections`                        | Create a connection     |
| `GET`    | `/api/connections/{locationId}`           | Get connection details  |
| `DELETE` | `/api/connections/{locationId}`           | Disconnect a connection |
| `POST`   | `/api/connections/{locationId}/reconnect` | Reconnect a connection  |
| `POST`   | `/api/messages/send`                      | Send a test message     |
| `GET`    | `/api/messages/{locationId}`              | Get message logs        |
| `GET`    | `/webhook/meta`                           | Webhook verification    |
| `POST`   | `/webhook/meta`                           | Webhook status updates  |

All backend API endpoints are documented in [here](https://github.com/hanlinthedev/RezervAssessment/blob/main/backend/README.md#swagger-api)

---

### Demo / Testing Flows

After starting the project with Docker Compose, the following flows can be tested from the frontend and Swagger UI.

#### 1. View Seeded Connections

Open the frontend:

```txt
http://localhost:5173
```

The dashboard shows seeded WhatsApp location connections with different states:

- Active
- Stale
- Expired
- Disconnected

The dashboard also shows summary cards, colored state badges, last activity information, and disconnect/reconnect actions.

#### 2. Create a New Connection

From the frontend dashboard, click **New Connection** and create a new location connection.

Example:

```txt
Location ID: loc-demo-001
Phone Number: +959123456789
Display Name: Demo Fitness Studio
```

Expected result:

- A new connection is created with the `Active` state
- The dashboard updates after creation
- Creating another connection with the same `locationId` is rejected
- Backend validation and business rule errors are displayed in the frontend

#### 3. Send a Test Message

Open an active or stale connection detail page and send a message.

Example:

```txt
Recipient Phone: +959999999999
Message Content: Hello! Your booking has been confirmed.
```

Expected result:

- A message log is created
- Successful mock Meta API calls update the message to `Sent`
- Every 5th send attempt fails and creates a `Failed` message log
- The frontend displays success or failure feedback using toast notifications

#### 4. Send Gating for Expired and Disconnected Connections

Open an expired or disconnected connection detail page.

Expected result:

- The send message form is disabled in the frontend
- Message sending is blocked by the backend
- The UI displays a warning based on the connection state
- Expired and disconnected connections cannot send messages

#### 5. Disconnect and Reconnect

From the dashboard or connection detail page:

- Disconnect an active, stale, or expired connection
- Reconnect a disconnected connection

Expected result:

- Disconnected connections cannot send messages
- Reconnected connections become `Active`
- Dashboard state badges update
- Summary card counts update
- Connection detail page reflects the latest state

#### 6. View Message Logs

Open a connection detail page.

Expected result:

- Message logs are shown in reverse chronological order
- Logs can be filtered by status
- Message lifecycle statuses are displayed:
  - Queued
  - Sent
  - Delivered
  - Read
  - Failed
- Lifecycle timestamps are displayed:
  - Created
  - Sent
  - Delivered
  - Read
  - Failed

#### 7. Test Webhook Verification

Use Swagger or curl to test the Meta webhook verification endpoint:

```txt
GET /webhook/meta
```

Example:

```bash
curl "http://localhost:8080/webhook/meta?hub.mode=subscribe&hub.verify_token=rezerv_verify_token&hub.challenge=test_challenge_123"
```

Expected result:

```txt
test_challenge_123
```

Invalid verify token should return `403`.

#### 8. Test Webhook Signature Validation and Status Updates

This section shows how to test the Meta webhook verification endpoint, webhook signature validation, and message status updates using `curl`.

The backend uses the following default values from Docker Compose:

```txt
Backend URL: http://localhost:8080
Verify Token: rezerv_verify_token
App Secret: rezerv_app_secret
```

---

##### 1. Test Webhook Verification

Meta verifies the webhook by calling the `GET /webhook/meta` endpoint with `hub.mode`, `hub.verify_token`, and `hub.challenge`.

```bash
curl "http://localhost:8080/webhook/meta?hub.mode=subscribe&hub.verify_token=rezerv_verify_token&hub.challenge=test_challenge_123"
```

Expected response:

```txt
test_challenge_123
```

Invalid verify token should return `403`:

```bash
curl -i "http://localhost:8080/webhook/meta?hub.mode=subscribe&hub.verify_token=wrong_token&hub.challenge=test_challenge_123"
```

Expected result:

```txt
HTTP/1.1 403 Forbidden
```

---

##### 2. Send a Message First

Before testing webhook delivery updates, send a message from an active connection.

You can do this from Swagger, the frontend, or curl:

```bash
curl -X POST "http://localhost:8080/api/messages/send" \
  -H "Content-Type: application/json" \
  -d '{
    "locationId": "loc-active-001",
    "recipientPhone": "+959999999999",
    "messageContent": "Hello! Your booking has been confirmed."
  }'
```

Copy the returned `metaMessageId` from the response. It should look similar to:

```txt
wamid.mock.xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```

Use that value in the webhook payload examples below.

---

##### 3. Test Webhook POST with a Valid Signature

Replace `PASTE_META_MESSAGE_ID_HERE` with the `metaMessageId` from the send message response.

This example sends a `delivered` status update.

```bash
TIMESTAMP=$(date +%s)

BODY='{"object":"whatsapp_business_account","entry":[{"changes":[{"value":{"statuses":[{"id":"PASTE_META_MESSAGE_ID_HERE","status":"delivered","timestamp":"'"$TIMESTAMP"'","recipient_id":"959999999999"}]}}]}]}'

SIGNATURE=$(printf '%s' "$BODY" | openssl dgst -sha256 -hmac "rezerv_app_secret" -hex | sed 's/^.* //')

curl -i -X POST "http://localhost:8080/webhook/meta" \
  -H "Content-Type: application/json" \
  -H "X-Hub-Signature-256: sha256=$SIGNATURE" \
  -d "$BODY"
```

Expected result:

```txt
HTTP/1.1 200 OK
```

After this, check the message logs from the frontend or API. The message status should be updated to `Delivered`, and `deliveredAt` should be set.

---

##### 4. Test Read Status Update

Use the same `metaMessageId` and send a `read` status update.

```bash
TIMESTAMP=$(date +%s)

BODY='{"object":"whatsapp_business_account","entry":[{"changes":[{"value":{"statuses":[{"id":"PASTE_META_MESSAGE_ID_HERE","status":"read","timestamp":"'"$TIMESTAMP"'","recipient_id":"959999999999"}]}}]}]}'

SIGNATURE=$(printf '%s' "$BODY" | openssl dgst -sha256 -hmac "rezerv_app_secret" -hex | sed 's/^.* //')

curl -i -X POST "http://localhost:8080/webhook/meta" \
  -H "Content-Type: application/json" \
  -H "X-Hub-Signature-256: sha256=$SIGNATURE" \
  -d "$BODY"
```

Expected result:

```txt
HTTP/1.1 200 OK
```

The message status should become `Read`, and `readAt` should be set.

---

##### 5. Test Invalid Signature

This should be rejected with `403`.

```bash
BODY='{"object":"whatsapp_business_account","entry":[{"changes":[{"value":{"statuses":[{"id":"PASTE_META_MESSAGE_ID_HERE","status":"delivered","timestamp":"1710000000","recipient_id":"959999999999"}]}}]}]}'

curl -i -X POST "http://localhost:8080/webhook/meta" \
  -H "Content-Type: application/json" \
  -H "X-Hub-Signature-256: sha256=invalid_signature" \
  -d "$BODY"
```

Expected result:

```txt
HTTP/1.1 403 Forbidden
```

---

##### 6. Test Duplicate Webhook Event

Run the same valid webhook request twice using the same payload and signature.

Expected result:

- Both requests return `200 OK`
- The message is not updated incorrectly
- No duplicate side effects occur

This confirms the webhook handling is idempotent.

---

##### 7. Test Status Regression Prevention

After a message becomes `Read`, send an older status such as `delivered` again.

```bash
TIMESTAMP=$(date +%s)

BODY='{"object":"whatsapp_business_account","entry":[{"changes":[{"value":{"statuses":[{"id":"PASTE_META_MESSAGE_ID_HERE","status":"delivered","timestamp":"'"$TIMESTAMP"'","recipient_id":"959999999999"}]}}]}]}'

SIGNATURE=$(printf '%s' "$BODY" | openssl dgst -sha256 -hmac "rezerv_app_secret" -hex | sed 's/^.* //')

curl -i -X POST "http://localhost:8080/webhook/meta" \
  -H "Content-Type: application/json" \
  -H "X-Hub-Signature-256: sha256=$SIGNATURE" \
  -d "$BODY"
```

Expected result:

- The request returns `200 OK`
- The message remains `Read`
- The status does not regress back to `Delivered`

---

##### 8. Test Unknown Meta Message ID

Use a fake `metaMessageId` that does not exist in the database.

```bash
TIMESTAMP=$(date +%s)

BODY='{"object":"whatsapp_business_account","entry":[{"changes":[{"value":{"statuses":[{"id":"wamid.mock.unknown_message_id","status":"delivered","timestamp":"'"$TIMESTAMP"'","recipient_id":"959999999999"}]}}]}]}'

SIGNATURE=$(printf '%s' "$BODY" | openssl dgst -sha256 -hmac "rezerv_app_secret" -hex | sed 's/^.* //')

curl -i -X POST "http://localhost:8080/webhook/meta" \
  -H "Content-Type: application/json" \
  -H "X-Hub-Signature-256: sha256=$SIGNATURE" \
  -d "$BODY"
```

Expected result:

```txt
HTTP/1.1 200 OK
```

Unknown message IDs are ignored safely because Meta may send retries or updates for messages that are no longer available in the system.

Expected webhook behavior:

- Valid `X-Hub-Signature-256` is accepted
- Invalid signature returns `403`
- Webhook status updates modify the matching message log by `metaMessageId`
- Unknown `metaMessageId` values are ignored safely
- Duplicate webhook events are handled idempotently
- Status regression is ignored, for example:
  - `Read` does not go back to `Delivered`
  - `Failed` does not become `Delivered`

#### 9. Test Background Connection Health Monitoring

The backend includes seeded connections for stale and expired scenarios.

Expected result:

- Connections with activity older than 8 days become `Stale`
- Connections with activity older than 12 days become `Expired`
- Manually disconnected connections remain `Disconnected`
- The frontend displays both formatted timestamps and human-readable relative activity, such as `9 days ago`

---

### Assumptions and Trade-offs

#### Assumptions

- Each physical business location can have only one WhatsApp connection.
- `LocationConnection.phoneNumber` represents the connected WhatsApp sender number for that location.
- `MessageLog.recipientPhone` represents the customer or recipient receiving the message.
- Sending a message means sending from the selected location's connected WhatsApp number to a customer or recipient phone number.
- Reconnecting a disconnected connection is treated as a successful reactivation and resets the connection state to `Active`.
- Stale connections can still send messages, but the UI displays a warning.
- Expired and disconnected connections cannot send messages.
- All timestamps are stored in UTC. The frontend displays them in a human-readable format.
- Webhook delivery timestamps are based on the timestamp received from the Meta webhook payload.
- Unknown webhook `metaMessageId` values are ignored because Meta may send retries or updates for messages that are no longer available in the system.
- Duplicate webhook events are treated as idempotent and do not create duplicate side effects.

#### Trade-offs

- The Meta WhatsApp Cloud API is mocked instead of calling the real Meta API, so the project can run without external credentials.
- The mock Meta client fails every 5th send attempt to simulate external API failure.
- Webhook processing is handled inside the API request after signature validation. In a production system, the webhook endpoint could acknowledge quickly and publish the payload to a queue for asynchronous processing.
- The connection health background job runs inside the backend API process to keep the assessment simple. In production, this could be moved to a separate worker service.
- MongoDB bulk updates are used for connection health monitoring instead of loading every connection into application memory.
- The frontend performs basic form validation, while backend validation remains the source of truth for business rules and request validation.
- Message logs auto-refresh in the frontend to make webhook updates visible during testing. In production, this could be replaced with WebSockets or server-sent events.
- Unit tests are not included in this submission, but the domain rules, service boundaries, and repository abstractions are structured so tests can be added later.
- Credentials are hardcoded inside docker-compose.yml for testing purpose

---
