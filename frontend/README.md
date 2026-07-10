## Rezerv WhatsApp Integration - Frontend

This is the React frontend for the Rezerv WhatsApp Integration assessment.
The frontend provides an operator dashboard for managing WhatsApp connections per business location, sending test messages, and viewing message delivery lifecycle logs.

---

### Tech Stack

- React
- TypeScript
- Vite
- Tailwind CSS
- shadcn/ui
- React Query
- React Router
- Axios
- Sonner

---

### Features

#### Connection Dashboard

- Lists all location WhatsApp connections
- Displays current connection state with colored badges
- Shows summary cards for:
  - Total
  - Active
  - Stale
  - Expired
  - Disconnected
- Supports filtering connections by state
- Allows operator to disconnect or reconnect a location directly from the dashboard
- Shows last activity using both formatted timestamp and relative time, for example `9 days ago`

#### Connection Detail Page

- Shows full connection information
- Displays connection state, phone number, connected date, last activity, and updated date
- Shows human-readable staleness indicators
- Allows reconnect and disconnect actions
- Shows warning messages for stale, expired, and disconnected connections

#### Send Message Form

- Allows sending a test WhatsApp message from a selected location connection
- Accepts recipient/customer phone number
- Accepts message content
- Disables sending when the connection is expired or disconnected
- Shows success or failure feedback using toast notifications
- Displays backend validation and business rule errors to the user

#### Message Logs

- Shows message history for each location
- Supports filtering message logs by status
- Displays message lifecycle status:
  - Queued
  - Sent
  - Delivered
  - Read
  - Failed
- Displays lifecycle timestamps:
  - Created
  - Sent
  - Delivered
  - Read
  - Failed
- Auto-refreshes message logs so webhook status updates become visible in the UI

---

### Frontend Architecture

- **api/** - only talks to the backend
- **hooks/** - owns React Query logic
- **pages/** - compose page layout
- **components/** - reusable UI blocks
- **types/** - frontend contracts
- **lib/** - shared utilities

---

### Project Structure Idea

The frontend is organized to keep UI rendering, API access, server-state logic, and shared utilities separated.

```txt
src/
  api/
    HTTP client and backend API functions

  components/
    Reusable UI components

  components/ui/
    shadcn/ui generated components

  hooks/
    React Query hooks for queries and mutations

  pages/
    Route-level page components

  types/
    Shared TypeScript types

  lib/
    Shared helpers such as date formatting and API error parsing

```

---

### Environment Variable

- VITE_API_BASE_URL=http://localhost:8080

---

### How To Run

#### Locally

```bash
npm install
npm run dev
```

#### Via Docker compose

```bash
docker compose up --build
```

---

### Main User Flow

- View all connections
- Create new connection
- Disconnect/reconnect connection
- Open connection detail
- Send message
- View message logs
- Filter logs by status
- Watch webhook updates appear after refresh/polling

---

### Validation And Error Handling

- Frontend performs basic required-field validation.
- Backend validation and business rule errors are displayed using toast notifications.
- Expired/disconnected connections disable message sending.

---

### Assumptions And Notes

- Message logs auto-refresh to show webhook updates.
- UI is intentionally simple and assessment-focused.

---
