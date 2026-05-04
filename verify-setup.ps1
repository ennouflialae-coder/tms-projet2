# TMS Database Verification Script
# Run this in PowerShell to verify all components

Write-Host "🧪 TMS Project Verification" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan

# 1. Check if project file exists
Write-Host "`n1️⃣ Checking Project Structure..."
if (Test-Path "TMS Project.csproj") {
    Write-Host "✅ Project file found" -ForegroundColor Green
} else {
    Write-Host "❌ Project file not found" -ForegroundColor Red
}

# 2. Check Model files
Write-Host "`n2️⃣ Checking Model Files..."
$models = @(
    "Models\Transporteur.cs",
    "Models\Camion.cs",
    "Models\Chauffeur.cs",
    "Models\Client.cs",
    "Models\Tournee.cs",
    "Models\Livraison.cs",
    "Models\Cout.cs"
)

foreach ($model in $models) {
    if (Test-Path $model) {
        Write-Host "✅ $model" -ForegroundColor Green
    } else {
        Write-Host "❌ $model missing" -ForegroundColor Red
    }
}

# 3. Check DbContext
Write-Host "`n3️⃣ Checking DbContext..."
if (Test-Path "Data\TmsDbContext.cs") {
    Write-Host "✅ TmsDbContext.cs found" -ForegroundColor Green
} else {
    Write-Host "❌ TmsDbContext.cs missing" -ForegroundColor Red
}

# 4. Check Configuration
Write-Host "`n4️⃣ Checking Configuration Files..."
if (Test-Path "Program.cs") {
    Write-Host "✅ Program.cs found" -ForegroundColor Green
    # Check if DbContext is registered
    $content = Get-Content "Program.cs" -Raw
    if ($content -match "AddDbContext") {
        Write-Host "✅ DbContext registered in Program.cs" -ForegroundColor Green
    } else {
        Write-Host "⚠️  DbContext might not be registered" -ForegroundColor Yellow
    }
}

if (Test-Path "appsettings.json") {
    Write-Host "✅ appsettings.json found" -ForegroundColor Green
    # Check connection string
    $settings = Get-Content "appsettings.json" -Raw
    if ($settings -match "DefaultConnection") {
        Write-Host "✅ Connection string configured" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Connection string not found" -ForegroundColor Yellow
    }
}

# 5. Check database script
Write-Host "`n5️⃣ Checking Database Script..."
if (Test-Path "Database\TMS_Database_Script.sql") {
    Write-Host "✅ TMS_Database_Script.sql found" -ForegroundColor Green
    $lines = (Get-Content "Database\TMS_Database_Script.sql").Count
    Write-Host "📊 SQL Script size: $lines lines" -ForegroundColor Cyan
} else {
    Write-Host "❌ TMS_Database_Script.sql missing" -ForegroundColor Red
}

# 6. Check Test Page
Write-Host "`n6️⃣ Checking Test Page..."
if (Test-Path "Pages\TestDatabase.cshtml") {
    Write-Host "✅ TestDatabase.cshtml found" -ForegroundColor Green
} else {
    Write-Host "⚠️  TestDatabase.cshtml not found" -ForegroundColor Yellow
}

Write-Host "`n================================" -ForegroundColor Cyan
Write-Host "✨ Verification Complete!" -ForegroundColor Cyan
Write-Host "`n📌 NEXT STEPS:" -ForegroundColor Yellow
Write-Host "1. Create database: Run Database\TMS_Database_Script.sql in SSMS" -ForegroundColor Cyan
Write-Host "2. Build project: dotnet build" -ForegroundColor Cyan
Write-Host "3. Run project: dotnet run" -ForegroundColor Cyan
Write-Host "4. Test: Visit https://localhost:7xxx/TestDatabase" -ForegroundColor Cyan
