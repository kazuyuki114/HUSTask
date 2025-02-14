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

## Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/HUSTask.git
   cd HUSTask
