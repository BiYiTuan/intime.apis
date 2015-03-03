

GO
PRINT N'Creating [dbo].[PointOrderRule]...';


GO
CREATE TABLE [dbo].[PointOrderRule] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [StorePromotionId] INT             NOT NULL,
    [RangeFrom]        INT             NULL,
    [RangeTo]          INT             NULL,
    [Ratio]            DECIMAL (10, 2) NULL,
    [CreateDate]       DATETIME        NULL,
    [CreateUser]       INT             NULL,
    [Status]           INT             NULL,
    [UpdateDate]       DATETIME        NULL,
    [UpdateUser]       INT             NULL,
    CONSTRAINT [PK_PointOrderRule] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Section]...';


GO
CREATE TABLE [dbo].[Section] (
    [Location]      NVARCHAR (200) NULL,
    [ContactPhone]  VARCHAR (50)   NOT NULL,
    [Status]        INT            NULL,
    [BrandId]       INT            NULL,
    [StoreId]       INT            NULL,
    [CreateDate]    DATETIME       NULL,
    [CreateUser]    INT            NULL,
    [UpdateDate]    DATETIME       NULL,
    [UpdateUser]    INT            NULL,
    [ContactPerson] VARBINARY (50) NULL
);


GO
PRINT N'Creating [dbo].[StoreCoupons]...';


GO
CREATE TABLE [dbo].[StoreCoupons] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [Code]             VARCHAR (20)    NULL,
    [ValidStartDate]   DATETIME        NULL,
    [ValidEndDate]     DATETIME        NULL,
    [VipCard]          VARCHAR (50)    NULL,
    [Points]           INT             NULL,
    [Status]           INT             NULL,
    [StorePromotionId] INT             NULL,
    [CreateDate]       DATETIME        NULL,
    [CreateUser]       INT             NULL,
    [UpdateDate]       DATETIME        NULL,
    [UpdateUser]       INT             NULL,
    [UserId]           INT             NULL,
    [Amount]           DECIMAL (10, 2) NULL
);


GO
PRINT N'Creating [dbo].[StorePromotion]...';


GO
CREATE TABLE [dbo].[StorePromotion] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (100)  NULL,
    [Description]     NVARCHAR (500)  NULL,
    [ActiveStartDate] DATETIME        NULL,
    [ActiveEndDate]   DATETIME        NULL,
    [PromotionType]   INT             NULL,
    [AcceptPointType] INT             NULL,
    [Notice]          NVARCHAR (1000) NULL,
    [CouponStartDate] DATETIME        NULL,
    [CouponEndDate]   DATETIME        NULL,
    [MinPoints]       INT             NULL,
    [Status]          INT             NULL,
    [CreateDate]      DATETIME        NULL,
    [CreateUser]      INT             NULL,
    [UpdateDate]      DATETIME        NULL,
    [UpdateUser]      INT             NULL,
    [UsageNotice]     NVARCHAR (500)  NULL,
    [InScopeNotice]   NVARCHAR (1000) NULL,
    CONSTRAINT [PK_StorePromotion] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[StorePromotionScope]...';


GO
CREATE TABLE [dbo].[StorePromotionScope] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [StorePromotionId] INT            NULL,
    [StoreId]          INT            NULL,
    [StoreName]        NVARCHAR (50)  NULL,
    [Excludes]         NVARCHAR (200) NULL,
    [Status]           INT            NULL,
    [CreateDate]       DATETIME       NULL,
    [CreateUser]       INT            NULL,
    [UpdateDate]       DATETIME       NULL,
    [UpdateUser]       INT            NULL,
    CONSTRAINT [PK_StorePromotionScope] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[UserAuth]...';


GO
CREATE TABLE [dbo].[UserAuth] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [UserId]      INT      NOT NULL,
    [StoreId]     INT      NOT NULL,
    [BrandId]     INT      NULL,
    [Status]      INT      NULL,
    [CreatedDate] DATETIME NULL,
    [CreatedUser] INT      NULL,
    [Type]        INT      NOT NULL,
    CONSTRAINT [PK_UserAuth] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Update complete.'
GO
