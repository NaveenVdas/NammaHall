# 🔗 How to Link Database to Web Service in Render

## Problem
You don't see `DATABASE_URL` as a "Linked Resource" - this means the database isn't linked to your web service yet.

---

## ✅ Solution: Link Database to Web Service

### Step 1: Go to Web Service Environment Tab

1. **In Render Dashboard**
   - Go to your **Web Service** (not the database)
   - Click on **"Environment"** tab (left sidebar)

### Step 2: Link the Database

1. **Scroll down** in the Environment tab
2. Look for one of these options:
   - **"Link Resource"** button
   - **"Add Environment Variable"** → Then look for **"Link Resource"** option
   - **"Link Database"** or similar option

3. **Click "Link Resource"** (or similar)
   - A modal/popup will appear
   - It will show available resources (your PostgreSQL database)
   - Select your PostgreSQL database from the list
   - Click **"Link"** or **"Save"**

### Step 3: Verify Link

After linking, you should see:
- `DATABASE_URL` appears in the environment variables list
- It might show as "Linked" or have a special indicator
- The value will be hidden/encrypted (this is normal)

### Step 4: Wait for Auto-Redeploy

- Render will automatically redeploy your service
- Wait 2-3 minutes for deployment to complete
- Then try `/api/halls` again

---

## 🔍 Alternative: Manual Setup (If Link Option Not Available)

If you don't see a "Link Resource" option, you can manually add `DATABASE_URL`:

### Step 1: Get Database Connection String

1. **Go to your PostgreSQL Database** dashboard (not web service)
2. Go to **"Info"** or **"Connections"** tab
3. Find **"Internal Database URL"** (for linking) or **"Connection String"**
4. Copy the connection string
   - Format: `postgresql://user:password@host:port/database`

### Step 2: Add as Environment Variable

1. **Back in Web Service** → **"Environment"** tab
2. Click **"Add Environment Variable"**
3. **Key**: `DATABASE_URL`
4. **Value**: Paste the connection string you copied
5. Click **"Save Changes"**

### Step 3: Redeploy

- Service will auto-redeploy
- Wait for deployment to complete

---

## 📋 Step-by-Step Visual Guide

### Option A: Using Link Resource (Recommended)

1. Web Service Dashboard → **"Environment"** tab
2. Scroll down → Look for **"Link Resource"** or **"Add Resource"**
3. Click it → Select your PostgreSQL database
4. Save → Wait for redeploy

### Option B: Manual Entry

1. Database Dashboard → **"Info"** tab → Copy **"Internal Database URL"**
2. Web Service Dashboard → **"Environment"** tab
3. **"Add Environment Variable"**
4. Key: `DATABASE_URL`
5. Value: Paste the connection string
6. Save → Wait for redeploy

---

## 🎯 What You Should See After Linking

In the Environment tab, you should see:
- ✅ `DATABASE_URL` (value hidden, shows as linked)
- ✅ `DatabaseProvider` = `PostgreSQL`
- ✅ `AllowedOrigins` = (your frontend URLs)
- ✅ `ASPNETCORE_ENVIRONMENT` = `Production` (optional)

---

## ⚠️ Important Notes

1. **Use Internal Database URL** for linking (not external)
   - Internal: `postgresql://user:pass@dpg-xxxxx-a:5432/db` (no domain)
   - External: `postgresql://user:pass@dpg-xxxxx-a.oregon-postgres.render.com:5432/db`

2. **Internal URL is better** - Faster, more secure, works within Render's network

3. **After linking/adding** - Service will auto-redeploy (wait 2-3 minutes)

---

## 🔍 If You Still Can't Find Link Option

Render's UI might vary. Try:

1. **Check "Settings" tab** - Sometimes linking is there
2. **Look for "Resources" or "Connected Resources"** section
3. **Check database dashboard** - Sometimes you link from there
4. **Use manual method** - Just add `DATABASE_URL` manually as environment variable

---

**Try the "Link Resource" option first - it's the easiest!** Once linked, your API should be able to connect to the database. 🚀

