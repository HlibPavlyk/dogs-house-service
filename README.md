
# DogsHouseService

**DogsHouseService** is a test assignment developed as an ASP.NET Core application to manage information about dogs. It provides an API for creat, read and ping operations on dog entities, with support for sorting, pagination, and rate-limiting incoming requests.

## Features

- **Operations**: Create, read and ping dog records.
- **Sorting and Pagination**: Retrieve dog records with sorting and pagination options.
- **Rate Limiting**: Controls the number of requests per time interval to prevent overloading.

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server or any configured database

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/HlibPavlyk/dogs-house-service.git
   cd dogs-house-service
   ```

2. **Configure the database connection** in `appsettings.json`:
   ```json
   "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true;"
    }
   ```

3. **Apply Migrations:**
   Run the following command to create the necessary database tables:
   ```bash
   dotnet ef database update
   ```
   
4. **Install Dependencies:**
   ```bash
    dotnet restore
    ```

### Running the Application

To start the application:
```bash
dotnet run --project .\src\DogsHouseService.Api\ --urls "http://localhost:5000"

```

The API will be accessible at `http://localhost:5000/swagger` by default.
