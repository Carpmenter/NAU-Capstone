
CREATE TABLE [dbo].[Admin] (
    [username] VARCHAR (50) NOT NULL,
    [password] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED ([username] ASC)
);

CREATE TABLE [dbo].[Category] (
    [ID] INT IDENTITY (1, 1) NOT NULL,
    [Name]  VARCHAR (100)  NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[Group] (
    [ID] INT IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (100)  NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[Survey] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [Description]  NVARCHAR (MAX) NOT NULL,
    [creationDate] DATE           NOT NULL,
    CONSTRAINT [PK_Survey] PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[Participant] (
    [ID] INT           IDENTITY (1, 1) NOT NULL,
    [GroupID]       INT           NULL,
    [SurveyID]      INT           NOT NULL,
    [Username]      NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Participant] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Participant_Survey_SurveyID] FOREIGN KEY ([SurveyID]) REFERENCES [dbo].[Survey] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Participant_Group_GroupID] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Question] (
    [ID] INT            IDENTITY (1, 1) NOT NULL,
    [GroupID]    INT  NULL,
    [CategoryID] INT  NULL,
    [Type]       INT            NOT NULL,
    [Text]       NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Question_Group_GroupID] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Question_Category_CategoryID] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[SurveyQuestion] (
    [SurveyID]   INT NOT NULL,
    [QuestionID] INT NOT NULL,
    CONSTRAINT [FK_SurveyQuestion_Survey_SurveyID] FOREIGN KEY ([SurveyID]) REFERENCES [dbo].[Survey] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_SurveyQuestion_Question_QuestionID] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[Question] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[SurveyResponse] (
    [ID]    INT            IDENTITY (1, 1) NOT NULL,
    [QuestionID]    INT            NOT NULL,
    [SurveyID]      INT            NOT NULL,
    [ParticipantID] INT            NOT NULL,
    [Score]         INT            NULL,
    [Comment]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SurveyResponse] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SurveyResponse_Question_QuestionID] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[Question] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_SurveyResponse_Survey_SurveyID] FOREIGN KEY ([SurveyID]) REFERENCES [dbo].[Survey] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_SurveyResponse_Participant_ParticipantID] FOREIGN KEY ([ParticipantID]) REFERENCES [dbo].[Participant] ([ID])
);
