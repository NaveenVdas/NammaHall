# 🔧 Fix: API Connecting to Different Database

## Problem
The API is connecting to a database, but that database doesn't have the "Halls" table. This means:
- Migrations were applied to Database A (using External URL from local machine)
- API is connecting to Database B (using Internal URL from Render)

---

## ✅ Solution: Verify and Fix Database Connection

### Step 1: Check Which Database API is Using

The API on Render uses the `DATABASE_URL` environment variable. When you link a database, Render provides the **Internal Database URL**.

### Step 2: Verify Migrations Were Applied

You ran migrations using the **External Database URL**. These should be the same database, but let's verify.

### Step 3: Check Database Name

1. **In Render Dashboard**
   - Go to your **PostgreSQL Database** dashboard
   - Check the **database name** (should be `nammahall` or similar)
   - Note it down

2. **Verify Connection String**
   - The External URL you used for migrations should have the same database name
   - Format: `postgresql://user:pass@host:port/database_name`

---

## 🎯 Most Likely Issue

The API and migrations might be using **different databases** or the **migrations weren't actually applied**.

### Quick Fix: Run Migrations Again

Since the API is using the Internal Database URL (from linked resource), let's verify the connection and potentially run migrations again.

**But wait** - you can't use Render Shell. So we need to use the Internal Database URL from your local machine.

---

## 🔍 Step-by-Step Fix

### Option 1: Get Internal Database URL and Run Migrations

1. **Get Internal Database URL from Render**
   - Go to your **PostgreSQL Database** dashboard
   - Go to **"Info"** or **"Connections"** tab
   - Look for **"Internal Database URL"** (not External)
   - Copy it - format: `postgresql://user:pass@dpg-xxxxx-a:5432/database`
   - **Note**: No `.render.com` domain - this is the internal URL

2. **Run Migrations Using Internal URL**
   ```powershell
   cd "C:\Namma Hall\api"
   
   # Use Internal Database URL (from Render database dashboard)
   $env:DatabaseProvider="PostgreSQL"
   $env:DATABASE_URL="postgresql://user:password@dpg-xxxxx-a:5432/database"
   
   # Apply migrations
   dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
   ```

3. **Test API Again**
   - After migrations complete, try `/api/halls` again

---

### Option 2: Verify Same Database

1. **Check Database Name in Both URLs**
   - External URL (used for migrations): What's the database name?
   - Internal URL (used by API): What's the database name?
   - They should match!

2. **If Different Databases**
   - You have two databases
   - Need to run migrations on the one the API is using

---

## 🚀 Quick Action Plan

1. **Get Internal Database URL** from Render database dashboard
2. **Run migrations** using that Internal URL from your local machine
3. **Test API** - should work now

---

## 💡 Why This Happens

- **External URL**: Used from outside Render (your local machine)
- **Internal URL**: Used from within Render (your API service)
- Both should point to the **same database**, but sometimes they can differ
- Or migrations were applied to a different database instance

---

**Get the Internal Database URL from Render and run migrations with it!** 🎯

