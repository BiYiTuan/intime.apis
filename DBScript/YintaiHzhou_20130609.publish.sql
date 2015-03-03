
GO
PRINT N'Creating [dbo].[WXReply]...';


GO
CREATE TABLE [dbo].[WXReply] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [MatchKey]   NVARCHAR (100)  NOT NULL,
    [ReplyMsg]   NVARCHAR (2000) NOT NULL,
    [Status]     INT             NOT NULL,
    [UpdateDate] DATETIME        NOT NULL,
    CONSTRAINT [PK_WXReply] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[WXReply].[IX_WXReply]...';


GO
CREATE NONCLUSTERED INDEX [IX_WXReply]
    ON [dbo].[WXReply]([MatchKey] ASC, [Status] ASC);


GO
PRINT N'Update complete.'
GO

CREATE TABLE [dbo].[CardBlack](
	[Id] [int] NOT NULL,
	[CardNo] [varchar](128) NOT NULL,
	[Status] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_CardBlack] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_CardBlack] ON [dbo].[CardBlack] 
(
	[CardNo] ASC,
	[Status] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
