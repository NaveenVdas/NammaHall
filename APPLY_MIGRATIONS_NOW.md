# 🚀 Apply Migrations to Render Database - Quick Guide

## Problem
The API is returning: `relation "Halls" does not exist` - this means migrations haven't been applied yet.

---

## ✅ Solution: Run Migrations from Local PowerShell

Since Render Shell requires a subscription, we'll run migrations from your local machine using Render's external database URL.

---

## 📋 Step-by-Step

### Step 1: Get External Database URL from Render

1. Go to Render Dashboard: https://dashboard.render.com
2. Click on your **PostgreSQL database** (not the web service)
3. Go to **"Info"** or **"Connections"** tab
4. Find **"External Database URL"** or **"Connection String"**
5. Copy the entire string - it should look like:
   ```
   postgresql://user:password@dpg-xxxxx-a.oregon-postgres.render.com:5432/database
   ```

**Important**: Use the **External** URL (has `.render.com`), not the internal one!

---

### Step 2: Pull Latest Code (If Needed)

Make sure you have the latest code with the fixed migrations:

```powershell
cd "C:\Namma Hall"
git pull origin develop
```

---

### Step 3: Run Migrations

Open PowerShell and run:

```powershell
cd "C:\Namma Hall\api"

# Set environment variables (REPLACE WITH YOUR ACTUAL CONNECTION STRING FROM RENDER)
$env:DatabaseProvider="PostgreSQL"
$env:DATABASE_URL="postgresql://user:password@dpg-xxxxx-a.oregon-postgres.render.com:5432/database"

# Apply migrations
dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
```

---

### Step 4: Verify Success

You should see:
```
Build started...
Build succeeded.
Applying migration '20251201112831_InitialCreate'...
Applying migration '20251201112851_SeedData'...
Done.
```

---

### Step 5: Test API

After migrations complete, test your API:
- Visit: `https://nammahall-api.onrender.com/api/halls`
- Should return data (or empty array if no halls)

---

## 🔍 Troubleshooting

### Error: "No such host is known"
- **Problem**: Using internal database URL
- **Solution**: Make sure you're using the **External Database URL** from Render

### Error: "Connection timeout"
- **Problem**: Network/firewall issue
- **Solution**: 
  - Check your internet connection
  - Try using Connection Pooling URL if available
  - Verify the database is running in Render

### Error: "Authentication failed"
- **Problem**: Wrong credentials
- **Solution**: Double-check the connection string from Render

### Error: "Pending model changes"
- **Problem**: Code mismatch
- **Solution**: Make sure you pulled the latest code with the fixes

---

## ✅ Complete Script

Copy and paste this (replace connection string):

```powershell
cd "C:\Namma Hall\api"

# Set PostgreSQL environment (REPLACE WITH YOUR ACTUAL CONNECTION STRING)
$env:DatabaseProvider="PostgreSQL"
$env:DATABASE_URL="postgresql://user:password@dpg-xxxxx-a.oregon-postgres.render.com:5432/database"

# Apply migrations
Write-Host "Applying migrations to Render database..." -ForegroundColor Green
dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Migrations completed successfully!" -ForegroundColor Green
    Write-Host "You can now test: https://nammahall-api.onrender.com/api/halls" -ForegroundColor Cyan
} else {
    Write-Host "❌ Migrations failed. Check errors above." -ForegroundColor Red
}
```

---

## 🎯 After Migrations Succeed

1. **Test API Endpoint**: `https://nammahall-api.onrender.com/api/halls`
2. **Check Health**: `https://nammahall-api.onrender.com/healthz`
3. **Verify Data**: If seed data was applied, you should see halls in the response

---

**Run the script above with your Render External Database URL!** 🚀

