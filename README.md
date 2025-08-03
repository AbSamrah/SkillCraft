# SkillCraft: An AI-Powered Learning Guide
SkillCraft is a modern, full-stack application designed to be a personal guide for mastering new skills. It provides users with structured, interactive learning paths called "Roadmaps." Built with a powerful .NET backend and a dynamic React frontend, SkillCraft uses a role-based architecture and integrates AI to automate and enhance content creation, offering a robust and scalable system for both learners and content creators.

Features
For Learners (Users)
Browse & Follow Roadmaps: Discover and follow expert-curated learning paths on a variety of topics.

Interactive Progress Tracking: Mark steps as complete and visually track your progress through a roadmap with dynamic completion percentages and time tracking.

Personal Profile: Add and remove roadmaps from your personal profile to manage your learning journey.

Interactive Quizzes: Test your knowledge with two types of quizzes (Multiple Choice and True/False) and get immediate feedback.

AI-Powered Generation: Generate personalized roadmaps and quizzes on any topic using AI.

For Editors & Admins
Full Content Management: A comprehensive dashboard for full CRUD (Create, Read, Update, Delete) operations on Roadmaps, Milestones, Steps, and Quizzes.

Advanced Filtering & Pagination: Easily search and navigate through large amounts of content with built-in search bars and pagination for all content types.

For Admins
User Management: A dedicated dashboard to create, view, edit, and delete users.

Role Management: Assign roles (User, Editor, Admin) to users to manage permissions across the application.

Architecture & Design Patterns
SkillCraft is built on a foundation of modern software architecture principles to ensure it is scalable, maintainable, and robust.

1. High-Level Architecture: Client-Server
The application follows a classic Client-Server Architecture, which separates the user interface from the business logic and data storage.

Client (React): A Single Page Application (SPA) that runs entirely in the user's browser, responsible for all user interactions and views.

Server (.NET): A centralized backend that handles business logic, data persistence, and security. The client communicates with the server via a RESTful API over HTTPS.

2. Backend Architecture: Modular Monolith
The backend is designed as a Modular Monolith. While it's deployed as a single application, it is internally divided into distinct, loosely coupled modules, each with a specific business responsibility. This approach combines the simplicity of a monolithic deployment with the organizational benefits of microservices.

The core modules are:

Users Management: Handles authentication, authorization, and user data.

Roadmap Management: Manages the creation and structure of learning roadmaps.

Quizzes Management: Manages all types of quizzes and their content.

Profiles Management: Manages user-specific data, like their progress and saved roadmaps.

3. Database Architecture: Hybrid Model
To best suit the different types of data, SkillCraft uses a hybrid database approach:

SQL Server: Used for the Users Management module. Its relational structure is ideal for storing user accounts and roles, where data integrity and well-defined relationships are critical.

MongoDB: Used for the Roadmap, Quizzes, and Profiles modules. Its flexible, document-based structure is perfect for handling the hierarchical and evolving nature of educational content and user profiles.

4. Key Design Patterns
Repository & Unit of Work Pattern: Used in the Data Access Layer to abstract the database logic from the business logic. Repositories handle the queries for each entity, and the Unit of Work manages transactions to ensure data consistency.

Strategy Pattern: Used for content creation. The system has an IRoadmapCreationStrategy and an IQuizCreationStrategy, with different implementations for Manual and AI creation. This makes the system easily extensible—new creation methods (e.g., from a different AI model) can be added without changing the core services.

Tech Stack
Backend (.NET)
Framework: ASP.NET Core

Authentication: JSON Web Tokens (JWT)

Databases: Entity Framework Core with SQL Server, and the official MongoDB Driver.

AI Integration: Gemini API

Testing: xUnit, Moq, Testcontainers

Frontend (React)
Framework: React (functional components and hooks)

Routing: React Router DOM

API Communication: Axios with JWT interceptors

State Management: React Context API (AuthContext, ThemeContext)

Styling: Bootstrap & Sass, with a dynamic Dark Theme.

Testing: Jest, React Testing Library

Getting Started
To get a local copy up and running, follow these simple steps.

Prerequisites
.NET 8 SDK or later

Node.js and npm

An IDE like Visual Studio or VS Code

Access to a SQL Server instance

Access to a MongoDB instance

Backend Setup
Clone the backend repository:

git clone [YOUR_BACKEND_REPO_URL]
cd [backend-folder]

Configure your connection strings:

Open appsettings.Development.json.

Update the DefaultConnection string to point to your SQL Server instance.

Update the MongoDbConnection string to point to your MongoDB instance.

Update the database:

Run dotnet ef database update in the terminal to apply the Entity Framework migrations for the SQL database.

Run the backend server:

dotnet run

The API will be running on http://localhost:5093.

Frontend Setup
Clone the frontend repository:

git clone [YOUR_FRONTEND_REPO_URL]
cd [frontend-folder]

Install NPM packages:

npm install

Run the frontend application:

npm start

The application will open in your browser at http://localhost:3000.

About the Project
SkillCraft was developed as a comprehensive solution for self-guided learning, focusing on a clean, maintainable codebase and a rich, interactive user experience. The choice of a Modular Monolith architecture for the backend allows for organized, domain-driven development while maintaining the simplicity of a single deployment pipeline. The integration of AI for content generation aims to reduce the effort for content creators and provide a unique, dynamic experience for learners.
