#!/bin/bash
# Railway Migration Script
# This script helps run EF Core migrations on Railway

echo "🚀 Running database migrations on Railway..."

# Check if Railway CLI is installed
if ! command -v railway &> /dev/null
then
    echo "❌ Railway CLI not found. Installing..."
    npm i -g @railway/cli
fi

# Check if logged in
if ! railway whoami &> /dev/null
then
    echo "🔐 Please login to Railway..."
    railway login
fi

# Run migrations
echo "📦 Running migrations..."
railway run dotnet ef database update \
    --project NammaHall.Infrastructure \
    --startup-project NammaHall.Api

echo "✅ Migrations completed!"

