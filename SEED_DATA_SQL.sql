-- Seed Data for Namma Hall Database
-- Run this in your database editor (DBeaver, pgAdmin, etc.)

-- ============================================
-- 1. Seed Admin User
-- Default password: admin123
-- Password hash (SHA256): 240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9
-- ============================================
INSERT INTO "AdminUsers" ("Email", "PasswordHash", "DisplayName", "IsActive", "CreatedAtUtc", "UpdatedAtUtc")
SELECT 'admin@nammahall.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Admin User', true, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC'
WHERE NOT EXISTS (SELECT 1 FROM "AdminUsers" WHERE "Email" = 'admin@nammahall.com');

-- ============================================
-- 2. Seed Hall 1: Sri Lakshmi Convention Hall
-- ============================================
DO $$
DECLARE
    v_hall_id INTEGER;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "Halls" WHERE "Name" = 'Sri Lakshmi Convention Hall') THEN
        INSERT INTO "Halls"
        ("Name", "AddressLine", "Village", "Taluk", "District", "PostalCode", "Capacity", "PriceFrom", "PriceTo", 
         "IsAc", "HasDiningHall", "HasParking", "HasRooms", "OwnerName", "OwnerPhone", "AlternatePhone", 
         "GoogleMapsUrl", "IsPublished", "IsVerified", "CreatedAtUtc", "UpdatedAtUtc")
        VALUES
        ('Sri Lakshmi Convention Hall', 'Near Bus Stand', 'K R Nagar', 'K R Nagar', 'Mysuru', '571602', 
         800, 45000.00, 60000.00, true, true, true, true, 'Lakshmi Venkatesh', '919999999999', NULL, 
         'https://maps.google.com', true, true, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC')
        RETURNING "Id" INTO v_hall_id;
        
        INSERT INTO "HallImages"("HallId", "ImageUrl", "DisplayOrder", "CreatedAtUtc", "UpdatedAtUtc")
        VALUES
        (v_hall_id, 'https://images.pexels.com/photos/169211/pexels-photo-169211.jpeg?auto=compress&cs=tinysrgb&w=800', 0, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC'),
        (v_hall_id, 'https://images.pexels.com/photos/169188/pexels-photo-169188.jpeg?auto=compress&cs=tinysrgb&w=800', 1, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC');
    END IF;
END $$;

-- ============================================
-- 3. Seed Hall 2: Nisarga Kalyana Mantapa
-- ============================================
DO $$
DECLARE
    v_hall_id INTEGER;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "Halls" WHERE "Name" = 'Nisarga Kalyana Mantapa') THEN
        INSERT INTO "Halls"
        ("Name", "AddressLine", "Village", "Taluk", "District", "PostalCode", "Capacity", "PriceFrom", "PriceTo", 
         "IsAc", "HasDiningHall", "HasParking", "HasRooms", "OwnerName", "OwnerPhone", "AlternatePhone", 
         "GoogleMapsUrl", "IsPublished", "IsVerified", "CreatedAtUtc", "UpdatedAtUtc")
        VALUES
        ('Nisarga Kalyana Mantapa', 'Main Road', 'T. Narasipura', 'T. Narasipura', 'Mysuru', '571124', 
         500, 30000.00, 40000.00, true, true, true, false, 'Ramesh Kumar', '919888888888', NULL, 
         'https://maps.google.com', false, false, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC')
        RETURNING "Id" INTO v_hall_id;
        
        INSERT INTO "HallImages"("HallId", "ImageUrl", "DisplayOrder", "CreatedAtUtc", "UpdatedAtUtc")
        VALUES
        (v_hall_id, 'https://images.pexels.com/photos/169188/pexels-photo-169188.jpeg?auto=compress&cs=tinysrgb&w=800', 0, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC');
    END IF;
END $$;

-- ============================================
-- 4. Seed Hall 3: Shree Venkateshwara Kalyana Mantapa
-- ============================================
DO $$
DECLARE
    v_hall_id INTEGER;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "Halls" WHERE "Name" = 'Shree Venkateshwara Kalyana Mantapa') THEN
        INSERT INTO "Halls"
        ("Name", "AddressLine", "Village", "Taluk", "District", "PostalCode", "Capacity", "PriceFrom", "PriceTo", 
         "IsAc", "HasDiningHall", "HasParking", "HasRooms", "OwnerName", "OwnerPhone", "AlternatePhone", 
         "GoogleMapsUrl", "IsPublished", "IsVerified", "CreatedAtUtc", "UpdatedAtUtc")
        VALUES
        ('Shree Venkateshwara Kalyana Mantapa', 'Near Temple', 'K R Nagar', 'K R Nagar', 'Mysuru', '571602', 
         600, 35000.00, NULL, true, true, true, true, 'Venkatesh Gowda', '919777777777', NULL, 
         'https://maps.google.com', true, true, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC')
        RETURNING "Id" INTO v_hall_id;
        
        INSERT INTO "HallImages"("HallId", "ImageUrl", "DisplayOrder", "CreatedAtUtc", "UpdatedAtUtc")
        VALUES
        (v_hall_id, 'https://images.pexels.com/photos/169211/pexels-photo-169211.jpeg?auto=compress&cs=tinysrgb&w=800', 0, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC');
    END IF;
END $$;

-- ============================================
-- Verification Queries
-- ============================================

-- Check admin user
SELECT * FROM "AdminUsers" WHERE "Email" = 'admin@nammahall.com';

-- Check halls
SELECT "Id", "Name", "District", "IsPublished", "IsVerified" FROM "Halls";

-- Check hall images
SELECT "Id", "HallId", "ImageUrl", "DisplayOrder" FROM "HallImages";

-- Count records
SELECT 
    (SELECT COUNT(*) FROM "AdminUsers") as admin_users,
    (SELECT COUNT(*) FROM "Halls") as halls,
    (SELECT COUNT(*) FROM "HallImages") as hall_images;

