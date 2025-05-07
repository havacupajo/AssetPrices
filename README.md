# AssetPrices

The **AssetPrices API** is a .NET 8-based web application that provides endpoints for managing assets and their prices. It allows users to retrieve, create, and update asset prices while ensuring data validation and integrity.

## Features
- Manage assets with unique identifiers (`Symbol`, `Name`, `ISIN`).
- Manage asset prices with attributes like `Symbol`, `Source`, `Date`, and `Price`.
- Validation of input data using FluentValidation.
- In-memory database support for testing.
- Unit tests using xUnit and Moq.

---

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A database connection (e.g., SQL Server) or use an in-memory database for testing.

---

## Setup Instructions

1. **Clone the Repository:**
	
	`git clone https://github.com/havacupajo/AssetPrices.git` 
	
	`cd AssetPrices.Api`

2. **Restore NuGet packages:**
	`dotnet restore`

3. **Build the project:**
	`dotnet build`

4. **Update the connection string:**

   Update the `appsettings.json` file to enable / disable seed data (OPTIONAL)
   	```bash 
	"DatabaseSettings": { "SeedAssets": true }
	```

5. **Run the application:**
	`dotnet run`
  
6. **Access the API**:
   The API will be available at `http://localhost:5023/swagger/index.html`.

---

## API Endpoints

### **Assets**
- **GET** `/assets`  
  Retrieve all assets.

- **GET** `/assets/{symbol}`  
  Retrieve a specific asset by its symbol.

- **POST** `/assets`  
  Create a new asset.  
  **Body**:
	```bash 
	{ 
	  "symbol": "XYZ1", 
	  "name": "Example Asset", 
	  "isin": "US1234567890" 
	}
	```

- **PUT** `/assets/{id}`  
  Update an existing asset.  
  **Body**: 
    ```bash 
	{ 
	  "symbol": "XYZ1", 
	  "name": "Updated Example Asset", 
	  "isin": "US0987654321" 
	}
	```
 
---

### **Prices**
- **GET** `/prices`  
  Retrieve prices for one or more assets.  
  **Query Parameters**:
  - `symbol` (optional)
  - `date` (optional)
  - `source` (optional)

- **POST** `/prices`  
  Create a new price.  
  **Body**: 
    ```bash 
    { 
	  "symbol": "AAPL", 
	  "source": "NASDAQ", 
	  "date": "2025-05-07", 
	  "price": 150 
	}
	```

- **PUT** `/prices`  
  Update an existing price.  
  **Body**: 
	```bash 
    { 
	  "symbol": "AAPL", 
	  "source": "NASDAQ", 
	  "date": "2025-05-07", 
	  "price": 200 
	}
	```

---

## Testing

### **Run Unit Tests**
The project includes unit tests for the `CreatePrice` and `UpdatePrice` methods using xUnit and Moq.

1. Navigate to the test project:
	`cd AssetPrices.Tests`

2. Run the tests:
    `dotnet test`

---

## Design Decisions

### **Design Approach**
The application follows a **modular and minimalistic design** using .NET 8's minimal APIs. This approach was chosen to:
- Simplify the development process by reducing boilerplate code.
- Focus on the core functionality of the API without introducing unnecessary complexity.
- Leverage the performance benefits of minimal APIs for lightweight applications.

The use of **FluentValidation** ensures robust input validation, while the **in-memory database** simplifies testing and development.

### **Alternative Designs Considered**
1. **CQRS (Command Query Responsibility Segregation):**
   - CQRS was considered for separating read and write operations. However, it was deemed unnecessary for this project due to its relatively simple domain and lack of complex business logic.
   - CQRS would have added additional layers of complexity, such as separate models for queries and commands, which were not justified for this use case.

2. **Vertical Slice Architecture:**
   - Vertical Slice Architecture was considered to group features by functionality rather than technical layers. While this could improve maintainability for larger projects, the current project size and scope did not warrant this approach.

3. **Clean Architecture:**
   - Clean Architecture was considered for its separation of concerns and testability. However, the additional layers (e.g., domain, application, infrastructure) were not necessary for this relatively small project.

### **Assumptions**
- The application assumes that asset symbols (`Symbol`) are unique and serve as the primary identifier for assets.
- The `ISIN` field is assumed to follow the standard International Securities Identification Number format, but no strict validation is enforced in the current implementation.
- The in-memory database is sufficient for development and testing purposes. For production, a relational database (e.g., SQL Server) is expected to be configured.
- The API consumers are expected to handle pagination and filtering on the client side, as these features are not implemented in the current version.

### **Other Considerations**
- **Scalability:** While the current implementation is suitable for small-scale use, additional features like caching, pagination, and database indexing may be required for larger datasets.
- **Error Handling:** Basic error handling is implemented, but more detailed error responses (e.g., error codes, logging) could be added for production readiness.
- **Security:** HTTPS is enforced, but additional security measures (e.g., authentication, authorization) should be implemented for production environments.
- **Extensibility:** The modular design allows for easy addition of new endpoints or features in the future.

---