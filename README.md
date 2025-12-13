# CarNest API

CarNest is a comprehensive RESTful API for a car marketplace built with ASP.NET Core 8.0, designed using Clean Architecture principles and implementing complete Role-Based Access Control (RBAC).

## Overview

CarNest provides a robust backend for a multi-role car marketplace application. It supports user registration and authentication, car listings with multiple images, favorites management, and structured catalog data (makes, models, body types, fuel types, and locations). The system is designed for scalability, maintainability, and clear separation of concerns.

## Features

### Core Functionality

- Multi-image support for car listings (stored as JSON, with file management)
- Full CRUD support for core entities (Cars, Favorites, Categories, etc.)
- Role-Based Access Control (RBAC) with distinct permissions
- JWT-based authentication and authorization
- Clean Architecture with clear layering (DAL, BLL, Presentation)
- File upload handling with validation (size, type, count, MIME)
- Pagination and advanced filtering support
- Ready-to-use Swagger documentation

### Car Features

- **Condition**: New or Used
- **Gear Type**: Manual or Automatic
- **Mileage tracking** for used cars
- **Last inspection date** support

### User Roles and Permissions

#### Admin

- Can view all cars
- Can create cars with images
- Can update any car
- Can delete any car
- Can register new admins
- Can manage all system entities and reference data

#### Vendor

- Can view only their own cars
- Can create cars with images
- Can update only their own cars
- Can delete only their own cars
- Cannot modify other vendors' cars

#### Customer

- Can view all cars
- Can view car details
- Can add cars to favorites
- Can view their favorites
- Can remove cars from favorites
- Cannot create, update, or delete cars

## Architecture

The solution follows Clean Architecture and is organized into distinct layers:

```
CarNest/
├── DAL/                    # Data Access Layer
│   ├── Context/            # Database context (CarNestDBContext)
│   ├── Entity/             # Database entities
│   ├── Migrations/         # EF Core migrations
│   ├── Repository/         # Generic repository pattern
│   └── UnitOfWork/         # Unit of Work pattern
├── BLL/                    # Business Logic Layer
│   └── Manager/            # Business logic managers
│       ├── CarManager/
│       ├── FavoriteManager/
│       ├── MakeManager/
│       ├── ModelManager/
│       ├── BodyTypeManager/
│       ├── FuelTypeManager/
│       └── LocationManager/
├── Shared/                 # Shared components
│   ├── DTOs/               # Data Transfer Objects
│   │   ├── Requests/
│   │   └── Responses/
│   └── Mappings/           # Entity-DTO mappings
└── Presentation/           # API Layer
    ├── Controllers/        # API controllers
    ├── Mappings/           # Presentation-layer mappings
    ├── DbInitializer.cs    # Database seeding
    └── wwwroot/            # Static files (car images)
```

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**

```bash
git clone https://github.com/amirabdelhak/CarNest.git
cd CarNest
```

2. **Update Connection String**

Edit `Presentation/appsettings.json` to set your database connection string:

```json
{
  "ConnectionStrings": {
    "DBConnection": "Server=YOUR_SERVER;Database=CarNest;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

> **Note**: Update the connection string key in `Program.cs` if you change the key name.

3. **Apply Database Migrations**

```bash
dotnet ef database update --project DAL --startup-project Presentation
```

4. **Build and Run**

```bash
dotnet build
dotnet run --project Presentation
```

The API will be available at: `https://localhost:7XXX` (check console output)

### Default Admin Account

On first run, the system automatically seeds an admin user with credentials from `appsettings.json`:

- **Email**: admin@carnest.com
- **Password**: Admin@123

### Test Data (Development Only)

In development mode, the system seeds sample data including:
- Body types (Sedan, SUV, Hatchback)
- Fuel types (Gasoline, Diesel, Electric)
- Locations (Cairo, Alexandria, Giza)
- Makes (Toyota, Honda) with models
- A test vendor account (`vendor@test.com` / `Vendor@123`)
- Sample car listings

## API Endpoints

### Authentication

