# 🚀 Deployment Guide: Railway + Vercel

This guide walks you through deploying the Namma Hall application to Railway (backend + PostgreSQL) and Vercel (frontend).

---

## ✅ What's Already Automated

The following has been set up for you:

1. **Dockerfile** - Ready for Railway deployment
2. **railway.json** - Railway configuration
3. **vercel.json** - Vercel configuration for React app
4. **Environment Variable Support** - Code reads from environment variables
5. **DATABASE_URL Parsing** - Automatically converts Railway's DATABASE_URL format
6. **CORS Configuration** - Supports environment-based CORS origins
7. **Provider-Aware Migrations** - Works with both SQL Server and PostgreSQL

---

## 📋 What You Need to Do Manually

### Prerequisites
- [ ] GitHub account
- [ ] Railway account (sign up at [railway.app](https://railway.app))
- [ ] Vercel account (sign up at [vercel.com](https://vercel.com))
- [ ] Your code pushed to a GitHub repository

---

## 🚂 Part 1: Deploy Backend to Railway

### Step 1: Create Railway Project

1. Go to [railway.app](https://railway.app) and sign in
2. Click **"New Project"**
3. Select **"Deploy from GitHub repo"**
4. Choose your repository
5. Railway will detect the Dockerfile automatically

### Step 2: Add PostgreSQL Database

1. In your Railway project, click **"+ New"**
2. Select **"Database"** → **"Add PostgreSQL"**
3. Railway will create a PostgreSQL instance
4. **Important**: Note the database name (you'll need it for migrations)

### Step 3: Configure Environment Variables

In your Railway service settings, add these environment variables:

| Variable Name | Value | Notes |
|--------------|-------|-------|
| `DatabaseProvider` | `PostgreSQL` | Tells the app to use PostgreSQL |
| `AllowedOrigins` | `https://your-app.vercel.app,https://www.yourdomain.com` | Add your Vercel URL(s) here (comma-separated) |
| `ASPNETCORE_ENVIRONMENT` | `Production` | Sets production mode |

**Important Notes:**
- Railway automatically provides `DATABASE_URL` - you don't need to set it manually
- The code automatically detects and parses Railway's `DATABASE_URL` format
- Add your Vercel frontend URL(s) to `AllowedOrigins` (you'll get this after deploying to Vercel)

### Step 4: Connect Database to API Service

1. In Railway, go to your PostgreSQL service
2. Click **"Variables"** tab
3. You'll see `DATABASE_URL` - this is automatically available to all services in the project
4. Your API service will automatically use this connection string

### Step 5: Run Database Migrations

**Option A: Using Railway CLI (Recommended)**

1. Install Railway CLI:
   ```bash
   npm i -g @railway/cli
   ```

2. Login to Railway:
   ```bash
   railway login
   ```

3. Link your project:
   ```bash
   railway link
   ```

4. Run migrations:
   ```bash
   railway run dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
   ```

**Option B: Using Railway's One-Click Deploy Script**

1. In Railway, go to your API service
2. Add a new environment variable:
   - Name: `RAILWAY_RUN_MIGRATIONS`
   - Value: `true`
3. Add this to your Dockerfile (before the ENTRYPOINT):
   ```dockerfile
   RUN dotnet tool install --global dotnet-ef
   ENV PATH="$PATH:/root/.dotnet/tools"
   ```
4. Modify ENTRYPOINT to run migrations first:
   ```dockerfile
   ENTRYPOINT ["sh", "-c", "dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api --no-build && dotnet NammaHall.Api.dll"]
   ```

**Option C: Manual Migration (After First Deploy)**

1. After your API is deployed, note the Railway service URL
2. SSH into the container or use Railway's shell
3. Run migrations manually

### Step 6: Get Your API URL

1. Once deployed, Railway will provide a URL like: `https://your-api.up.railway.app`
2. **Copy this URL** - you'll need it for Vercel configuration
3. Your API will be accessible at: `https://your-api.up.railway.app/api`

---

## ⚡ Part 2: Deploy Frontend to Vercel

### Step 1: Connect GitHub Repository

1. Go to [vercel.com](https://vercel.com) and sign in
2. Click **"Add New..."** → **"Project"**
3. Import your GitHub repository
4. Vercel will auto-detect it's a Vite project

### Step 2: Configure Build Settings

Vercel should auto-detect these, but verify:

- **Framework Preset**: Vite
- **Root Directory**: `ui` (if your UI is in a subfolder)
- **Build Command**: `npm run build`
- **Output Directory**: `dist`
- **Install Command**: `npm install`

### Step 3: Add Environment Variables

In Vercel project settings → Environment Variables, add:

| Variable Name | Value | Environment |
|--------------|-------|-------------|
| `VITE_API_URL` | `https://your-api.up.railway.app/api` | Production, Preview, Development |

**Replace `your-api.up.railway.app` with your actual Railway API URL!**

### Step 4: Deploy

1. Click **"Deploy"**
2. Vercel will build and deploy your app
3. You'll get a URL like: `https://your-app.vercel.app`

### Step 5: Update Railway CORS Settings

1. Go back to Railway
2. Update the `AllowedOrigins` environment variable:
   ```
   https://your-app.vercel.app,https://your-app-git-main.vercel.app
   ```
3. Redeploy your Railway service (or it will auto-redeploy)

---

## 🔄 Part 3: Update API URL in Frontend

The frontend is already configured to use `VITE_API_URL` environment variable. After setting it in Vercel, rebuilds will automatically use the correct API URL.

---

## 🧪 Testing Your Deployment

### Test Backend (Railway)

1. Visit: `https://your-api.up.railway.app/swagger` (if Swagger is enabled in production)
2. Or test: `https://your-api.up.railway.app/api/halls`
3. Should return JSON data

### Test Frontend (Vercel)

1. Visit: `https://your-app.vercel.app`
2. Try browsing halls
3. Try creating a booking
4. Test admin login (if you have seed data)

---

## 🔧 Troubleshooting

### CORS Errors

**Problem**: Frontend can't connect to API

**Solution**:
1. Check `AllowedOrigins` in Railway includes your Vercel URL
2. Make sure there are no trailing slashes
3. Use `https://` not `http://` for production
4. Redeploy Railway service after changing CORS

### Database Connection Errors

**Problem**: API can't connect to database

**Solution**:
1. Verify `DATABASE_URL` is set (Railway sets this automatically)
2. Check `DatabaseProvider` is set to `PostgreSQL`
3. Verify database service is running in Railway
4. Check Railway logs for connection errors

### Migration Errors

**Problem**: Migrations fail

**Solution**:
1. Make sure you're running migrations against the correct database
2. Check that `DATABASE_URL` is accessible
3. Verify EF Core tools are installed: `dotnet tool install --global dotnet-ef`
4. Try running migrations locally first with Railway's connection string

### Build Errors

**Problem**: Railway build fails

**Solution**:
1. Check Dockerfile path is correct
2. Verify all project references are correct
3. Check Railway build logs for specific errors
4. Test Docker build locally: `docker build -t nammahall-api ./api`

---

## 📝 Environment Variables Reference

### Railway (Backend)

| Variable | Required | Default | Description |
|----------|----------|---------|-------------|
| `DATABASE_URL` | Auto | - | Automatically provided by Railway |
| `DatabaseProvider` | Yes | `SqlServer` | Set to `PostgreSQL` |
| `AllowedOrigins` | Yes | - | Comma-separated list of frontend URLs |
| `ASPNETCORE_ENVIRONMENT` | No | `Production` | Environment name |

### Vercel (Frontend)

| Variable | Required | Default | Description |
|----------|----------|---------|-------------|
| `VITE_API_URL` | Yes | - | Full API URL with `/api` suffix |

---

## 🎯 Quick Checklist

### Railway Setup
- [ ] Created Railway project
- [ ] Added PostgreSQL database
- [ ] Set `DatabaseProvider=PostgreSQL`
- [ ] Set `AllowedOrigins` (will update after Vercel deploy)
- [ ] Deployed API service
- [ ] Ran database migrations
- [ ] Tested API endpoint
- [ ] Copied API URL

### Vercel Setup
- [ ] Connected GitHub repository
- [ ] Set `VITE_API_URL` environment variable
- [ ] Deployed frontend
- [ ] Copied Vercel URL

### Final Steps
- [ ] Updated Railway `AllowedOrigins` with Vercel URL
- [ ] Tested full application flow
- [ ] Verified admin login works
- [ ] Tested booking creation

---

## 🚀 Next Steps

1. **Custom Domain**: Add your own domain in Vercel/Railway settings
2. **SSL**: Both platforms provide SSL automatically
3. **Monitoring**: Set up Railway/Vercel monitoring
4. **Backups**: Configure PostgreSQL backups in Railway
5. **CI/CD**: Both platforms auto-deploy on git push

---

## 📚 Additional Resources

- [Railway Documentation](https://docs.railway.app)
- [Vercel Documentation](https://vercel.com/docs)
- [.NET Docker Guide](https://docs.microsoft.com/en-us/dotnet/core/docker/)
- [EF Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

---

## 💡 Pro Tips

1. **Use Railway's CLI** for easier migration management
2. **Set up staging environment** - create separate Railway/Vercel projects for testing
3. **Monitor logs** - Both platforms provide excellent logging
4. **Use Railway's metrics** to monitor API performance
5. **Enable Vercel Analytics** for frontend insights

---

**Need Help?** Check the troubleshooting section or review the platform-specific documentation.

