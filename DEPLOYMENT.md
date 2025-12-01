# 🚀 Deployment Guide: Render + Vercel/Netlify

This guide walks you through deploying the Namma Hall application to Render (backend + PostgreSQL) and Vercel or Netlify (frontend).

---

## ✅ What's Already Automated

The following has been set up for you:

1. **Dockerfile** - Ready for Render deployment
2. **render.yaml** - Render configuration (optional, for Blueprint deployment)
3. **vercel.json** - Vercel configuration for React app
4. **Environment Variable Support** - Code reads from environment variables
5. **DATABASE_URL Parsing** - Automatically converts Render's DATABASE_URL format
6. **CORS Configuration** - Supports environment-based CORS origins
7. **Provider-Aware Migrations** - Works with both SQL Server and PostgreSQL

---

## 📋 What You Need to Do Manually

### Prerequisites
- [ ] GitHub account
- [ ] Render account (sign up at [render.com](https://render.com))
- [ ] Vercel account (sign up at [vercel.com](https://vercel.com)) OR Netlify account (sign up at [netlify.com](https://netlify.com))
- [ ] Your code pushed to a GitHub repository

---

## 🎨 Part 1: Deploy Backend to Render

### Step 1: Create Render Account and Connect GitHub

1. Go to [render.com](https://render.com) and sign in
2. Click **"New +"** → **"Blueprint"** (for automated setup) OR **"Web Service"** (for manual setup)
3. Connect your GitHub account if not already connected
4. Select your repository: `NaveenVdas/NammaHall`

### Step 2: Create PostgreSQL Database

**Option A: Using Blueprint (Recommended - Easiest)**

1. In Render dashboard, click **"New +"** → **"Blueprint"**
2. Connect your GitHub repository
3. Render will detect `render.yaml` and create both the web service and database automatically
4. Review the services and click **"Apply"**

**Option B: Manual Setup**

1. In Render dashboard, click **"New +"** → **"PostgreSQL"**
2. Configure:
   - **Name**: `nammahall-db` (or any name you prefer)
   - **Database**: `nammahall` (or any name)
   - **User**: `nammahall` (or any name)
   - **Region**: Choose closest to your users
   - **Plan**: **Free** (1 GB storage, good for MVP)
3. Click **"Create Database"**
4. **Important**: Note the **Internal Database URL** - you'll need this later

### Step 3: Create Web Service (API)

**Option A: If you used Blueprint**
- The web service is already created! Skip to Step 4.

**Option B: Manual Setup**

1. In Render dashboard, click **"New +"** → **"Web Service"**
2. Connect your GitHub repository
3. Configure the service:
   - **Name**: `nammahall-api` (or any name)
   - **Region**: Same as your database
   - **Branch**: `develop` (or `main`)
   - **Root Directory**: `api`
   - **Environment**: `Docker`
   - **Dockerfile Path**: `api/Dockerfile` (or just `Dockerfile` if root directory is `api`)
   - **Docker Context**: `api` (or `.` if root directory is `api`)
4. Click **"Create Web Service"**

### Step 4: Configure Environment Variables

In your Render Web Service settings → **"Environment"** tab, add these variables:

| Variable Name | Value | Notes |
|--------------|-------|-------|
| `DatabaseProvider` | `PostgreSQL` | Tells the app to use PostgreSQL |
| `AllowedOrigins` | `https://your-app.vercel.app,https://your-app.netlify.app` | Add your frontend URL(s) here (comma-separated, no spaces) |
| `ASPNETCORE_ENVIRONMENT` | `Production` | Sets production mode |

**Important Notes:**
- Render automatically provides `DATABASE_URL` when you link the database - you don't need to set it manually
- The code automatically detects and parses Render's `DATABASE_URL` format
- Add your Vercel/Netlify frontend URL(s) to `AllowedOrigins` (you'll get this after deploying frontend)

### Step 5: Link Database to Web Service

1. In your Web Service dashboard, go to **"Environment"** tab
2. Scroll down to **"Add Environment Variable"**
3. Click **"Link Resource"** → Select your PostgreSQL database
4. Render will automatically add `DATABASE_URL` environment variable
5. The connection string format will be: `postgresql://user:password@host:port/database`

### Step 6: Configure Build Settings (if needed)

Render should auto-detect Docker, but verify:
- **Build Command**: (Leave empty - Docker handles this)
- **Start Command**: (Leave empty - Dockerfile ENTRYPOINT handles this)

### Step 7: Run Database Migrations

**Option A: Using Render Shell (Recommended)**

1. In your Render Web Service dashboard, click **"Shell"** tab
2. Wait for the shell to connect
3. Run migrations:
   ```bash
   cd /opt/render/project/src/api
   dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
   ```

**Option B: Using Render CLI**

1. Install Render CLI:
   ```bash
   npm install -g render-cli
   # OR
   brew install render
   ```

2. Login:
   ```bash
   render login
   ```

3. Get database connection string:
   ```bash
   render db:connection-string --name nammahall-db
   ```

4. Run migrations locally (with the connection string):
   ```bash
   export DATABASE_URL="<connection-string-from-render>"
   cd api
   dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
   ```

**Option C: Using the Helper Script**

1. Make the script executable:
   ```bash
   chmod +x api/render-migrate.sh
   ```

2. Run it:
   ```bash
   ./api/render-migrate.sh
   ```

### Step 8: Get Your API URL

1. Once deployed, Render will provide a URL like: `https://nammahall-api.onrender.com`
2. **Copy this URL** - you'll need it for frontend configuration
3. Your API will be accessible at: `https://nammahall-api.onrender.com/api`

**Note**: Free tier services may take 30-60 seconds to wake up after inactivity. This is normal for Render's free tier.

---

## ⚡ Part 2: Deploy Frontend to Vercel

### Step 1: Connect GitHub Repository

1. Go to [vercel.com](https://vercel.com) and sign in
2. Click **"Add New..."** → **"Project"**
3. Import your GitHub repository: `NaveenVdas/NammaHall`
4. Vercel will auto-detect it's a Vite project

### Step 2: Configure Build Settings

Vercel should auto-detect these, but verify:

- **Framework Preset**: Vite
- **Root Directory**: `ui` (click "Edit" and set to `ui`)
- **Build Command**: `npm run build`
- **Output Directory**: `dist`
- **Install Command**: `npm install`

### Step 3: Add Environment Variables

In Vercel project settings → **"Environment Variables"**, add:

| Variable Name | Value | Environment |
|--------------|-------|-------------|
| `VITE_API_URL` | `https://your-api.onrender.com/api` | Production, Preview, Development |

**Replace `your-api.onrender.com` with your actual Render API URL!**

### Step 4: Deploy

1. Click **"Deploy"**
2. Vercel will build and deploy your app
3. You'll get a URL like: `https://nammahall.vercel.app`

### Step 5: Update Render CORS Settings

1. Go back to Render
2. Update the `AllowedOrigins` environment variable in your Web Service:
   ```
   https://your-app.vercel.app,https://your-app-git-main.vercel.app
   ```
3. Render will automatically redeploy with the new settings

---

## 🌐 Part 2 Alternative: Deploy Frontend to Netlify

### Step 1: Connect GitHub Repository

1. Go to [netlify.com](https://netlify.com) and sign in
2. Click **"Add new site"** → **"Import an existing project"**
3. Choose **"GitHub"** and select your repository: `NaveenVdas/NammaHall`

### Step 2: Configure Build Settings

Netlify should auto-detect, but configure:

- **Base directory**: `ui`
- **Build command**: `npm run build`
- **Publish directory**: `ui/dist`

### Step 3: Add Environment Variables

In Netlify site settings → **"Environment variables"**, add:

| Variable Name | Value | Scopes |
|--------------|-------|--------|
| `VITE_API_URL` | `https://your-api.onrender.com/api` | All scopes |

**Replace `your-api.onrender.com` with your actual Render API URL!**

### Step 4: Deploy

1. Click **"Deploy site"**
2. Netlify will build and deploy your app
3. You'll get a URL like: `https://nammahall.netlify.app`

### Step 5: Update Render CORS Settings

1. Go back to Render
2. Update the `AllowedOrigins` environment variable:
   ```
   https://your-app.netlify.app
   ```
3. Render will automatically redeploy

---

## 🔄 Part 3: Update API URL in Frontend

The frontend is already configured to use `VITE_API_URL` environment variable. After setting it in Vercel/Netlify, rebuilds will automatically use the correct API URL.

---

## 🧪 Testing Your Deployment

### Test Backend (Render)

1. Visit: `https://your-api.onrender.com/healthz` (health check endpoint)
2. Or test: `https://your-api.onrender.com/api/halls`
3. Should return JSON data
4. **Note**: First request after inactivity may take 30-60 seconds (free tier spin-up time)

### Test Frontend (Vercel/Netlify)

1. Visit: `https://your-app.vercel.app` or `https://your-app.netlify.app`
2. Try browsing halls
3. Try creating a booking
4. Test admin login (if you have seed data)

---

## 🔧 Troubleshooting

### CORS Errors

**Problem**: Frontend can't connect to API

**Solution**:
1. Check `AllowedOrigins` in Render includes your Vercel/Netlify URL
2. Make sure there are no trailing slashes
3. Use `https://` not `http://` for production
4. Separate multiple origins with commas (no spaces): `https://app1.vercel.app,https://app2.netlify.app`
5. Redeploy Render service after changing CORS

### Database Connection Errors

**Problem**: API can't connect to database

**Solution**:
1. Verify database is linked to web service in Render dashboard
2. Check `DATABASE_URL` is set (Render sets this automatically when linked)
3. Check `DatabaseProvider` is set to `PostgreSQL`
4. Verify database service is running in Render
5. Check Render logs for connection errors

### Migration Errors

**Problem**: Migrations fail

**Solution**:
1. Make sure you're running migrations against the correct database
2. Check that `DATABASE_URL` is accessible
3. Verify EF Core tools are available in Render shell
4. Try running migrations locally first with Render's connection string
5. Check Render logs for detailed error messages

### Build Errors

**Problem**: Render build fails

**Solution**:
1. Check Dockerfile path is correct in Render settings
2. Verify root directory is set to `api` (if Dockerfile is in api folder)
3. Check Render build logs for specific errors
4. Test Docker build locally: `docker build -t nammahall-api ./api`

### Slow First Request (Free Tier)

**Problem**: First API request takes 30-60 seconds

**Solution**:
- This is normal for Render's free tier - services sleep after 15 minutes of inactivity
- Consider upgrading to a paid plan for always-on service
- Or use a service like [UptimeRobot](https://uptimerobot.com) to ping your API every 5 minutes to keep it awake

---

## 📝 Environment Variables Reference

### Render (Backend)

| Variable | Required | Default | Description |
|----------|----------|---------|-------------|
| `DATABASE_URL` | Auto | - | Automatically provided by Render when database is linked |
| `DatabaseProvider` | Yes | `SqlServer` | Set to `PostgreSQL` |
| `AllowedOrigins` | Yes | - | Comma-separated list of frontend URLs (no spaces) |
| `ASPNETCORE_ENVIRONMENT` | No | `Production` | Environment name |

### Vercel/Netlify (Frontend)

| Variable | Required | Default | Description |
|----------|----------|---------|-------------|
| `VITE_API_URL` | Yes | - | Full API URL with `/api` suffix |

---

## 🎯 Quick Checklist

### Render Setup
- [ ] Created Render account
- [ ] Created PostgreSQL database
- [ ] Created Web Service
- [ ] Linked database to web service
- [ ] Set `DatabaseProvider=PostgreSQL`
- [ ] Set `AllowedOrigins` (will update after frontend deploy)
- [ ] Deployed API service
- [ ] Ran database migrations
- [ ] Tested API endpoint
- [ ] Copied API URL

### Vercel/Netlify Setup
- [ ] Connected GitHub repository
- [ ] Set `VITE_API_URL` environment variable
- [ ] Deployed frontend
- [ ] Copied frontend URL

### Final Steps
- [ ] Updated Render `AllowedOrigins` with frontend URL
- [ ] Tested full application flow
- [ ] Verified admin login works
- [ ] Tested booking creation

---

## 🚀 Next Steps

1. **Custom Domain**: Add your own domain in Render/Vercel/Netlify settings
2. **SSL**: All platforms provide SSL automatically
3. **Monitoring**: Set up Render/Vercel/Netlify monitoring
4. **Backups**: Configure PostgreSQL backups in Render (available on paid plans)
5. **CI/CD**: All platforms auto-deploy on git push
6. **Always-On Service**: Consider upgrading Render plan to avoid sleep delays

---

## 💡 Pro Tips

1. **Use Render Blueprint** - Easiest way to set up both database and web service at once
2. **Keep Service Awake** - Use a monitoring service to ping your API every 5 minutes (free tier limitation)
3. **Monitor Logs** - Render provides excellent logging in the dashboard
4. **Use Environment Variables** - Never commit secrets, use Render's environment variable system
5. **Test Locally First** - Run migrations locally with Render's connection string before deploying
6. **Staging Environment** - Create separate Render/Vercel projects for testing

---

## 📚 Additional Resources

- [Render Documentation](https://render.com/docs)
- [Vercel Documentation](https://vercel.com/docs)
- [Netlify Documentation](https://docs.netlify.com)
- [.NET Docker Guide](https://docs.microsoft.com/en-us/dotnet/core/docker/)
- [EF Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

---

## ⚠️ Important Notes About Render Free Tier

1. **Sleep Behavior**: Free web services sleep after 15 minutes of inactivity
   - First request after sleep takes 30-60 seconds to wake up
   - This is normal and expected behavior
   - Consider upgrading for production use

2. **Database Limits**: Free PostgreSQL has 1 GB storage
   - Good for MVP and early development
   - Monitor your usage in Render dashboard
   - Upgrade when approaching limits

3. **Build Time**: Free tier has build time limits
   - Usually sufficient for .NET apps
   - Monitor build times in Render dashboard

---

**Need Help?** Check the troubleshooting section or review the platform-specific documentation.
