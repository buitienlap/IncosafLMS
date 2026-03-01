-- =====================================================
-- Migration: Add ShuffledAnswerIds to PracticeExamAnswer
-- =====================================================
-- Stores the pre-selected and shuffled answer IDs for each question in the exam.
-- This ensures:
--   1. Questions with more answers than the max display limit show only a random subset
--   2. The correct answer is always included in the subset
--   3. Answer order is randomized and consistent across page loads

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PracticeExamAnswer') AND name = 'ShuffledAnswerIds')
BEGIN
    ALTER TABLE [dbo].[PracticeExamAnswer] ADD [ShuffledAnswerIds] NVARCHAR(MAX) NULL;
END
GO
