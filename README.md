# HUSTask

A simple yet powerful web application for personal and team task management, enabling individuals and project leaders to stay organized and monitor task progress effectively.

## Project Description

HUSTask is a task management web application designed to cater to both individual users and teams. It allows individuals to manage their personal tasks efficiently while providing project leaders with tools to assign tasks to team members, track progress, and ensure deadlines are met. 

Built with **Vue.js** for the frontend, **ASP.NET Core** for the backend, and **PostgreSQL** as the database, the application focuses on **usability, scalability, and collaboration**.

---

## Key Features

### 1. Personal Task Management
- Users can create, update, and delete their own tasks.
- Prioritize tasks using statuses (e.g., **To-Do, In Progress, Done**).
- Set due dates and receive reminders for upcoming deadlines.

### 2. Team Task Assignment
- **Project leaders** can:
  - Create projects and assign tasks to team members.
  - Define task priorities (**Low, Medium, High**).
  - Add deadlines and instructions for each task.

### 3. Task Monitoring
- Track task progress using a **Kanban-style workflow view**.
- View task status (**To-Do, In Progress, Done**).
- Monitor task completion rates for team members.

### 4. User Roles
- **Project Leader**:  
  - Assign tasks, monitor task progress, and oversee the team’s workload.
- **Team Members**:  
  - Manage assigned tasks and update their status as they progress.
- **Individual Users**:  
  - Use the system for personal task organization without team involvement.

### 5. Notifications and Reminders
- Notify team members of **new tasks** or **approaching deadlines**.
- Provide project leaders with **progress updates** or **overdue task alerts**.

### 6. Reporting
- Generate simple **reports** for task statuses and team performance.
- View **historical data** for completed projects.

---

## Technology Stack

### Frontend:
- **Vue.js**: A progressive framework for building interactive and responsive user interfaces.
- **Libraries**:  
  - Axios (API communication)  
  - Tailwind CSS (UI design)

### Backend:
- **ASP.NET Core**: A cross-platform framework for building robust and scalable RESTful APIs.
- **SignalR**: For real-time notifications and updates.

### Database:
- **PostgreSQL**: A reliable and scalable relational database for storing tasks, users, and projects.

---

## User Stories

### 1. As a **Team Member**:
- I want to view all tasks assigned to me so I can focus on my responsibilities.
- I want to update the status of my tasks to reflect my progress.

### 2. As a **Project Leader**:
- I want to assign tasks to team members so they know what to work on.
- I want to monitor task progress to ensure project timelines are met.
- I can add comments for team members on the tasks they’re dealing with.

### 3. As an **Individual User**:
- I want to manage my personal tasks without being part of a team.
- I want to prioritize and track my tasks to improve productivity.

---

# Installation & Setup

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (LTS version recommended)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/downloads)

## Backend Setup

1. Navigate to the backend directory:
```bash
cd backend/backend
```

2. Install .NET dependencies:
```bash
dotnet restore
```

3. Configure Database Connection:
   - Create a new file named `appsettings.Development.json` in the backend project
   - Add the following configuration (modify according to your PostgreSQL setup):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=your_database_name;Username=your_username;Password=your_password"
  },
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  }
}
```

4. Apply database migrations:
```bash
dotnet ef database update
```

5. Run the backend:
```bash
dotnet run
```
The API will be available at `https://localhost:7001` (or similar port)

## Frontend Setup

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Create a `.env` file in the frontend directory:
```env
VITE_API_URL=https://localhost:7001/api
```

4. Run the development server:
```bash
npm run dev
```
The frontend will be available at `http://localhost:5173`

# Database Setup

### Step 1: Using SQL Dump File
If you have a SQL dump file:

1. Create a new database:
```bash
psql -U postgres
CREATE DATABASE your_database_name;
\q
```

2. Import the database:
```bash
psql -U postgres -d your_database_name < path_to_your_dump.sql
```

### Step 2: Using Entity Framework Migrations

If starting fresh:

1. Create database in PostgreSQL:
```bash
psql -U postgres
CREATE DATABASE your_database_name;
\q
```

2. Navigate to the backend directory and run:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Development Notes

### Backend
- Uses JWT authentication
- CORS is enabled for frontend requests

### Frontend
- Built with Vue 3 and Vite
- Uses Vuex for state management
- Includes Tailwind CSS for styling
- Toast notifications via vue-toastification
- Sweet Alert 2 for dialogs

## Common Issues and Solutions

1. Database Connection Issues:
   - Verify PostgreSQL is running
   - Check connection string in `appsettings.Development.json`
   - Ensure database user has proper permissions

2. Frontend API Connection:
   - Verify API URL in `.env` matches backend URL
   - Check CORS configuration in backend
   - Ensure backend is running when starting frontend

3. JWT Token Issues:
   - Verify JWT configuration in `appsettings.Development.json`
   - Check token expiration settings
   - Ensure secure key storage

## Production Deployment

1. Backend:
```bash
dotnet publish -c Release
```

2. Frontend:
```bash
npm run build
```

The frontend build output will be in the `dist` directory.

## Additional Resources

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Vue.js Documentation](https://vuejs.org/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
