# 🗄️ Connect to Render PostgreSQL Database from Database Editor

## ⚠️ Important Note
**SSMS (SQL Server Management Studio) is for SQL Server only!**  
You need a **PostgreSQL client** to connect to your Render PostgreSQL database.

---

## 🛠️ Recommended PostgreSQL Database Editors

### Option 1: DBeaver (Free, Cross-Platform) ⭐ Recommended

**Download**: https://dbeaver.io/download/

**Why it's great:**
- Free and open source
- Works on Windows, Mac, Linux
- Supports PostgreSQL, MySQL, SQL Server, and more
- Easy to use interface

**How to Connect:**

1. **Download and Install DBeaver**
   - Go to https://dbeaver.io/download/
   - Download Community Edition (free)
   - Install it

2. **Create New Connection**
   - Open DBeaver
   - Click **"New Database Connection"** (plug icon)
   - Select **"PostgreSQL"**
   - Click **"Next"**

3. **Enter Connection Details**
   - **Host**: `dpg-d4mh91q4d50c73ele0a0-a.oregon-postgres.render.com`
   - **Port**: `5432`
   - **Database**: `nammahall`
   - **Username**: `nammahall`
   - **Password**: `4xFxqCNElivJFBrr6vOyeFN0iEaYyQF8`
   - **Show all databases**: Unchecked (optional)

4. **SSL Settings** (Important!)
   - Go to **"SSL"** tab
   - Check **"Use SSL"**
   - SSL Mode: **"require"** or **"allow"**
   - Check **"Allow invalid hostname"** (if needed)

5. **Test Connection**
   - Click **"Test Connection"**
   - If it asks to download drivers, click **"Download"**
   - Should show "Connected"

6. **Connect**
   - Click **"Finish"**
   - You should now see your database and tables!

---

### Option 2: pgAdmin (Official PostgreSQL Tool)

**Download**: https://www.pgadmin.org/download/

**How to Connect:**

1. **Install pgAdmin**
   - Download from https://www.pgadmin.org/download/
   - Install it

2. **Create New Server**
   - Right-click **"Servers"** → **"Register"** → **"Server"**
   - **General Tab**:
     - Name: `Render NammaHall DB`
   - **Connection Tab**:
     - Host: `dpg-d4mh91q4d50c73ele0a0-a.oregon-postgres.render.com`
     - Port: `5432`
     - Database: `nammahall`
     - Username: `nammahall`
     - Password: `4xFxqCNElivJFBrr6vOyeFN0iEaYyQF8`
   - **SSL Tab**:
     - SSL Mode: **"Require"**

3. **Save and Connect**
   - Click **"Save"**
   - Expand the server to see your database

---

### Option 3: Azure Data Studio (Free, Microsoft)

**Download**: https://aka.ms/azuredatastudio

**Note**: Requires PostgreSQL extension

**How to Connect:**

1. **Install Azure Data Studio**
2. **Install PostgreSQL Extension**
   - Click Extensions icon
   - Search for "PostgreSQL"
   - Install it
3. **Create Connection**
   - Click "New Connection"
   - Select "PostgreSQL"
   - Enter connection details (same as above)

---

### Option 4: VS Code Extension

**If you use VS Code:**

1. **Install Extension**
   - Search for "PostgreSQL" in VS Code Extensions
   - Install "PostgreSQL" by Chris Kolkman or similar

2. **Connect**
   - Use the connection details below

---

## 📋 Connection Details (Use These)

From your Render External Database URL:
```
postgresql://nammahall:4xFxqCNElivJFBrr6vOyeFN0iEaYyQF8@dpg-d4mh91q4d50c73ele0a0-a.oregon-postgres.render.com/nammahall
```

**Parsed Details:**
- **Host**: `dpg-d4mh91q4d50c73ele0a0-a.oregon-postgres.render.com`
- **Port**: `5432`
- **Database**: `nammahall`
- **Username**: `nammahall`
- **Password**: `4xFxqCNElivJFBrr6vOyeFN0iEaYyQF8`
- **SSL**: Required

---

## ✅ What You Can Do After Connecting

1. **View Tables**
   - Expand database → Schemas → public → Tables
   - You should see: `Halls`, `Bookings`, `HallImages`, `AdminUsers`

2. **View Data**
   - Right-click a table → "View Data" or "Select Top 1000 Rows"
   - See if seed data was applied

3. **Run Queries**
   - Open SQL editor
   - Run: `SELECT * FROM "Halls";`
   - Should show your halls (if seed data was applied)

4. **Verify Migrations**
   - Check `__EFMigrationsHistory` table
   - Should show: `20251201112831_InitialCreate` and `20251201112851_SeedData`

---

## 🔍 Quick Verification Queries

Once connected, run these to verify:

```sql
-- Check if tables exist
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public';

-- Check migrations
SELECT * FROM "__EFMigrationsHistory";

-- Check halls data
SELECT * FROM "Halls";

-- Check admin user
SELECT * FROM "AdminUsers";
```

---

## 🎯 Recommended: Use DBeaver

**DBeaver is the easiest and most user-friendly option:**
- Free
- Works great with PostgreSQL
- Easy connection setup
- Good query editor
- Can export/import data

**Download**: https://dbeaver.io/download/

---

## ⚠️ Security Note

- **Don't share your connection string publicly**
- **Don't commit passwords to git**
- **Use environment variables in production** (which you're already doing)

---

**Try DBeaver - it's the easiest way to connect and verify your database!** 🚀

