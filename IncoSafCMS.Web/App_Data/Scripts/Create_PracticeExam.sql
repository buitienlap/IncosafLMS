-- ============================================================
-- Create PracticeExam and PracticeExamAnswer tables
-- for the Practice Exam feature
-- ============================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PracticeExam')
BEGIN
    CREATE TABLE [dbo].[PracticeExam] (
        [Id]                    INT             IDENTITY(1,1) NOT NULL,
        [UserId]                INT             NOT NULL,
        [Title]                 NVARCHAR(500)   NOT NULL,
        [QuestionCount]         INT             NOT NULL DEFAULT 0,
        [CorrectCount]          INT             NOT NULL DEFAULT 0,
        [Score]                 DECIMAL(5,1)    NULL,
        [Status]                INT             NOT NULL DEFAULT 0,  -- 0=InProgress, 1=Completed
        [StartedAt]             DATETIME        NOT NULL DEFAULT GETUTCDATE(),
        [CompletedAt]           DATETIME        NULL,
        [DurationSeconds]       INT             NULL,
        [CategoryFilter]        NVARCHAR(500)   NULL,
        [DifficultyFilter]      NVARCHAR(100)   NULL,
        [CurrentQuestionIndex]  INT             NOT NULL DEFAULT 0,
        [QuestionIds]           NVARCHAR(MAX)   NULL,
        CONSTRAINT [PK_PracticeExam] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_PracticeExam_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers]([Id])
    );

    CREATE NONCLUSTERED INDEX [IX_PracticeExam_UserId] ON [dbo].[PracticeExam] ([UserId]);
    CREATE NONCLUSTERED INDEX [IX_PracticeExam_Status] ON [dbo].[PracticeExam] ([Status]);

    PRINT 'Created table PracticeExam';
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PracticeExamAnswer')
BEGIN
    CREATE TABLE [dbo].[PracticeExamAnswer] (
        [Id]                INT         IDENTITY(1,1) NOT NULL,
        [PracticeExamId]    INT         NOT NULL,
        [QuestionId]        INT         NOT NULL,
        [SelectedAnswerId]  INT         NULL,
        [IsCorrect]         BIT         NULL,
        [QuestionOrder]     INT         NOT NULL DEFAULT 0,
        [AnsweredAt]        DATETIME    NULL,
        CONSTRAINT [PK_PracticeExamAnswer] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_PracticeExamAnswer_PracticeExam] FOREIGN KEY ([PracticeExamId]) REFERENCES [dbo].[PracticeExam]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PracticeExamAnswer_Question] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Questions]([Id]),
        CONSTRAINT [FK_PracticeExamAnswer_Answer] FOREIGN KEY ([SelectedAnswerId]) REFERENCES [dbo].[Answers]([Id])
    );

    CREATE NONCLUSTERED INDEX [IX_PracticeExamAnswer_PracticeExamId] ON [dbo].[PracticeExamAnswer] ([PracticeExamId]);
    CREATE NONCLUSTERED INDEX [IX_PracticeExamAnswer_QuestionId] ON [dbo].[PracticeExamAnswer] ([QuestionId]);

    PRINT 'Created table PracticeExamAnswer';
END
GO
