# SkillCraft: An AI-Powered Learning Guide

![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)
![C#](https://img.shields.io/badge/C%23-12.0-green)
![Entity%20Framework%20Core](https://img.shields.io/badge/EF%20Core-8.0-blue)
![Architecture](https://img.shields.io/badge/Architecture-Clean-red)
![License](https://img.shields.io/badge/License-MIT-yellow.svg)


SkillCraft is a modern, full-stack application designed to be a personal guide for mastering new skills. It provides users with structured, interactive learning paths called "Roadmaps." Built with a powerful .NET backend and a dynamic React frontend, SkillCraft uses a role-based architecture and integrates AI to automate and enhance content creation.

**Frontend Repository:** [**AbSamrah/skillcraft-frontend**](https://github.com/AbSamrah/skillcraft-frontend)

## About The Project

This project is a comprehensive learning platform built on a clean, modular architecture to ensure maintainability and scalability. The API exposes a set of well-defined endpoints to support a decoupled frontend application, creating a robust system for both learners and content creators.

The core functionalities include:
* **Structured Learning:** A complete system for creating and managing learning "Roadmaps" composed of milestones and steps.
* **AI-Powered Content Generation:** Integrates with the Gemini API to automatically generate entire roadmaps and quizzes based on a single topic.
* **Interactive Knowledge Checks:** A module for creating and taking two types of quizzes (Multiple Choice and True/False) to test user knowledge.
* **Personalized User Profiles:** Allows users to track their progress, save roadmaps, and manage their learning journey.

## Key Features

-   **RESTful API:** A well-structured set of endpoints for all application features.
-   **User Authentication:** Secure user registration and login system using JWT (JSON Web Tokens).
-   **Role-Based Access Control:**
    -   **Learner (User):** Can browse content, follow roadmaps, track progress, take quizzes, and generate content using AI.
    -   **Editor:** Can perform all Learner actions, plus has full CRUD (Create, Read, Update, Delete) access to all educational content.
    -   **Admin:** Has full access, including all Editor permissions plus user and role management.
-   **Advanced Content:** Filtering: All content lists in the editor dashboard are searchable and paginated.
-   **Dynamic Frontend:** A responsive React SPA with a dark theme, providing a seamless user experience.

## Technology Stack & Architecture

* **High-Level Architecture:** Client-Server
* The application follows a classic Client-Server Architecture.

**Client (React):** A Single Page Application (SPA) that runs in the user's browser.

**Server (.NET):** A centralized backend that handles business logic, data persistence, and security.

## Backend Architecture: Modular Monolith
The backend is designed as a Modular Monolith, divided internally into distinct modules:
* **Users Management:** Handles authentication, authorization, and user data.
* **Roadmap Management:** Manages the creation and structure of learning roadmaps.
* **Quizzes Management:** Manages all types of quizzes and their content.
* **Profiles Management:** Manages user-specific data, like their progress and saved roadmaps.

## Database Architecture: Hybrid Model
* **SQL Server:** Used for the Users Management module (via Entity Framework Core).
* **MongoDB:** Used for the Roadmap, Quizzes, and Profiles modules.

## Key Design Patterns
* **Repository & Unit of Work Pattern:** Used in the Data Access Layer to abstract database logic.
* **Strategy Pattern:** Used for content creation, allowing for both Manual and AI creation strategies.

## Getting Started
To get a local copy up and running, follow these steps.

Prerequisites
.NET 8.0 SDK

Node.js and npm

Microsoft SQL Server

MongoDB

A code editor like Visual Studio or VS Code

Installation
Clone the Repositories

# Clone the backend
git clone [YOUR_BACKEND_REPO_URL]

# Clone the frontend
git clone [YOUR_FRONTEND_REPO_URL]

Configure Backend Secrets
It's recommended to use the .NET Secret Manager for your connection strings and API keys.

Navigate to the SkillCraft.Api project directory and set up your secrets:

dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your_SQL_Server_Connection_String"
dotnet user-secrets set "MongoDbConnection:ConnectionString" "Your_MongoDB_Connection_String"
dotnet user-secrets set "MongoDbConnection:DatabaseName" "SkillCraftDb"
dotnet user-secrets set "Jwt:Key" "Your_Super_Secret_JWT_Key"
dotnet user-secrets set "Jwt:Issuer" "Your_Issuer"

Alternatively, you can add these values to your appsettings.Development.json file.

Setup the Databases
Navigate to the DataAccessLayer project directory and run the following command to apply migrations for the identity database:

# Apply migrations for the identity database
dotnet ef database update

The application will automatically seed the databases with initial data for roles on the first run.

Run the Backend API
Navigate back to the SkillCraft.Api project directory and run the application:

dotnet run

The API will be available at http://localhost:5093 by default.

Setup and Run the Frontend
Navigate to your frontend project directory and run:

# Install dependencies
npm install

# Run the application
npm start

The application will open in your browser at http://localhost:3000.
