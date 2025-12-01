#!/bin/bash
# Render Migration Script
# This script helps run EF Core migrations on Render

echo "🚀 Running database migrations on Render..."

# Check if Render CLI is installed
if ! command -v render &> /dev/null
then
    echo "❌ Render CLI not found."
    echo "📦 Install it from: https://render.com/docs/cli"
    echo "   Or use: npm install -g render-cli"
    exit 1
fi

# Check if logged in
if ! render whoami &> /dev/null
then
    echo "🔐 Please login to Render..."
    render login
fi

# Get the database connection string from Render
echo "📡 Fetching database connection string..."
DB_URL=$(render db:connection-string)

if [ -z "$DB_URL" ]; then
    echo "❌ Could not get database connection string."
    echo "💡 Make sure you have a PostgreSQL database in your Render dashboard."
    exit 1
fi

# Export DATABASE_URL for dotnet ef
export DATABASE_URL="$DB_URL"

# Run migrations
echo "📦 Running migrations..."
dotnet ef database update \
    --project NammaHall.Infrastructure \
    --startup-project NammaHall.Api

echo "✅ Migrations completed!"

