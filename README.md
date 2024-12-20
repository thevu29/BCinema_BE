# Cinema Ticket Booking Backend

This is the backend application for the Cinema Ticket Booking system, built using .NET 8.0 with Onion architecture. It provides RESTful APIs for managing users, movies, schedules, bookings, and payments.

Frontend: https://github.com/thevu29/BCinema_FE.git

---

## Table of Contents
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Environment Variables](#environment-variables)
- [License](#license)

---

## Features
- User authentication and authorization using JWT
- Schedule management for movies
- Ticket booking with seat selection
- Payment integration with Momo
- Custom API responses for better client-side handling
- Role-based access control (Admin and Customer roles)

---

## Tech Stack
- **Framework**: .NET 8.0
- **Database**: PostgreSQL
- **Authentication**: JWT and OAuth2 (Google)
- **Payment Gateway**: Momo
- **Containerization**: Docker
- **API Documentation**: Swagger

---

## Getting Started

### Prerequisites
Ensure you have the following installed on your system:
- .NET 8.0 SDK
- Docker
- PostgreSQL
- Visual Studio or VS Code or JetBrains Rider

### Clone the Repository
```bash
git clone https://github.com/thevu29/BCinema_BE.git
```

### Setup Database
1. Create a PostgreSQL database.
2. Update the `appsettings.json` file with your database connection string.

### Run the Application
```bash
dotnet build
dotnet run
```
The application will run at `https://localhost:7263` or `http://localhost:5178`.

### Docker Setup
 ```bash
docker-compose up --build
```

---

## Environment Variables
Set up the following environment variables in your `appsettings.json` based on `appsettings.json.example`.

---

## Database Schema
Below is the database schema diagram:

![Database Schema](/database_diagram.png)

---

## License
This project is licensed under the MIT License. See the LICENSE file for details.