| Method | Endpoint                     | Description                      | Access    |
|--------|------------------------------|----------------------------------|-----------|
| POST   | `/api/account/register/admin`    | Register new admin           | Admin     |
| POST   | `/api/account/register/vendor`   | Register new vendor          | Public    |
| POST   | `/api/account/register/customer` | Register new customer        | Public    |
| POST   | `/api/account/login`             | Login and get JWT token      | Public    |

### Cars

| Method | Endpoint        | Description                                    | Access              |
|--------|-----------------|------------------------------------------------|---------------------|
| GET    | `/api/car`      | Get all cars with pagination/filtering         | Authenticated       |
| GET    | `/api/car/{id}` | Get car details                                | Authenticated       |
| POST   | `/api/car`      | Create car with images                         | Admin, Vendor       |
| PUT    | `/api/car/{id}` | Update car                                     | Admin, Vendor (own) |
| DELETE | `/api/car/{id}` | Delete car                                     | Admin, Vendor (own) |

#### Pagination & Filtering Parameters

| Parameter    | Type     | Description                          |
|--------------|----------|--------------------------------------|
| PageNumber   | int      | Page number (default: 1)             |
| PageSize     | int      | Items per page (default: 10)         |
| MakeId       | int?     | Filter by make                       |
| ModelId      | int?     | Filter by model                      |
| BodyTypeId   | int?     | Filter by body type                  |
| FuelId       | int?     | Filter by fuel type                  |
| LocId        | int?     | Filter by location                   |
| MinPrice     | decimal? | Minimum price                        |
| MaxPrice     | decimal? | Maximum price                        |
| Year         | int?     | Filter by year                       |
| Condition    | enum?    | New (0) or Used (1)                  |
| MinMileage   | int?     | Minimum mileage                      |
| MaxMileage   | int?     | Maximum mileage                      |
| GearType     | enum?    | Manual (0) or Automatic (1)          |
| SearchTerm   | string?  | Text search                          |

### Favorites

| Method | Endpoint                | Description                      | Access   |
|--------|-------------------------|----------------------------------|----------|
| GET    | `/api/favorite`         | Get customer's favorites         | Customer |
| POST   | `/api/favorite`         | Add car to favorites             | Customer |
| DELETE | `/api/favorite/{carId}` | Remove from favorites            | Customer |

### Reference Data

| Method | Endpoint        | Description          |
|--------|-----------------|----------------------|
| GET    | `/api/make`     | Get all car makes    |
| GET    | `/api/model`    | Get all car models   |
| GET    | `/api/bodytype` | Get all body types   |
| GET    | `/api/fueltype` | Get all fuel types   |
| GET    | `/api/location` | Get all locations    |

## Request/Response Examples

### Create Car with Images (Multipart Form Data)

```javascript
const formData = new FormData();
formData.append('Year', 2024);
formData.append('Price', 450000);
formData.append('Description', 'Brand new Toyota Corolla');
formData.append('ModelId', 1);
formData.append('BodyTypeId', 1);
formData.append('FuelId', 1);
formData.append('LocId', 1);
formData.append('Condition', 0);        // 0 = New, 1 = Used
formData.append('GearType', 1);         // 0 = Manual, 1 = Automatic
formData.append('Mileage', 0);          // Optional for new cars

// Add multiple images
for (let i = 0; i < imageFiles.length; i++) {
    formData.append('images', imageFiles[i]);
}

const response = await fetch('/api/car', {
    method: 'POST',
    headers: {
        'Authorization': `Bearer ${token}`
    },
    body: formData
});
```

### Update Car with Image Management

```javascript
const formData = new FormData();
formData.append('Year', 2024);
formData.append('Price', 440000);
// ... other fields

// Add new images
newImageFiles.forEach(file => formData.append('newImages', file));

// Delete existing images (JSON array)
const imagesToDelete = ['images/cars/old-image-1.jpg'];
formData.append('imagesToDeleteJson', JSON.stringify(imagesToDelete));

await fetch(`/api/car/${carId}`, {
    method: 'PUT',
    headers: { 'Authorization': `Bearer ${token}` },
    body: formData
});
```

