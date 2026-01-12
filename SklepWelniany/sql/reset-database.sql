-- Reset SklepWelnianyDb on local SQL Server (localhost)
-- Drops the database if it exists and creates a fresh empty database.

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'SklepWelnianyDb')
BEGIN
    ALTER DATABASE [SklepWelnianyDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [SklepWelnianyDb];
END

CREATE DATABASE [SklepWelnianyDb];
GO

-- After running this script you still need to apply EF migrations to create schema:
-- Use Visual Studio Package Manager Console:
--     Update-Database -Project SklepWelniany -StartupProject SklepWelniany
-- Or use dotnet-ef CLI (install tool if missing):
--     dotnet tool install --global dotnet-ef
--     dotnet ef database update --project SklepWelniany --startup-project SklepWelniany
