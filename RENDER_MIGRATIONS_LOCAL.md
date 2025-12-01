# 🖥️ Run Render Migrations from Local PowerShell

Since Render Shell requires a subscription, here's how to run migrations from your local machine.

---

## 📋 Step 1: Get External Database URL from Render

1. **Go to Render Dashboard**
   - Visit: https://dashboard.render.com
   - Click on your **PostgreSQL database** (not the web service)

2. **Get Connection String**
   - Go to **"Info"** or **"Connections"** tab
   - Look for **"External Database URL"** or **"Connection String"**
   - It should look like:
     ```
     postgresql://user:password@dpg-xxxxx-a.oregon-postgres.render.com:5432/database
     ```
   - **Copy this entire string** - you'll need it

3. **Alternative: Connection Pooling URL** (Recommended)
   - Look for **"Connection Pooling"** section
   - If available, use the **pooling URL** instead
   - Pooling URLs are better for external connections
   - Format: `postgresql://user:password@dpg-xxxxx-a-pooler.oregon-postgres.render.com:5432/database`

---

## 🔧 Step 2: Set Up Environment Variables in PowerShell

Open PowerShell and navigate to your project:

```powershell
cd "C:\Namma Hall\api"
```

### Set Environment Variables:

```powershell
# Replace with your actual connection string from Render
$env:DATABASE_URL="postgresql://user:password@dpg-xxxxx-a.oregon-postgres.render.com:5432/database"

# Set database provider
$env:DatabaseProvider="PostgreSQL"
```

**Important**: 
- Replace the connection string with your actual one from Render
- Make sure there are no extra spaces
- The connection string should be in quotes

---

## 🚀 Step 3: Run Migrations

Once environment variables are set, run:

```powershell
dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api
```

### Expected Output:

You should see:
```
Build started...
Build succeeded.
Applying migration '20251130171950_InitialCreate'...
Applying migration '20251130172259_SeedData'...
Done.
```

---

## ✅ Step 4: Verify Migrations Applied

You can verify by checking if tables were created:

### Option A: Check via API (After Deployment)

Once your API is deployed, test:
- `https://your-api.onrender.com/api/halls`
- Should return data (or empty array if no halls yet)

### Option B: Connect with Database Tool

Use a PostgreSQL client (like pgAdmin, DBeaver, or VS Code extension) to connect and verify tables exist.

---

## 🔍 Troubleshooting

### Error: "No such host is known"

**Problem**: Using internal database URL  
**Solution**: Make sure you're using the **External Database URL**, not the internal one

**How to identify:**
- ❌ Internal: `dpg-xxxxx-a:5432` (no domain)
- ✅ External: `dpg-xxxxx-a.oregon-postgres.render.com:5432` (has `.render.com`)

### Error: "Connection timeout"

**Problem**: Firewall or network issue  
**Solution**: 
- Check if your IP is allowed (Render free tier allows all IPs by default)
- Try using Connection Pooling URL instead
- Check your internet connection

### Error: "Authentication failed"

**Problem**: Wrong credentials in connection string  
**Solution**: 
- Double-check the connection string from Render
- Make sure you copied the entire string correctly
- Verify username and password are correct

### Error: "Pending model changes"

**Problem**: Model mismatch  
**Solution**: 
- Make sure you've pulled the latest code (with the fix we just pushed)
- The fix should suppress this warning now
- If still happening, we may need to regenerate migrations

---

## 📝 Complete PowerShell Script

Here's a complete script you can copy-paste (just replace the connection string):

```powershell
# Navigate to API directory
cd "C:\Namma Hall\api"

# Set environment variables (REPLACE WITH YOUR ACTUAL CONNECTION STRING)
$env:DATABASE_URL="postgresql://user:password@dpg-xxxxx-a.oregon-postgres.render.com:5432/database"
$env:DatabaseProvider="PostgreSQL"

# Run migrations
dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api

# Verify (optional - check exit code)
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Migrations completed successfully!" -ForegroundColor Green
} else {
    Write-Host "❌ Migrations failed. Check errors above." -ForegroundColor Red
}
```

---

## 🎯 Quick Steps Summary

1. ✅ Get External Database URL from Render dashboard
2. ✅ Open PowerShell in `C:\Namma Hall\api`
3. ✅ Set `$env:DATABASE_URL` with your connection string
4. ✅ Set `$env:DatabaseProvider="PostgreSQL"`
5. ✅ Run `dotnet ef database update --project NammaHall.Infrastructure --startup-project NammaHall.Api`
6. ✅ Verify success

---

## 💡 Pro Tips

1. **Save Connection String Securely**
   - Don't commit it to git
   - Consider using PowerShell profile or environment variables
   - Or save in a local `.env` file (not committed)

2. **Use Connection Pooling**
   - If available, use the pooling URL
   - Better performance for external connections
   - More reliable

3. **Test Connection First**
   - You can test the connection string with a PostgreSQL client
   - Verify you can connect before running migrations

---

**Once migrations succeed, your database will be ready and your API should work!** 🚀

