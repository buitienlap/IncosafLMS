-- =============================================
-- ALTER table Course: add content fields
-- for IncosafLMS database
-- =============================================

-- ContentType: 0=Html, 1=Pdf, 2=PowerPoint, 3=Video
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = 'ContentType')
BEGIN
    ALTER TABLE [dbo].[Course] ADD [ContentType] INT NOT NULL DEFAULT 0;
    PRINT N'Column [ContentType] added.';
END
GO

-- ContentBody: HTML body content (nvarchar max)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = 'ContentBody')
BEGIN
    ALTER TABLE [dbo].[Course] ADD [ContentBody] NVARCHAR(MAX) NULL;
    PRINT N'Column [ContentBody] added.';
END
GO

-- ContentUrl: path to PDF/PPTX file or video streaming URL
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = 'ContentUrl')
BEGIN
    ALTER TABLE [dbo].[Course] ADD [ContentUrl] NVARCHAR(1000) NULL;
    PRINT N'Column [ContentUrl] added.';
END
GO

-- DurationMinutes: estimated course duration
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND name = 'DurationMinutes')
BEGIN
    ALTER TABLE [dbo].[Course] ADD [DurationMinutes] INT NULL;
    PRINT N'Column [DurationMinutes] added.';
END
GO

PRINT N'ALTER Course completed.';
GO
