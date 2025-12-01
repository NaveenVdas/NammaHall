# 🚀 Deploy UI to Vercel - Step-by-Step Guide

## ✅ Prerequisites
- [x] API is working: `https://nammahall-api.onrender.com/api/halls`
- [ ] Vercel account (sign up at [vercel.com](https://vercel.com) if you don't have one)
- [ ] GitHub repository: `NaveenVdas/NammaHall` (already pushed)

---

## 📋 Step 1: Create Vercel Account

1. Go to [vercel.com](https://vercel.com)
2. Click **"Sign Up"**
3. Choose **"Continue with GitHub"** (recommended - easier integration)
4. Authorize Vercel to access your GitHub account

---

## 📋 Step 2: Import Your Repository

1. **In Vercel Dashboard**
   - After signing in, you'll see the dashboard
   - Click **"Add New..."** → **"Project"**

2. **Select Repository**
   - You'll see a list of your GitHub repositories
   - Find and select: **`NaveenVdas/NammaHall`**
   - Click **"Import"**

---

## 📋 Step 3: Configure Project Settings

Vercel should auto-detect it's a Vite project, but verify these settings:

### Framework Preset
- **Framework Preset**: `Vite` (should auto-detect)

### Build Settings
- **Root Directory**: Click **"Edit"** and set to: `ui`
- **Build Command**: `npm run build` (should auto-fill)
- **Output Directory**: `dist` (should auto-fill)
- **Install Command**: `npm install` (should auto-fill)

### Environment Variables
- Click **"Environment Variables"** section
- Add this variable:
  - **Key**: `VITE_API_URL`
  - **Value**: `https://nammahall-api.onrender.com/api`
  - **Environments**: Select all (Production, Preview, Development)
  - Click **"Add"**

---

## 📋 Step 4: Deploy

1. **Review Settings**
   - Make sure Root Directory = `ui`
   - Make sure `VITE_API_URL` is set
   - Framework = Vite

2. **Deploy**
   - Click **"Deploy"** button
   - Vercel will:
     - Install dependencies
     - Build your React app
     - Deploy it
   - This takes 2-3 minutes

3. **Wait for Deployment**
   - You'll see build logs in real-time
   - Status will change to **"Ready"** when complete

---

## 📋 Step 5: Get Your Frontend URL

After deployment completes:

1. **Copy Your URL**
   - Vercel will provide a URL like: `https://nammahall.vercel.app`
   - **Copy this URL** - you'll need it for the next step

2. **Note Preview URLs**
   - You might also see preview URLs for branches
   - Format: `https://nammahall-git-main.vercel.app`

---

## 📋 Step 6: Update Render CORS Settings

Now we need to allow your Vercel URL to access the API:

1. **Go to Render Dashboard**
   - Visit: https://dashboard.render.com
   - Click on your **Web Service** (API)

2. **Update Environment Variables**
   - Go to **"Environment"** tab
   - Find `AllowedOrigins` variable
   - Click **"Edit"** (or pencil icon)

3. **Add Vercel URLs**
   - Update the value to include your Vercel URLs:
     ```
     https://your-app.vercel.app,https://your-app-git-main.vercel.app
     ```
   - Replace `your-app` with your actual Vercel app name
   - **No spaces** between URLs, just commas
   - Example: `https://nammahall.vercel.app,https://nammahall-git-main.vercel.app`

4. **Save**
   - Click **"Save Changes"**
   - Render will automatically redeploy with new CORS settings
   - Wait 2-3 minutes for redeploy

---

## ✅ Step 7: Test Your Application

1. **Visit Your Vercel URL**
   - Go to: `https://your-app.vercel.app`
   - Should see your React app

2. **Test Features**
   - Browse halls (should load from API)
   - View hall details
   - Try creating a booking
   - Test admin login

3. **Check Browser Console**
   - Open browser DevTools (F12)
   - Check Console tab for any errors
   - Check Network tab to verify API calls

---

## 🔧 Troubleshooting

### CORS Errors

**Problem**: Browser shows CORS error when calling API

**Solution**:
1. Verify `AllowedOrigins` in Render includes your Vercel URL
2. Make sure there are no trailing slashes
3. Use `https://` not `http://`
4. Wait for Render to redeploy after changing CORS

### API Not Loading

**Problem**: UI loads but no data from API

**Solution**:
1. Check `VITE_API_URL` environment variable in Vercel
2. Should be: `https://nammahall-api.onrender.com/api` (with `/api` suffix)
3. Check browser console for errors
4. Verify API is accessible: `https://nammahall-api.onrender.com/api/halls`

### Build Fails

**Problem**: Vercel build fails

**Solution**:
1. Check build logs in Vercel
2. Common issues:
   - Wrong root directory (should be `ui`)
   - Missing dependencies
   - TypeScript errors
3. Fix errors and redeploy

---

## 📝 Quick Checklist

### Vercel Setup
- [ ] Created Vercel account
- [ ] Imported GitHub repository
- [ ] Set Root Directory = `ui`
- [ ] Set `VITE_API_URL` = `https://nammahall-api.onrender.com/api`
- [ ] Deployed successfully
- [ ] Got Vercel URL

### Render CORS Update
- [ ] Updated `AllowedOrigins` with Vercel URL
- [ ] Render redeployed
- [ ] CORS errors resolved

### Testing
- [ ] UI loads on Vercel
- [ ] Halls load from API
- [ ] No CORS errors
- [ ] Admin features work

---

## 🎯 Your URLs After Deployment

- **Frontend**: `https://your-app.vercel.app`
- **API**: `https://nammahall-api.onrender.com/api`
- **API Health**: `https://nammahall-api.onrender.com/healthz`

---

## 💡 Pro Tips

1. **Automatic Deployments**: Vercel auto-deploys on every git push
2. **Preview Deployments**: Each branch gets its own preview URL
3. **Environment Variables**: Can be different for Production vs Preview
4. **Custom Domain**: You can add your own domain later in Vercel settings

---

**Follow the steps above to deploy your UI to Vercel!** 🚀

