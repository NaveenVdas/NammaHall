# 🚀 Step-by-Step: Deploy Backend to Render

This is a simplified guide specifically for deploying your backend API to Render.

---

## 📋 Prerequisites Checklist

- [x] Render account created and signed in
- [ ] GitHub repository: `NaveenVdas/NammaHall` (already pushed)
- [ ] Both UI and API are in the same repository

---

## 🗄️ Step 1: Create PostgreSQL Database

1. **Go to Render Dashboard**
   - Visit: https://dashboard.render.com
   - You should see your dashboard

2. **Create New PostgreSQL Database**
   - Click the **"+ New +"** button (top right)
   - Select **"PostgreSQL"** from the dropdown

3. **Configure Database Settings**
   - **Name**: `nammahall-db` (or any name you prefer)
   - **Database**: `nammahall` (or any name)
   - **User**: `nammahall` (or any name)
   - **Region**: Choose the closest region to your users (e.g., `Oregon (US West)` or `Singapore (Asia Pacific)`)
   - **PostgreSQL Version**: Leave default (usually 15 or 16)
   - **Plan**: Select **"Free"** (1 GB storage - good for MVP)
   - **Datadog API Key**: Leave empty (optional monitoring)

4. **Create Database**
   - Click **"Create Database"** button
   - Wait 1-2 minutes for the database to be provisioned
   - **Important**: Once created, you'll see the database dashboard
   - **Note the Internal Database URL** - it looks like: `postgresql://user:password@host:port/database`
   - You don't need to copy it manually - Render will link it automatically

---

## 🌐 Step 2: Create Web Service (API)

1. **Create New Web Service**
   - In Render dashboard, click **"+ New +"** button again
   - Select **"Web Service"** from the dropdown

2. **Connect GitHub Repository**
   - If this is your first time, click **"Connect account"** next to GitHub
   - Authorize Render to access your GitHub repositories
   - Select your repository: **`NaveenVdas/NammaHall`**
   - Click **"Connect"**

