# 🔧 Fix: Database Migration Connection Error

## Problem
You're trying to connect to Render's **internal** database URL from your local machine. Internal URLs only work from within Render's network.

**Error**: `No such host is known` for `dpg-d4mh91q4d50c73ele0a0-a`

---

## ✅ Solution 1: Run Migrations from Render Shell (Recommended)

This is the easiest way - run migrations directly on Render where the internal URL works.

### Steps:

1. **Go to Your Web Service Dashboard**
   - In Render, click on your web service

2. **Open Shell**
   - Click **"Shell"** tab (left sidebar)
   - Wait for shell to connect (10-20 seconds)

3. **Navigate to API Directory**
   ```bash
   cd /opt/render/project/src/api
   ```

4. **Install EF Core Tools** (if not already installed)
   ```bash
   dotnet tool install --global dotnet-ef
   export PATH="$PATH:/root/.dotnet/tools"
   ```

5. **Run Migrations**
   ```bash
   dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
   ```

6. **Verify Success**
   - You should see: "Applying migration 'InitialCreate'..."
   - Then: "Applying migration 'SeedData'..."
   - Finally: "Done."

**This should work because you're running from inside Render's network!**

---

## ✅ Solution 2: Use External Database URL (If Running Locally)

If you want to run migrations from your local machine, you need the **external** database URL.

### Steps:

1. **Get External Database URL**
   - Go to your **PostgreSQL database** dashboard in Render
   - Click on **"Info"** or **"Connections"** tab
   - Look for **"External Database URL"** or **"Connection Pooling"**
   - Copy the connection string
   - It should look like: `postgresql://user:password@dpg-xxxxx-a.oregon-postgres.render.com:5432/database`

2. **Run Migrations Locally**
   - Open PowerShell on your local machine
   - Navigate to your project:
     ```powershell
     cd "C:\Namma Hall\api"
     ```
   - Set environment variables:
     ```powershell
     $env:DATABASE_URL="postgresql://user:password@external-host.render.com:5432/database"
     $env:DatabaseProvider="PostgreSQL"
     ```
   - Run migrations:
     ```powershell
     dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
     ```

**Note**: External URLs work from anywhere, but they're slower and may have connection limits.

---

## ✅ Solution 3: Use Connection Pooling URL (Best for Local)

Render provides a connection pooling URL that's better for external connections.

### Steps:

1. **Enable Connection Pooling** (if not already enabled)
   - Go to your PostgreSQL database dashboard
   - Look for **"Connection Pooling"** section
   - Enable it if available
   - Copy the **pooling URL**

2. **Use Pooling URL for Migrations**
   - The pooling URL is designed for external connections
   - Use it the same way as Solution 2

---

## 🎯 Recommended: Use Render Shell (Solution 1)

**Why?**
- ✅ Internal URLs work (faster, more reliable)
- ✅ No need to expose external URLs
- ✅ Same environment as production
- ✅ Easier setup

**Just use the Shell tab in Render!**

---

## 🔍 How to Identify Internal vs External URLs

### Internal URL (Only works from Render):
```
postgresql://user:pass@dpg-xxxxx-a:5432/db
                    ^^^^^^^^^^^^
                    Short hostname, no domain
```

### External URL (Works from anywhere):
```
postgresql://user:pass@dpg-xxxxx-a.oregon-postgres.render.com:5432/db
                    ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                    Full domain name with .render.com
```

### Connection Pooling URL:
```
postgresql://user:pass@dpg-xxxxx-a-pooler.oregon-postgres.render.com:5432/db
                    ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                    Has "-pooler" in the hostname
```

---

## ✅ Quick Fix Right Now

**Just use Render Shell:**

1. Web Service → **"Shell"** tab
2. Run:
   ```bash
   cd /opt/render/project/src/api
   dotnet tool install --global dotnet-ef
   export PATH="$PATH:/root/.dotnet/tools"
   dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
   ```

That's it! This will work because you're inside Render's network.

---

## 🐛 If Shell Method Doesn't Work

If you get errors in the shell:

1. **Check DATABASE_URL is set**
   ```bash
   echo $DATABASE_URL
   ```
   Should show the connection string

2. **Verify Database is Linked**
   - Go to Web Service → **"Environment"** tab
   - Make sure `DATABASE_URL` exists
   - If not, link the database again

3. **Check Database Status**
   - Go to Database dashboard
   - Make sure status is **"Available"**

---

**Try Solution 1 (Render Shell) first - it's the easiest!** 🚀

