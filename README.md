# CarNest API 🚗

A comprehensive RESTful API for a car marketplace built with ASP.NET Core 8.0, following Clean Architecture principles with complete Role-Based Access Control (RBAC).

## 🌟 Features

### Core Functionality
- **Multi-Image Support**: Each car can have multiple images stored as JSON with file management
- **Role-Based Access Control (RBAC)**: Three distinct user roles with specific permissions
- **Favorites System**: Customers can save cars to their favorites
- **Complete CRUD Operations**: Full Create, Read, Update, Delete operations for all entities
- **JWT Authentication**: Secure token-based authentication
- **Clean Architecture**: Separation of concerns with DAL, BLL, and Presentation layers

### User Roles & Permissions

#### 👨‍💼 Admin
- ✅ View all cars
- ✅ Create cars with images
- ✅ Update any car
- ✅ Delete any car
- ✅ Manage all system entities

#### 🏪 Vendor
- ✅ View only their own cars
- ✅ Create cars with images
- ✅ Update only their own cars
- ✅ Delete only their own cars
- ❌ Cannot modify other vendors' cars

#### 👤 Customer
- ✅ View all cars
- ✅ View car details
- ✅ Add cars to favorites
- ✅ View their favorites
- ✅ Remove cars from favorites
- ❌ Cannot create/update/delete cars

## 🏗️ Architecture

```
CarNestRepo/
├── DAL/                    # Data Access Layer
│   ├── Context/           # Database context
│   ├── Entity/            # Database entities
│   ├── Migrations/        # EF Core migrations
│   ├── Repository/        # Generic repository pattern
│   └── UnitOfWork/        # Unit of Work pattern
├── BLL/                    # Business Logic Layer
│   └── Manager/           # Business logic managers
│       ├── CarManager/
│       ├── FavoriteManager/
│       ├── MakeManager/
│       ├── ModelManager/
│       └── ...
├── Shared/                 # Shared components
│   ├── DTOs/              # Data Transfer Objects
│   │   ├── Requests/
│   │   └── Responses/
│   └── Mappings/          # Entity-DTO mappings
└── Presentation/           # API Layer
    ├── Controllers/       # API controllers
    └── wwwroot/          # Static files (car images)
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/amirabdelhak/CarNest.git
cd CarNest
```

2. **Update Connection String**
Edit `Presentation/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "MostafaDB": "Server=YOUR_SERVER;Database=CarNest;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

3. **Run Database Migration**
Execute the SQL script:
```bash
# Run this script on your SQL Server
DAL/Migrations/Manual_RemoveCarImagesAddImageUrls.sql
```

4. **Build and Run**
```bash
dotnet build
dotnet run --project Presentation
```

The API will be available at: `https://localhost:7XXX` (check console output)

## 📡 API Endpoints

### Authentication
```
POST   /api/account/register          # Register new user
POST   /api/account/login             # Login and get JWT token
```

### Cars
```
GET    /api/car                       # Get all cars (Vendors see only their cars)
GET    /api/car/{id}                  # Get car details
POST   /api/car                       # Create car with images (Admin, Vendor)
PUT    /api/car/{id}                  # Update car (Admin, Vendor - own cars only)
DELETE /api/car/{id}                  # Delete car (Admin, Vendor - own cars only)
```

### Favorites
```
GET    /api/favorite                  # Get customer's favorites (Customer)
POST   /api/favorite                  # Add car to favorites (Customer)
DELETE /api/favorite/{carId}          # Remove from favorites (Customer)
```

### Categories
```
GET    /api/make                      # Get all car makes
GET    /api/model                     # Get all car models
GET    /api/bodytype                  # Get all body types
GET    /api/fueltype                  # Get all fuel types
GET    /api/location                  # Get all locations
```

## 🖼️ Image Upload Example

### Create Car with Images (Multipart Form Data)

```javascript
const formData = new FormData();
formData.append('Year', 2024);
formData.append('Price', 25000);
formData.append('Description', 'Beautiful car');
formData.append('MakeId', 1);
formData.append('ModelId', 5);
formData.append('BodyTypeId', 2);
formData.append('FuelId', 1);
formData.append('LocId', 3);

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
// ... add car fields

// Add new images
newImageFiles.forEach(file => formData.append('newImages', file));

// Delete existing images
const imagesToDelete = ['images/cars/old-image-1.jpg'];
formData.append('imagesToDeleteJson', JSON.stringify(imagesToDelete));

await fetch(`/api/car/${carId}`, {
    method: 'PUT',
    headers: { 'Authorization': `Bearer ${token}` },
    body: formData
});
```

## 🔐 Authentication

The API uses JWT Bearer tokens. Include the token in the Authorization header:

```
Authorization: Bearer YOUR_JWT_TOKEN
```

### Sample Login Response
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "user@example.com",
  "roles": ["Vendor"]
}
```

## 📊 Database Schema

### Main Entities
- **Cars**: Vehicle listings with multiple images (JSON)
- **Makes**: Car manufacturers
- **Models**: Car models
- **BodyTypes**: Vehicle body types
- **FuelTypes**: Fuel types
- **LocationCities**: Available locations
- **Favorites**: Customer favorites
- **Users**: Admin, Vendor, Customer accounts

## ✅ Validation Rules

### Car Entity
- **Year**: 1900-2100
- **Price**: > 0
- **Description**: Max 2048 characters
- **Images**: Max 5MB per file, formats: jpg, jpeg, png, gif, webp

### Required Fields
All foreign keys (MakeId, ModelId, BodyTypeId, FuelId, LocId) are required.

## 🛡️ Security Features

- JWT token authentication
- Role-based authorization
- Password hashing with ASP.NET Core Identity
- CORS configuration
- File upload validation (size, type, MIME)

## 📝 Response Format

### Success Response
```json
{
  "carId": "abc-123",
  "year": 2024,
  "price": 25000,
  "description": "Beautiful car",
  "makeId": 1,
  "modelId": 5,
  "imageUrls": [
    "images/cars/guid1.jpg",
    "images/cars/guid2.jpg"
  ],
  "createdDate": "2025-11-27T20:00:00Z"
}
```

### Error Response
```json
{
  "message": "You can only update your own cars"
}
```

## 🔧 Configuration

### JWT Settings (appsettings.json)
```json
{
  "JwtSettings": {
    "SecretKey": "YourSecretKey_MinimumLength32Characters!",
    "Issuer": "CarNestAPI",
    "Audience": "CarNestClient",
    "ExpiryMinutes": 60
  }
}
```

## 📦 Dependencies

- ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

## 👥 Authors

- **Mostafa G. Zain** - Initial work
- **Amir Abdelhak** - Repository maintainer

## 🙏 Acknowledgments

- Built with Clean Architecture principles
- Follows RESTful API best practices
- Implements SOLID principles

---

**Note**: Remember to run the database migration script before first use!

For detailed API documentation, visit `/swagger` when running the application.
