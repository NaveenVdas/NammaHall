using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NammaHall.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var isPostgres = migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL";
            
            if (isPostgres)
            {
                SeedDataForPostgreSQL(migrationBuilder);
            }
            else
            {
                SeedDataForSqlServer(migrationBuilder);
            }
        }

        private void SeedDataForSqlServer(MigrationBuilder migrationBuilder)
        {
            // Seed Admin User (default password: admin123)
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [dbo].[AdminUsers] WHERE Email = 'admin@nammahall.com')
                BEGIN
                    INSERT INTO [dbo].[AdminUsers](Email, PasswordHash, DisplayName, IsActive, CreatedAtUtc, UpdatedAtUtc)
                    VALUES('admin@nammahall.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Admin User', 1, GETUTCDATE(), GETUTCDATE())
                END
            ");

            // Seed Hall 1: Sri Lakshmi Convention Hall
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [dbo].[Halls] WHERE Name = 'Sri Lakshmi Convention Hall')
                BEGIN
                    DECLARE @HallId1 INT;
                    
                    INSERT INTO [dbo].[Halls]
                    (Name, AddressLine, Village, Taluk, District, PostalCode, Capacity, PriceFrom, PriceTo, 
                     IsAc, HasDiningHall, HasParking, HasRooms, OwnerName, OwnerPhone, AlternatePhone, 
                     GoogleMapsUrl, IsPublished, IsVerified, CreatedAtUtc, UpdatedAtUtc)
                    VALUES
                    ('Sri Lakshmi Convention Hall', 'Near Bus Stand', 'K R Nagar', 'K R Nagar', 'Mysuru', '571602', 
                     800, 45000.00, 60000.00, 1, 1, 1, 1, 'Lakshmi Venkatesh', '919999999999', NULL, 
                     'https://maps.google.com', 1, 1, GETUTCDATE(), GETUTCDATE());
                    
                    SET @HallId1 = SCOPE_IDENTITY();
                    
                    INSERT INTO [dbo].[HallImages](HallId, ImageUrl, DisplayOrder, CreatedAtUtc, UpdatedAtUtc)
                    VALUES
                    (@HallId1, 'https://images.pexels.com/photos/169211/pexels-photo-169211.jpeg?auto=compress&cs=tinysrgb&w=800', 0, GETUTCDATE(), GETUTCDATE()),
                    (@HallId1, 'https://images.pexels.com/photos/169188/pexels-photo-169188.jpeg?auto=compress&cs=tinysrgb&w=800', 1, GETUTCDATE(), GETUTCDATE());
                END
            ");

            // Seed Hall 2: Nisarga Kalyana Mantapa
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [dbo].[Halls] WHERE Name = 'Nisarga Kalyana Mantapa')
                BEGIN
                    DECLARE @HallId2 INT;
                    
                    INSERT INTO [dbo].[Halls]
                    (Name, AddressLine, Village, Taluk, District, PostalCode, Capacity, PriceFrom, PriceTo, 
                     IsAc, HasDiningHall, HasParking, HasRooms, OwnerName, OwnerPhone, AlternatePhone, 
                     GoogleMapsUrl, IsPublished, IsVerified, CreatedAtUtc, UpdatedAtUtc)
                    VALUES
                    ('Nisarga Kalyana Mantapa', 'Main Road', 'T. Narasipura', 'T. Narasipura', 'Mysuru', '571124', 
                     500, 30000.00, 40000.00, 1, 1, 1, 0, 'Ramesh Kumar', '919888888888', NULL, 
                     'https://maps.google.com', 0, 0, GETUTCDATE(), GETUTCDATE());
                    
                    SET @HallId2 = SCOPE_IDENTITY();
                    
                    INSERT INTO [dbo].[HallImages](HallId, ImageUrl, DisplayOrder, CreatedAtUtc, UpdatedAtUtc)
                    VALUES
                    (@HallId2, 'https://images.pexels.com/photos/169188/pexels-photo-169188.jpeg?auto=compress&cs=tinysrgb&w=800', 0, GETUTCDATE(), GETUTCDATE());
                END
            ");

            // Seed Hall 3: Shree Venkateshwara Kalyana Mantapa
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [dbo].[Halls] WHERE Name = 'Shree Venkateshwara Kalyana Mantapa')
                BEGIN
                    DECLARE @HallId3 INT;
                    
                    INSERT INTO [dbo].[Halls]
                    (Name, AddressLine, Village, Taluk, District, PostalCode, Capacity, PriceFrom, PriceTo, 
                     IsAc, HasDiningHall, HasParking, HasRooms, OwnerName, OwnerPhone, AlternatePhone, 
                     GoogleMapsUrl, IsPublished, IsVerified, CreatedAtUtc, UpdatedAtUtc)
                    VALUES
                    ('Shree Venkateshwara Kalyana Mantapa', 'Near Temple', 'K R Nagar', 'K R Nagar', 'Mysuru', '571602', 
                     600, 35000.00, NULL, 1, 1, 1, 1, 'Venkatesh Gowda', '919777777777', NULL, 
                     'https://maps.google.com', 1, 1, GETUTCDATE(), GETUTCDATE());
                    
                    SET @HallId3 = SCOPE_IDENTITY();
                    
                    INSERT INTO [dbo].[HallImages](HallId, ImageUrl, DisplayOrder, CreatedAtUtc, UpdatedAtUtc)
                    VALUES
                    (@HallId3, 'https://images.pexels.com/photos/169211/pexels-photo-169211.jpeg?auto=compress&cs=tinysrgb&w=800', 0, GETUTCDATE(), GETUTCDATE());
                END
            ");
        }

        private void SeedDataForPostgreSQL(MigrationBuilder migrationBuilder)
        {
            // Seed Admin User (default password: admin123)
            migrationBuilder.Sql(@"
                INSERT INTO ""AdminUsers"" (""Email"", ""PasswordHash"", ""DisplayName"", ""IsActive"", ""CreatedAtUtc"", ""UpdatedAtUtc"")
                SELECT 'admin@nammahall.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Admin User', true, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC'
                WHERE NOT EXISTS (SELECT 1 FROM ""AdminUsers"" WHERE ""Email"" = 'admin@nammahall.com')
            ");

            // Seed Hall 1: Sri Lakshmi Convention Hall
            migrationBuilder.Sql(@"
                DO $$
                DECLARE
                    v_hall_id INTEGER;
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM ""Halls"" WHERE ""Name"" = 'Sri Lakshmi Convention Hall') THEN
                        INSERT INTO ""Halls""
                        (""Name"", ""AddressLine"", ""Village"", ""Taluk"", ""District"", ""PostalCode"", ""Capacity"", ""PriceFrom"", ""PriceTo"", 
                         ""IsAc"", ""HasDiningHall"", ""HasParking"", ""HasRooms"", ""OwnerName"", ""OwnerPhone"", ""AlternatePhone"", 
                         ""GoogleMapsUrl"", ""IsPublished"", ""IsVerified"", ""CreatedAtUtc"", ""UpdatedAtUtc"")
                        VALUES
                        ('Sri Lakshmi Convention Hall', 'Near Bus Stand', 'K R Nagar', 'K R Nagar', 'Mysuru', '571602', 
                         800, 45000.00, 60000.00, true, true, true, true, 'Lakshmi Venkatesh', '919999999999', NULL, 
                         'https://maps.google.com', true, true, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC')
                        RETURNING ""Id"" INTO v_hall_id;
                        
                        INSERT INTO ""HallImages""(""HallId"", ""ImageUrl"", ""DisplayOrder"", ""CreatedAtUtc"", ""UpdatedAtUtc"")
                        VALUES
                        (v_hall_id, 'https://images.pexels.com/photos/169211/pexels-photo-169211.jpeg?auto=compress&cs=tinysrgb&w=800', 0, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC'),
                        (v_hall_id, 'https://images.pexels.com/photos/169188/pexels-photo-169188.jpeg?auto=compress&cs=tinysrgb&w=800', 1, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC');
                    END IF;
                END $$;
            ");

            // Seed Hall 2: Nisarga Kalyana Mantapa
            migrationBuilder.Sql(@"
                DO $$
                DECLARE
                    v_hall_id INTEGER;
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM ""Halls"" WHERE ""Name"" = 'Nisarga Kalyana Mantapa') THEN
                        INSERT INTO ""Halls""
                        (""Name"", ""AddressLine"", ""Village"", ""Taluk"", ""District"", ""PostalCode"", ""Capacity"", ""PriceFrom"", ""PriceTo"", 
                         ""IsAc"", ""HasDiningHall"", ""HasParking"", ""HasRooms"", ""OwnerName"", ""OwnerPhone"", ""AlternatePhone"", 
                         ""GoogleMapsUrl"", ""IsPublished"", ""IsVerified"", ""CreatedAtUtc"", ""UpdatedAtUtc"")
                        VALUES
                        ('Nisarga Kalyana Mantapa', 'Main Road', 'T. Narasipura', 'T. Narasipura', 'Mysuru', '571124', 
                         500, 30000.00, 40000.00, true, true, true, false, 'Ramesh Kumar', '919888888888', NULL, 
                         'https://maps.google.com', false, false, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC')
                        RETURNING ""Id"" INTO v_hall_id;
                        
                        INSERT INTO ""HallImages""(""HallId"", ""ImageUrl"", ""DisplayOrder"", ""CreatedAtUtc"", ""UpdatedAtUtc"")
                        VALUES
                        (v_hall_id, 'https://images.pexels.com/photos/169188/pexels-photo-169188.jpeg?auto=compress&cs=tinysrgb&w=800', 0, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC');
                    END IF;
                END $$;
            ");

            // Seed Hall 3: Shree Venkateshwara Kalyana Mantapa
            migrationBuilder.Sql(@"
                DO $$
                DECLARE
                    v_hall_id INTEGER;
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM ""Halls"" WHERE ""Name"" = 'Shree Venkateshwara Kalyana Mantapa') THEN
                        INSERT INTO ""Halls""
                        (""Name"", ""AddressLine"", ""Village"", ""Taluk"", ""District"", ""PostalCode"", ""Capacity"", ""PriceFrom"", ""PriceTo"", 
                         ""IsAc"", ""HasDiningHall"", ""HasParking"", ""HasRooms"", ""OwnerName"", ""OwnerPhone"", ""AlternatePhone"", 
                         ""GoogleMapsUrl"", ""IsPublished"", ""IsVerified"", ""CreatedAtUtc"", ""UpdatedAtUtc"")
                        VALUES
                        ('Shree Venkateshwara Kalyana Mantapa', 'Near Temple', 'K R Nagar', 'K R Nagar', 'Mysuru', '571602', 
                         600, 35000.00, NULL, true, true, true, true, 'Venkatesh Gowda', '919777777777', NULL, 
                         'https://maps.google.com', true, true, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC')
                        RETURNING ""Id"" INTO v_hall_id;
                        
                        INSERT INTO ""HallImages""(""HallId"", ""ImageUrl"", ""DisplayOrder"", ""CreatedAtUtc"", ""UpdatedAtUtc"")
                        VALUES
                        (v_hall_id, 'https://images.pexels.com/photos/169211/pexels-photo-169211.jpeg?auto=compress&cs=tinysrgb&w=800', 0, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC');
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var isPostgres = migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL";
            
            if (isPostgres)
            {
                migrationBuilder.Sql(@"
                    DELETE FROM ""HallImages"" WHERE ""HallId"" IN 
                        (SELECT ""Id"" FROM ""Halls"" WHERE ""Name"" IN 
                            ('Sri Lakshmi Convention Hall', 'Nisarga Kalyana Mantapa', 'Shree Venkateshwara Kalyana Mantapa'));
                    
                    DELETE FROM ""Halls"" WHERE ""Name"" IN 
                        ('Sri Lakshmi Convention Hall', 'Nisarga Kalyana Mantapa', 'Shree Venkateshwara Kalyana Mantapa');
                    
                    DELETE FROM ""AdminUsers"" WHERE ""Email"" = 'admin@nammahall.com';
                ");
            }
            else
            {
                migrationBuilder.Sql(@"
                    DELETE FROM [dbo].[HallImages] WHERE HallId IN 
                        (SELECT Id FROM [dbo].[Halls] WHERE Name IN 
                            ('Sri Lakshmi Convention Hall', 'Nisarga Kalyana Mantapa', 'Shree Venkateshwara Kalyana Mantapa'));
                    
                    DELETE FROM [dbo].[Halls] WHERE Name IN 
                        ('Sri Lakshmi Convention Hall', 'Nisarga Kalyana Mantapa', 'Shree Venkateshwara Kalyana Mantapa');
                    
                    DELETE FROM [dbo].[AdminUsers] WHERE Email = 'admin@nammahall.com';
                ");
            }
        }
    }
}