### Login Response

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "abc-123-def",
  "email": "vendor@test.com",
  "role": "Vendor",
  "expiresAt": "2025-01-28T20:00:00Z"
}
```

### Car Response

```json
{
  "carId": "abc-123",
  "year": 2024,
  "price": 450000,
  "description": "Brand new Toyota Corolla 2024",
  "createdDate": "2025-01-27T20:00:00Z",
  "imageUrls": [
    "images/cars/guid1.jpg",
    "images/cars/guid2.jpg"
  ],
  "makeName": "Toyota",
  "modelName": "Corolla",
  "bodyTypeName": "Sedan",
  "fuelName": "Gasoline",
  "locationName": "Cairo",
  "condition": 0,
  "mileage": null,
  "lastInspectionDate": null,
  "gearType": 1
}
```

### Paginated Response

```json
{
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5,
  "totalRecords": 48,
  "data": [
    { /* car objects */ }
  ]
}
```

### Error Response

```json
{
  "message": "You can only update your own cars"
}
```

## Authentication

The API uses JWT Bearer tokens. Include the token in the Authorization header:

```
Authorization: Bearer YOUR_JWT_TOKEN
```

Token expiry is configurable (default: 1440 minutes / 24 hours).

## Database Schema

### Main Entities

| Entity         | Description                              |
|----------------|------------------------------------------|
| Cars           | Vehicle listings with images (JSON)      |
| Makes          | Car manufacturers                        |
| Models         | Car models (linked to Makes)             |
| BodyTypes      | Vehicle body types                       |
| FuelTypes      | Fuel types                               |
| LocationCities | Available locations                      |
| Favorites      | Customer favorites                       |
| Admin          | Admin user accounts (extends IdentityUser) |
| Vendor         | Vendor user accounts (extends IdentityUser) |
| Customer       | Customer user accounts (extends IdentityUser) |

### Car Entity Fields

| Field              | Type          | Constraints                      |
|--------------------|---------------|----------------------------------|
| CarId              | string (36)   | Primary Key (GUID)               |
| Year               | int           | Required, 1900-2026              |
| Price              | decimal(18,2) | Required, > 0                    |
| Description        | string        | Max 2048 characters              |
| ImageUrls          | string (JSON) | Array of image paths             |
| Condition          | enum          | New (0), Used (1)                |
| Mileage            | int?          | >= 0, for used cars              |
| LastInspectionDate | DateTime?     | Optional inspection date         |
| GearType           | enum          | Manual (0), Automatic (1)        |
| ModelId            | int           | Required FK                      |
| BodyTypeId         | int           | Required FK                      |
| FuelId             | int           | Required FK                      |
| LocId              | int           | Required FK                      |
| AdminId            | string?       | FK (if created by admin)         |
| VendorId           | string?       | FK (if created by vendor)        |
| CreatedDate        | DateTime      | Auto-set to UTC now              |

## Configuration

### appsettings.json Structure

```json
{
  "ConnectionStrings": {
    "DBConnection": "Server=.;Database=CarNest;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKey_MinimumLength32Characters!",
    "Issuer": "CarNestAPI",
    "Audience": "CarNestClient",
    "ExpiryMinutes": 1440
  },
  "Admin": {
    "Username": "admin",
    "Email": "admin@carnest.com",
    "Password": "Admin@123",
    "FirstName": "System",
    "LastName": "Administrator",
    "Address": "Cairo, Egypt",
    "NationalId": "00000000000000"
  }
}
```

## Security Features

- JWT token authentication with configurable expiry
- Role-based authorization (Admin, Vendor, Customer)
- Password hashing with ASP.NET Core Identity
- CORS configuration (AllowAll policy - configure for production)
- File upload validation (size, type, MIME)
- Ownership validation for Vendor operations

## Dependencies

- ASP.NET Core 8.0
- Entity Framework Core 8.0
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.AspNetCore.Authentication.JwtBearer
- SQL Server
- Swashbuckle.AspNetCore (Swagger)

## Project Structure

| Project      | Description                                      |
|--------------|--------------------------------------------------|
| Presentation | API controllers, configuration, static files     |
| BLL          | Business logic managers                          |
| DAL          | Database context, entities, repositories         |
| Shared       | DTOs, mappings shared across layers              |

## Contributing

We welcome contributions! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.

---

**Quick Start**: After setup, visit `/swagger` for interactive API documentation.
