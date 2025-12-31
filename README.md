# CarNest API

CarNest is a comprehensive RESTful API for a car marketplace built with ASP.NET Core 8.0. The solution implements a robust **Layered Architecture** to ensure separation of concerns, maintainability, and scalability, featuring complete Role-Based Access Control (RBAC).

## Overview

CarNest provides a backend for a multi-role car marketplace application. It supports user registration and authentication, detailed car listings with multiple images and specifications, favorites management, and structured catalog data.

## Features

### Core Functionality

- **Layered Architecture**: Organized into DAL, BLL, Shared, and Presentation projects.
- **Role-Based Access Control (RBAC)**: Distinct permissions for Admins, Vendors, and Customers.
- **JWT Authentication**: Secure, token-based authentication with configurable expiry.
- **Advanced Car Listings**: Support for multiple images, license verification, and detailed specs.
- **File Management**: robust handling of image uploads (cars & licenses) with validation.
- **Approval Workflow**: System for Admins to approve or reject car listings.
- **Search & Filter**: Advanced pagination and filtering capabilities.

### Car Specifications

- **Condition**: New or Used
- **Transmission**: Manual or Automatic
- **Drivetrain**: FWD, RWD, AWD, or 4WD
- **Engine**: Capacity (Liters) and Horsepower
- **Appearance**: Exterior and Interior colors
- **History**: Mileage tracking and inspection dates for used cars
- **Verification**: Mandatory car license image upload
- **Status**: Pending → Approved / Rejected workflow

### User Roles

#### Admin
- Full access to all cars (Approved, Pending, Rejected).
- Manage car approvals (Approve/Reject listings).
- Create and manage system-wide data (Makes, Models, etc.).
- Register new Admins.

#### Vendor
- Create and manage their own car listings.
- View only their own cars.
- Updates to cars reset status to "Pending" for re-approval.

#### Customer
- View approved car listings.
- Manage a personal "Favorites" list.
- View car details (excluding sensitive license data).

## Architecture

The solution is organized into four distinct projects:

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

### Installation

1. **Clone the repository**

```bash
git clone https://github.com/amirabdelhak/CarNest.git
cd CarNest
```

2. **Configure Database**
   Update `Presentation/appsettings.json` with your connection string:

```json
{
  "ConnectionStrings": {
    "DBConnection": "Server=YOUR_SERVER;Database=CarNest;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

> **Note**: Update the connection string key in `Program.cs` if you change the key name.

3. **Apply Migrations**

```bash
dotnet ef database update --project DAL --startup-project Presentation
```

4. **Run the API**

```bash
dotnet run --project Presentation
```

The API will be available at: `https://localhost:7XXX` (check console output)

### Default Admin
The system seeds a default admin on the first run:
- **Email**: `admin@carnest.com`
- **Password**: `Admin@123`

## API Endpoints

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/account/login` | Login to get JWT token |
| POST | `/api/account/register/customer` | Register as Customer |
| POST | `/api/account/register/vendor` | Register as Vendor |
| POST | `/api/account/register/admin` | Register as Admin (Admin only) |
| PUT | `/api/account/profile` | Update profile info |
| POST | `/api/account/change-password` | Change password |

### Cars
| Method | Endpoint | Description | Access |
|--------|----------|-------------|--------|
| GET | `/api/car` | Get approved cars (Paginated) | Auth |
| GET | `/api/car/{id}` | Get car details | Auth |
| POST | `/api/car` | Create new listing | Admin, Vendor |
| PUT | `/api/car/{id}` | Update listing | Admin, Vendor (Owner) |
| DELETE | `/api/car/{id}` | Delete listing | Admin, Vendor (Owner) |
| GET | `/api/car/pending` | View pending cars | Admin |
| GET | `/api/car/rejected` | View rejected cars | Admin |
| PUT | `/api/car/{id}/status` | Update status (Approve/Reject) | Admin |

### Favorites
| Method | Endpoint | Description | Access |
|--------|----------|-------------|--------|
| GET | `/api/favorite` | Get my favorites | Customer |
| POST | `/api/favorite` | Add to favorites | Customer |
| DELETE | `/api/favorite/{carId}` | Remove from favorites | Customer |

## Request Examples

### Create Car (Multipart/Form-Data)

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

### Key Entities
- **Cars**: Main listing entity.
- **Users**: Extends IdentityUser (Admin, Vendor, Customer).
- **Reference Data**: Makes, Models, BodyTypes, FuelTypes, Locations.
- **Favorites**: Join table for Customer favorites.

### Car Constraints
- **Price**: Must be > 10,000.
- **Year**: Between 1900 and 2026.
- **Engine**: Capacity 0.1-99.99L, HP 1-2000.
- **Images**: Max 5MB per file, allowed types: jpg, png, gif, webp.

## License

This project is licensed under the MIT License.

---

**Quick Start**: After setup, visit `/swagger` for interactive API documentation.
