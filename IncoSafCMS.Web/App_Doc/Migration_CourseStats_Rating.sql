-- =====================================================
-- Migration: Add Course Statistics & CourseRating table
-- =====================================================

-- 1. Add statistics columns to Course table
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Course') AND name = 'ParticipantCount')
BEGIN
    ALTER TABLE [dbo].[Course] ADD [ParticipantCount] INT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Course') AND name = 'ViewCount')
BEGIN
    ALTER TABLE [dbo].[Course] ADD [ViewCount] INT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Course') AND name = 'AverageRating')
BEGIN
    ALTER TABLE [dbo].[Course] ADD [AverageRating] FLOAT NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Course') AND name = 'RatingCount')
BEGIN
    ALTER TABLE [dbo].[Course] ADD [RatingCount] INT NOT NULL DEFAULT 0;
END
GO

-- 2. Create CourseRating table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('CourseRating') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[CourseRating] (
        [Id]        INT           IDENTITY(1,1) NOT NULL,
        [CourseId]  INT           NOT NULL,
        [UserId]    INT           NOT NULL,
        [Stars]     INT           NOT NULL,
        [Comment]   NVARCHAR(500) NULL,
        [CreatedAt] DATETIME      NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT [PK_CourseRating] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_CourseRating_Course] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Course]([Id]) ON DELETE CASCADE,
        CONSTRAINT [CK_CourseRating_Stars] CHECK ([Stars] >= 1 AND [Stars] <= 5)
    );

    -- Each user can only rate a course once
    CREATE UNIQUE INDEX [IX_CourseRating_CourseId_UserId] ON [dbo].[CourseRating] ([CourseId], [UserId]);
END
GO

-- 3. Backfill existing statistics from ActivityLog
UPDATE c SET
    c.ParticipantCount = ISNULL(stats.DistinctUsers, 0),
    c.ViewCount = ISNULL(stats.TotalSessions, 0)
FROM [dbo].[Course] c
LEFT JOIN (
    SELECT
        a.RelatedId,
        COUNT(DISTINCT a.UserId) AS DistinctUsers,
        COUNT(*) AS TotalSessions
    FROM [dbo].[ActivityLog] a
    WHERE a.[Type] = 10  -- ActivityType.Learning
      AND a.RelatedId IS NOT NULL
    GROUP BY a.RelatedId
) stats ON stats.RelatedId = CAST(c.Id AS NVARCHAR(50));
GO

PRINT 'Migration completed: Course statistics columns + CourseRating table created.';
GO
