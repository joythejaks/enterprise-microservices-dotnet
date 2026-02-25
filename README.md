# 🚀 Enterprise Microservices Architecture (.NET 9)

Production-style microservices architecture built with .NET 9,
demonstrating authentication, authorization, API Gateway pattern,
Dockerized infrastructure, and clean API design.

------------------------------------------------------------------------

# 📌 Overview

This project demonstrates a secure and scalable microservices
architecture including:

-   🔐 JWT Authentication
-   🛡 Policy-Based Authorization
-   🌐 API Gateway (YARP)
-   🗄 SQL Server with EF Core
-   🐳 Docker & Docker Compose
-   🧱 Clean API Response Wrapper
-   ⚠ Global Exception Handling
-   🔄 Automatic Database Migration with Retry Strategy

------------------------------------------------------------------------

# 🏗 System Architecture

                ┌──────────────────┐
                │    API Gateway   │
                │     (YARP)       │
                └─────────┬────────┘
                          │
         ┌────────────────┴────────────────┐
         │                                 │

┌───────────────┐ ┌────────────────┐ │ AuthService │ │ AttendanceSvc │ │
JWT Provider │ │ Secure APIs │ │ Role Manager │ │ Admin Policy │
└───────────────┘ └────────────────┘ │ ▼ ┌────────────────┐ │ SQL Server
│ │ (Dockerized) │ └────────────────┘

------------------------------------------------------------------------

# 🔐 Authentication & Authorization Flow

## 1️⃣ Register User

POST /auth/api/auth/register

## 2️⃣ Login

POST /auth/api/auth/login

Returns JWT token.

## 3️⃣ Access Protected Endpoint

GET /attendance/api/attendance/check-in Authorization: Bearer {token}

## 4️⃣ Admin Only Endpoint

GET /attendance/api/attendance/check-in-admin

Requires Role = Admin

------------------------------------------------------------------------

# 🛡 Security Features

-   JWT Token Validation
-   Role Claims (User / Admin)
-   Policy-Based Authorization
-   Custom Authorization Middleware
-   Custom 401 / 403 JSON Response
-   Standardized API Response Format
-   Password Hashing using BCrypt

------------------------------------------------------------------------

# 🧱 Clean API Response Format

All responses follow consistent structure:

{ "success": true, "message": "Operation successful", "data": {},
"errors": null }

------------------------------------------------------------------------

# 🗄 Database Design

## AuthService Database

-   Users (Id, Username, PasswordHash, Role)

## AttendanceService Database

-   AttendanceRecords (Id, Username, CheckInTime)

Migrations are automatically applied on container startup with retry
logic.

------------------------------------------------------------------------

# 🐳 Dockerized Infrastructure

Run the entire system:

docker compose up --build

Services:

-   API Gateway → http://localhost:5000
-   AuthService → internal container
-   AttendanceService → internal container
-   SQL Server → containerized instance

------------------------------------------------------------------------

# 📂 Project Structure

enterprise-microservices-dotnet/ │ ├── src/ │ ├── AuthService/ │ ├──
AttendanceService/ │ ├── ApiGateway/ │ ├── docker-compose.yml └──
README.md

------------------------------------------------------------------------

# 🛠 Tech Stack

-   .NET 9
-   ASP.NET Core Web API
-   Entity Framework Core
-   SQL Server
-   YARP Reverse Proxy
-   JWT Bearer Authentication
-   Policy-Based Authorization
-   Docker
-   Docker Compose
-   BCrypt Password Hashing

------------------------------------------------------------------------

# 🚀 How to Run

1.  Clone repository
2.  Navigate to root folder
3.  Run:

docker compose up --build

4.  Access via: http://localhost:5000

------------------------------------------------------------------------

# 👨‍💻 Author

Jordi Setiawan\
Backend Engineer \
