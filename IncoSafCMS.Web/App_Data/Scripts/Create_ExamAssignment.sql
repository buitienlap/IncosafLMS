-- =============================================
-- Create table ExamAssignment
-- for IncosafLMS database
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExamAssignment]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ExamAssignment] (
        [Id]                INT             IDENTITY(1,1) NOT NULL,
        [UserId]            INT             NOT NULL,
        [ExamTitle]         NVARCHAR(500)   NOT NULL,
        [Description]       NVARCHAR(2000)  NULL,
        [QuestionCount]     INT             NOT NULL DEFAULT 0,
        [TimeLimitMinutes]  INT             NULL,
        [Deadline]          DATETIME        NULL,
        [Status]            INT             NOT NULL DEFAULT 0,   -- 0=Pending, 1=InProgress, 2=Completed
        [Score]             DECIMAL(18,2)   NULL,
        [CreatedAt]         DATETIME        NOT NULL DEFAULT GETUTCDATE(),
        [CompletedAt]       DATETIME        NULL,
        [CategoryCodes]     NVARCHAR(500)   NULL,

        CONSTRAINT [PK_ExamAssignment] PRIMARY KEY CLUSTERED ([Id] ASC),

        CONSTRAINT [FK_ExamAssignment_AppUser] FOREIGN KEY ([UserId])
            REFERENCES [dbo].[AppUser] ([Id])
            ON DELETE CASCADE
    );

    -- Index for fast lookup: pending exams per user (used on Home dashboard)
    CREATE NONCLUSTERED INDEX [IX_ExamAssignment_UserId_Status]
        ON [dbo].[ExamAssignment] ([UserId], [Status])
        INCLUDE ([ExamTitle], [QuestionCount], [TimeLimitMinutes], [Deadline]);

    -- Index for deadline queries
    CREATE NONCLUSTERED INDEX [IX_ExamAssignment_Deadline]
        ON [dbo].[ExamAssignment] ([Deadline])
        WHERE [Status] <> 2;

    PRINT N'Table [ExamAssignment] created successfully.';
END
ELSE
BEGIN
    PRINT N'Table [ExamAssignment] already exists. Skipped.';
END
GO
