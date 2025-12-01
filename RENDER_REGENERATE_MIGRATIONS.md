# 🔧 Fix: Migrations Use SQL Server Types (Need PostgreSQL)

## Problem
The migrations were created for SQL Server (using `nvarchar`, `datetime2`, `bit`) but you're deploying to PostgreSQL (needs `varchar`, `timestamp`, `boolean`).

**Error**: `type "nvarchar" does not exist`

---

## ✅ Solution: Regenerate Migrations for PostgreSQL

We need to delete the old migrations and create new ones for PostgreSQL.

### Step 1: Delete Old Migrations

**⚠️ Important**: Only do this if you haven't deployed to production yet, or if the database is empty.

In PowerShell:

```powershell
cd "C:\Namma Hall\api"

# Delete migration files
Remove-Item "NammaHall.Infrastructure\Migrations\20251130171950_InitialCreate.cs"
Remove-Item "NammaHall.Infrastructure\Migrations\20251130171950_InitialCreate.Designer.cs"
Remove-Item "NammaHall.Infrastructure\Migrations\20251130172259_SeedData.cs"
Remove-Item "NammaHall.Infrastructure\Migrations\20251130172259_SeedData.Designer.cs"
Remove-Item "NammaHall.Infrastructure\Migrations\NammaHallDbContextModelSnapshot.cs"
```

### Step 2: Set Environment for PostgreSQL

```powershell
# Make sure you're using PostgreSQL
$env:DatabaseProvider="PostgreSQL"
$env:DATABASE_URL="postgresql://user:password@dpg-xxxxx-a.oregon-postgres.render.com:5432/database"
```

### Step 3: Generate New Migrations for PostgreSQL

```powershell
# Generate initial migration
dotnet ef migrations add InitialCreate --project NammaHall.Infrastructure --startup-project NammaHall.Api

# Generate seed data migration
dotnet ef migrations add SeedData --project NammaHall.Infrastructure --startup-project NammaHall.Api
```

### Step 4: Apply Migrations

```powershell
dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
```

---

## 🎯 Alternative: Keep Seed Data, Regenerate Schema Only

If you want to keep the seed data migration (since it's provider-aware), you can:

1. **Delete only the InitialCreate migration**
2. **Regenerate it for PostgreSQL**
3. **Keep SeedData migration** (it's already provider-aware)

### Steps:

```powershell
cd "C:\Namma Hall\api"

# Set PostgreSQL environment
$env:DatabaseProvider="PostgreSQL"
$env:DATABASE_URL="postgresql://user:password@dpg-xxxxx-a.oregon-postgres.render.com:5432/database"

# Delete only InitialCreate migration
Remove-Item "NammaHall.Infrastructure\Migrations\20251130171950_InitialCreate.cs"
Remove-Item "NammaHall.Infrastructure\Migrations\20251130171950_InitialCreate.Designer.cs"
Remove-Item "NammaHall.Infrastructure\Migrations\NammaHallDbContextModelSnapshot.cs"

# Regenerate InitialCreate for PostgreSQL
dotnet ef migrations add InitialCreate --project NammaHall.Infrastructure --startup-project NammaHall.Api

# Update SeedData migration to reference new InitialCreate
# (You may need to update the migration name in SeedData if the timestamp changed)

# Apply migrations
dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
```

---

## 🚀 Recommended: Complete Regeneration

Since you're starting fresh with PostgreSQL, the cleanest approach is:

1. Delete all migrations
2. Regenerate for PostgreSQL
3. Apply to database

This ensures everything is PostgreSQL-compatible.

---

## 📝 Complete Script

Here's a complete PowerShell script:

```powershell
cd "C:\Namma Hall\api"

# Set PostgreSQL environment (REPLACE WITH YOUR CONNECTION STRING)
$env:DatabaseProvider="PostgreSQL"
$env:DATABASE_URL="postgresql://user:password@dpg-xxxxx-a.oregon-postgres.render.com:5432/database"

# Delete old migrations
Write-Host "Deleting old migrations..." -ForegroundColor Yellow
Remove-Item "NammaHall.Infrastructure\Migrations\20251130171950_InitialCreate.cs" -ErrorAction SilentlyContinue
Remove-Item "NammaHall.Infrastructure\Migrations\20251130171950_InitialCreate.Designer.cs" -ErrorAction SilentlyContinue
Remove-Item "NammaHall.Infrastructure\Migrations\20251130172259_SeedData.cs" -ErrorAction SilentlyContinue
Remove-Item "NammaHall.Infrastructure\Migrations\20251130172259_SeedData.Designer.cs" -ErrorAction SilentlyContinue
Remove-Item "NammaHall.Infrastructure\Migrations\NammaHallDbContextModelSnapshot.cs" -ErrorAction SilentlyContinue

# Generate new migrations for PostgreSQL
Write-Host "Generating InitialCreate migration for PostgreSQL..." -ForegroundColor Green
dotnet ef migrations add InitialCreate --project NammaHall.Infrastructure --startup-project NammaHall.Api

Write-Host "Generating SeedData migration..." -ForegroundColor Green
dotnet ef migrations add SeedData --project NammaHall.Infrastructure --startup-project NammaHall.Api

# Apply migrations
Write-Host "Applying migrations to database..." -ForegroundColor Green
dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Migrations completed successfully!" -ForegroundColor Green
} else {
    Write-Host "❌ Migrations failed. Check errors above." -ForegroundColor Red
}
```

---

## ⚠️ Important Notes

1. **Backup First**: If you have any data, back it up before deleting migrations
2. **Fresh Database**: This approach is safe for a fresh/empty database
3. **Commit Changes**: After regenerating, commit the new migration files to git
4. **Seed Data**: The SeedData migration is provider-aware, but you may need to update it after regenerating InitialCreate

---

## 🔍 Verify New Migrations

After regenerating, check the migration files:

1. Open `InitialCreate.cs`
2. Look for PostgreSQL types:
   - ✅ `character varying` or `varchar` (not `nvarchar`)
   - ✅ `timestamp` (not `datetime2`)
   - ✅ `boolean` (not `bit`)
   - ✅ `integer` (not `int`)

If you see SQL Server types, the migration wasn't generated with PostgreSQL provider.

---

**Run the script above to regenerate migrations for PostgreSQL!** 🚀

