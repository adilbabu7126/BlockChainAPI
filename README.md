Introduction: Blockchain API
================================
A .NET 8 Web API to fetch blockchain data (BTC, ETH, DASH, LTC) from BlockCypher, store in SQLite, and return history.

Solution Creation
-------------------
- Added New Project - ASP.NET Core Web API - For BlockChainAPI.csproj
- Added New Project - Class Library - For Application.csproj
- Added New Project - Class Library - For Domain.csproj
- Added New Project - Class Library - For Infrastructure.csproj
- Added New Project - xUnit Test Project - For Test.csproj


NuGet Package  installation (Tools >> NuGet Package Manager >> Package Manager Console)
--------------------------------------------------------------------------------------
- Install-Package Microsoft.EntityFrameworkCore
- Install-Package Microsoft.EntityFrameworkCore.Sqlite
- Install-Package Microsoft.Extensions.Logging
- Install-Package Moq

Steps to Build and Run Application:
----------
- Build the Solution fron Visual Studio - Build >> Build Solution
- Run the Solution in https - Command (F5)
Swagger URL => https://localhost:7107/swagger/index.html


API Endpoints
-------------------
- POST /api/blockchain/Get/{blockchainType} → fetch & store (blockchainType = BTC/ETH/DASH/LTC)
- GET /api/blockchain/history/{blockchainType} → get history sorted by CreatedAt
- GET /api/blockchain/{id} → get record by Id
- GET /health → health check


Technology Used
---------------
1. .NET 8 Web API
2. EF Core + SQLite
3. Repository + Service pattern
4. Swagger + HealthChecks

======================================================
