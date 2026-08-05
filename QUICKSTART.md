# Quick Start Guide - Admin Portal API

## 5-Minute Setup

### 1. Update Connection String (Optional)
If you're not using default local SQL Server, edit `Admin-Web-API/appsettings.json`:

```json
"ConnectionStrings": {
  "AdminPortalConnection": "Server=YOUR_SERVER;Database=AdminPortalDb;Trusted_Connection=true;TrustServerCertificate=true;Encrypt=false;"
}
```

### 2. Restore & Run
```bash
# From project root
dotnet restore
dotnet build
cd Admin-Web-API
dotnet run
```

### 3. Access API
- **Swagger UI:** https://localhost:5001/swagger
- **API Base URL:** https://localhost:5001/api/auth

Database is created automatically on first run!

---

## API Quick Reference

### Register
```bash
POST /api/auth/register
Content-Type: application/json

{
  "username": "admin",
  "email": "admin@example.com",
  "password": "Secure123",
  "role": 1
}
```

### Login
```bash
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Secure123"
}
```

---

## Database Query Examples

### Connect to Database
1. Open SQL Server Management Studio (SSMS)
2. Connect to: `.` (local server)
3. Database: `AdminPortalDb`

### View All Users
```sql
SELECT * FROM AdminUsers
```

### View Specific User
```sql
SELECT * FROM AdminUsers WHERE Username = 'admin'
```

### Delete User
```sql
DELETE FROM AdminUsers WHERE Username = 'admin'
```

### Update User Status
```sql
UPDATE AdminUsers SET IsActive = 0 WHERE Username = 'admin'
```

---

## Role IDs Reference
- `1` = SuperAdmin
- `2` = Admin
- `3` = Manager
- `4` = User (default)

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Cannot connect to database | Verify SQL Server is running & check connection string |
| Database already exists | Delete database from SSMS and restart app |
| Username already exists | Use different username or delete the record from database |
| HTTPS certificate error | Add `-k` flag to cURL or disable HTTPS validation in Postman |

---

## Project Structure Summary

```
Core Layer
  ?? Entities (AdminUser)
  ?? Enums (UserRole)
  ?? Interfaces (IAdminUserRepository)

Contracts Layer
  ?? DTOs (RegisterRequest, LoginRequest, AdminUserDto)
  ?? Responses (RegisterResponse, LoginResponse)

Infrastructure Layer
  ?? DbContext (AdminPortalDbContext)
  ?? Repositories (AdminUserRepository)
  ?? Migrations (Database schema)

Application Layer
  ?? Services (AdminAuthService)
  ?? Business Logic & Validation

Shared Layer
  ?? Utilities (PasswordHasher)

API Layer
  ?? Controllers (AuthController)
  ?? Configuration (Program.cs)
```

For detailed documentation, see `SETUP_GUIDE.md`