3. **Configure Web Service Settings**

   **Basic Settings:**
   - **Name**: `nammahall-api` (or any name you prefer)
   - **Region**: **Same region as your database** (important for performance)
   - **Branch**: `develop` (or `main` if that's your main branch)
   - **Root Directory**: **`api`** ⚠️ **IMPORTANT** - This tells Render to look in the `api` folder
   - **Runtime**: Leave as default or select **"Docker"**
   - **Environment**: Leave as **"Docker"** (since we have a Dockerfile)

   **Build & Deploy Settings:**
   - **Build Command**: Leave **empty** (Docker handles this)
   - **Start Command**: Leave **empty** (Dockerfile ENTRYPOINT handles this)

   **Advanced Settings (click "Advanced" to expand):**
   - **Dockerfile Path**: `Dockerfile` (or `api/Dockerfile` if root directory is not set to `api`)
   - **Docker Context**: `.` (current directory, which will be `api` if root directory is set correctly)

4. **Create Web Service**
   - Click **"Create Web Service"** button
   - Render will start building your Docker image
   - This will take 3-5 minutes the first time

---

## 🔗 Step 3: Link Database to Web Service

1. **In Your Web Service Dashboard**
   - After the service is created, you'll see the service dashboard
   - Go to the **"Environment"** tab (left sidebar)

2. **Link Database**
   - Scroll down to find **"Add Environment Variable"** section
   - Look for a button/link that says **"Link Resource"** or **"Link Database"**
   - Click on it
   - A modal will appear showing available databases
   - Select your PostgreSQL database: **`nammahall-db`** (or whatever you named it)
   - Click **"Link"**

3. **Verify Database Link**
   - After linking, you should see a new environment variable added:
     - **Name**: `DATABASE_URL`
     - **Value**: `postgresql://user:password@host:port/database` (hidden/encrypted)
   - This is automatically set by Render - you don't need to configure it manually!

---

## ⚙️ Step 4: Configure Environment Variables

Still in the **"Environment"** tab of your Web Service:

1. **Add DatabaseProvider**
   - Click **"Add Environment Variable"**
   - **Key**: `DatabaseProvider`
   - **Value**: `PostgreSQL`
   - Click **"Save Changes"**

2. **Add AllowedOrigins** (Temporary - you'll update this after deploying frontend)
   - Click **"Add Environment Variable"** again
   - **Key**: `AllowedOrigins`
   - **Value**: `http://localhost:5173,http://localhost:3000` (temporary for testing)
   - **Note**: You'll update this later with your Vercel/Netlify URL
   - Click **"Save Changes"**

3. **Add ASPNETCORE_ENVIRONMENT** (Optional but recommended)
   - Click **"Add Environment Variable"** again
   - **Key**: `ASPNETCORE_ENVIRONMENT`
   - **Value**: `Production`
   - Click **"Save Changes"**

4. **Verify All Variables**
   You should now have these environment variables:
   - ✅ `DATABASE_URL` (automatically added when you linked database)
   - ✅ `DatabaseProvider` = `PostgreSQL`
   - ✅ `AllowedOrigins` = `http://localhost:5173,http://localhost:3000`
   - ✅ `ASPNETCORE_ENVIRONMENT` = `Production` (optional)

---

## 🚀 Step 5: Wait for First Deploy

1. **Monitor Build Process**
   - Go to the **"Events"** or **"Logs"** tab in your Web Service dashboard
   - You'll see the build progress
   - First build takes 3-5 minutes

2. **Check for Errors**
   - If build fails, check the logs for errors
   - Common issues:
     - Wrong root directory (should be `api`)
     - Wrong Dockerfile path
     - Build timeout (rare)

3. **Wait for "Live" Status**
   - Once build completes, service will deploy
   - Status will change to **"Live"** when ready

---

## 🗄️ Step 6: Run Database Migrations

Once your service is live, you need to run migrations to create the database schema.

### Option A: Using Render Shell (Easiest)

1. **Open Shell**
   - In your Web Service dashboard, click the **"Shell"** tab (left sidebar)
   - Wait for the shell to connect (may take 10-20 seconds)

2. **Navigate to API Directory**
   ```bash
   cd /opt/render/project/src/api
   ```
   (Render mounts your code at `/opt/render/project/src`)

3. **Check if EF Core Tools are Available**
   ```bash
   dotnet ef --version
   ```
   If this fails, you'll need to install EF Core tools first:
   ```bash
   dotnet tool install --global dotnet-ef
   export PATH="$PATH:/root/.dotnet/tools"
   ```

4. **Run Migrations**
   ```bash
   dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
   ```

5. **Verify Success**
   - You should see messages like:
     - "Applying migration 'InitialCreate'..."
     - "Applying migration 'SeedData'..."
     - "Done."
   - If you see errors, check the connection string and database link

### Option B: Using Local Machine (Alternative)

If the shell method doesn't work, you can run migrations from your local machine:

1. **Get Database Connection String**
   - In Render dashboard, go to your **PostgreSQL database** dashboard
   - Go to **"Info"** or **"Connections"** tab
   - Copy the **"Internal Database URL"** or **"Connection String"**
   - It looks like: `postgresql://user:password@host:port/database`

2. **Run Migrations Locally**
   - Open terminal on your local machine
   - Navigate to your project:
     ```bash
     cd "C:\Namma Hall\api"
     ```
   - Set the connection string temporarily:
     ```bash
     # PowerShell
     $env:DATABASE_URL="postgresql://user:password@host:port/database"
     $env:DatabaseProvider="PostgreSQL"
     ```
   - Run migrations:
     ```bash
     dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
     ```

---

## ✅ Step 7: Verify Deployment

1. **Get Your API URL**
   - In your Web Service dashboard, you'll see the URL at the top
   - It looks like: `https://nammahall-api.onrender.com`
   - **Copy this URL** - you'll need it for frontend configuration

2. **Test Health Endpoint**
   - Open a new browser tab
   - Visit: `https://your-api-url.onrender.com/healthz`
   - Should return: `Healthy` or similar

3. **Test API Endpoint**
   - Visit: `https://your-api-url.onrender.com/api/halls`
   - Should return JSON data (may be empty array if no data, or error if migrations not run)
   - If you see data, migrations worked! ✅

4. **Check Logs**
   - Go to **"Logs"** tab in your Web Service dashboard
   - Look for any errors or warnings
   - Should see normal startup messages

---

## 🎯 Step 8: Update CORS (After Frontend Deploy)

Once you deploy your frontend to Vercel/Netlify:

1. **Get Frontend URL**
   - Copy your Vercel URL: `https://your-app.vercel.app`
   - Or Netlify URL: `https://your-app.netlify.app`

2. **Update AllowedOrigins**
   - Go back to Render Web Service → **"Environment"** tab
   - Find `AllowedOrigins` variable
   - Click **"Edit"** or the pencil icon
   - Update value to: `https://your-app.vercel.app,https://your-app-git-main.vercel.app`
   - (Add both the main URL and preview URLs)
   - Click **"Save Changes"**
   - Render will automatically redeploy with new CORS settings

---

## 🐛 Troubleshooting

### Build Fails

**Problem**: Build fails with "Dockerfile not found"
- **Solution**: Check that "Root Directory" is set to `api` in service settings

**Problem**: Build fails with timeout
- **Solution**: Free tier has build time limits. Try again or check Dockerfile for inefficiencies

### Service Won't Start

**Problem**: Service shows "Failed" status
- **Solution**: 
  1. Check **"Logs"** tab for error messages
  2. Verify all environment variables are set correctly
  3. Verify database is linked

### Database Connection Errors

**Problem**: API can't connect to database
- **Solution**:
  1. Verify database is linked in "Environment" tab
  2. Check `DATABASE_URL` exists
  3. Verify `DatabaseProvider` is set to `PostgreSQL`
  4. Check database is running (should show "Available" status)

### Migrations Fail

**Problem**: Migrations fail in shell
- **Solution**:
  1. Verify you're in the correct directory: `/opt/render/project/src/api`
  2. Check EF Core tools are installed: `dotnet tool install --global dotnet-ef`
  3. Verify `DATABASE_URL` is accessible
  4. Try running migrations from local machine with Render's connection string

### Slow First Request

**Problem**: First API request takes 30-60 seconds
- **Solution**: This is normal for Render free tier - services sleep after 15 minutes of inactivity
- First request wakes up the service (takes time)
- Subsequent requests are fast
- Consider upgrading to paid plan for always-on service

---

## 📝 Quick Reference

### Your Render URLs:
- **API URL**: `https://nammahall-api.onrender.com` (or your custom name)
- **API Base**: `https://nammahall-api.onrender.com/api`
- **Health Check**: `https://nammahall-api.onrender.com/healthz`

### Environment Variables Summary:
```
DATABASE_URL = (auto-set by Render when database is linked)
DatabaseProvider = PostgreSQL
AllowedOrigins = https://your-frontend-url.vercel.app
ASPNETCORE_ENVIRONMENT = Production
```

---

## ✅ Deployment Checklist

- [ ] PostgreSQL database created
- [ ] Web service created with root directory = `api`
- [ ] Database linked to web service
- [ ] Environment variables configured
- [ ] First deployment successful (status = "Live")
- [ ] Database migrations run successfully
- [ ] Health endpoint works (`/healthz`)
- [ ] API endpoint works (`/api/halls`)
- [ ] CORS updated with frontend URL (after frontend deploy)

---

**Next Step**: Once backend is deployed and working, deploy your frontend to Vercel or Netlify using the main `DEPLOYMENT.md` guide!

