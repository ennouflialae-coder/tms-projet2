# Person A - Database & Models Setup Guide

## ✅ Deliverables Completed

### 1. **Entity Models** (7 classes)
- ✓ `Transporteur.cs` - Transport company
- ✓ `Camion.cs` - Trucks/vehicles
- ✓ `Chauffeur.cs` - Drivers
- ✓ `Client.cs` - Delivery clients
- ✓ `Tournee.cs` - Routes/tours
- ✓ `Livraison.cs` - Deliveries
- ✓ `Cout.cs` - Costs tracking

### 2. **DbContext**
- ✓ `TmsDbContext.cs` - Entity Framework Core context with:
  - All DbSet configurations
  - Relationships (Foreign Keys) defined
  - Data validation (MaxLength, Required)
  - Cascade delete policies

### 3. **Configuration**
- ✓ `Program.cs` - DbContext dependency injection
- ✓ `appsettings.json` - SQL Server connection string
- ✓ NuGet packages added:
  - `Microsoft.EntityFrameworkCore.SqlServer` v10.0.0
  - `Microsoft.EntityFrameworkCore.Tools` v10.0.0

### 4. **Database Script**
- ✓ `Database/TMS_Database_Script.sql` - Complete SQL Server setup with:
  - All 7 tables with proper constraints
  - Foreign keys with cascade rules
  - Performance indexes
  - Sample data (4 transporters, 4 trucks, 4 drivers, 4 clients, 4 routes)
  - Useful views for reporting

---

## 🔧 Setup Instructions

### Step 1: Update Connection String (if needed)
Edit `appsettings.json` if your SQL Server is on a different server:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=TMSDatabase;Trusted_Connection=true;Encrypt=false;"
}
```

### Step 2: Create Database
**Option A: Using SQL Server Management Studio (Recommended)**
1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Open and execute: `Database/TMS_Database_Script.sql`
4. Database `TMSDatabase` will be created with all tables and sample data

**Option B: Using Entity Framework Migrations (Alternative)**
```powershell
# In Package Manager Console
Add-Migration InitialCreate
Update-Database
```

### Step 3: Verify Setup
Build the project:
```powershell
dotnet build
```

---

## 📋 Database Schema

### Tables & Relationships

```
Transporteur (1) ──────────→ (N) Camion
    │
    ├──────────────→ (N) Chauffeur
    │
    └──────────────→ (N) Tournee ──────────→ (N) Livraison ──→ Client
                        │
                        └──────────→ (N) Cout
```

### Key Relationships:
- **Transporteur → Camion**: One transporter has many trucks
- **Transporteur → Chauffeur**: One transporter has many drivers
- **Transporteur → Tournee**: One transporter has many routes
- **Camion → Tournee**: One truck used in many routes
- **Chauffeur → Tournee**: One driver handles many routes
- **Tournee → Livraison**: One route has many deliveries
- **Tournee → Cout**: One route has multiple costs
- **Client → Livraison**: One client can have multiple deliveries

---

## 📊 Sample Data Included

### Transporteurs:
1. **Express Transport** - Paris
2. **Logistique Plus** - Lyon
3. **Transco Solutions** - Marseille

### Camions:
- 4 trucks with different capacities (18,000 - 25,000 kg)
- All active and assigned to transporters

### Chauffeurs:
- 4 drivers with licenses and hire dates

### Clients:
- 4 clients (supermarkets and stores)

### Tournees & Livraisons:
- 4 routes with various statuses
- 5 deliveries with tracking data

### Couts:
- 8 cost entries (fuel, tolls, maintenance)

---

## 🚀 Next Steps for Person B, C, D

**Your models are ready!** The other team members can now:

- **Person B**: Create CRUD pages for Transporteurs, Camions, Chauffeurs
  - Use the models and DbContext directly in Razor Pages
  
- **Person C**: Build the route and delivery assignment logic
  - Access data via `TmsDbContext`
  - Implement automatic cost calculations
  
- **Person D**: Create Dashboard and cost reporting
  - Query views: `vw_CoutParTransporteur` and `vw_TourneeActives`
  - Build admin interface

---

## 🔍 Testing the Setup

### Query the database in Razor Page:
```csharp
public class IndexModel : PageModel
{
    private readonly TmsDbContext _context;
    
    public IndexModel(TmsDbContext context)
    {
        _context = context;
    }
    
    public void OnGet()
    {
        var transporters = _context.Transporteurs.ToList();
        var tours = _context.Tournees.Include(t => t.Chauffeur).ToList();
    }
}
```

---

## ⚠️ Important Notes

1. **Connection String**: Update if you're using SQL Server Authentication or a remote server
2. **Database Name**: The script creates `TMSDatabase`. Change if needed in the SQL script.
3. **Sample Data**: Can be deleted/modified for production
4. **Cascade Deletes**: Configured appropriately (Transporteur cascades to Camion/Chauffeur, but Tournee restricts)

---

## 📝 File Structure
```
TMS Project/
├── Models/
│   ├── Transporteur.cs
│   ├── Camion.cs
│   ├── Chauffeur.cs
│   ├── Client.cs
│   ├── Tournee.cs
│   ├── Livraison.cs
│   └── Cout.cs
├── Data/
│   └── TmsDbContext.cs
├── Database/
│   └── TMS_Database_Script.sql
├── Program.cs
├── appsettings.json
└── TMS Project.csproj
```

---

## ✨ Database Ready!

**Your foundation is solid. All models, relationships, and sample data are in place. Person B, C, and D can now build the business logic on top!** 🎉
