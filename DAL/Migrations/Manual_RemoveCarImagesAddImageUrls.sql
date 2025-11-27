-- Migration: Remove CarImages Table and Add ImageUrls to Cars
-- This migration removes the CarImages table and adds an ImageUrls column to the Cars table
-- Run this script on your database after backing up your data

BEGIN TRANSACTION;

-- Step 1: Drop foreign key constraint from CarImages table
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CarImages_Cars_CarId')
BEGIN
    ALTER TABLE [CarImages] DROP CONSTRAINT [FK_CarImages_Cars_CarId];
END
GO

-- Step 2: Drop CarImages table
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'CarImages')
BEGIN
    DROP TABLE [CarImages];
END
GO

-- Step 3: Add ImageUrls column to Cars table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Cars]') AND name = 'ImageUrls')
BEGIN
    ALTER TABLE [Cars] ADD [ImageUrls] NVARCHAR(MAX) NULL;
END
GO

COMMIT TRANSACTION;

-- Verification queries (optional)
-- SELECT TOP 10 * FROM Cars;
-- SELECT COUNT(*) FROM Cars WHERE ImageUrls IS NOT NULL;
