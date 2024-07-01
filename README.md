# Banking Control Panel API

This repository contains the source code for the Banking Control Panel API, which provides endpoints for managing clients and their accounts securely.

## Overview

The Banking Control Panel API is built using ASP.NET Core and integrates with SQLite for data storage. It includes authentication via JWT tokens and authorization based on user roles. Swagger is integrated for API documentation.

## Features

- **Client Management**:
  - Add, update, delete clients
  - Retrieve clients by ID, filter, or sort
- **Account Management**:
  - Manage client accounts with balance validation

## Technologies Used

- ASP.NET Core
- Entity Framework Core (SQLite)
- JWT Authentication
- Swagger/OpenAPI
- FluentValidation

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (optional)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/FaisalKhadim/BankingControlPanel.git
