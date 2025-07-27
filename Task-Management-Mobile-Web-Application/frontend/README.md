# TaskFlowManager.Frontend

This is the Vue.js frontend for the Task Management Assignment.

## Setup Instructions

### Prerequisites
- [Node.js (v16+ recommended)](https://nodejs.org/)
- npm (comes with Node.js)

### 1. Install Dependencies
```sh
cd frontend
npm install
```

### 2. Configure API URL
- If your backend is not on `localhost:7000`, update the API base URL in:
  - `src/services/api.ts`
  - `vite.config.js` (proxy section)

### 3. Run the App
```sh
npm run dev
```
The app will start on `http://localhost:5173` (or as shown in your terminal).

### 4. Test
- Open your browser and go to `http://localhost:5173`
- Try creating, editing, and deleting users and tasks.

---

## Notes
- If running on a different network or PC, update all relevant IP addresses.
- Use alerts for feedback; no extra notification libraries required.
