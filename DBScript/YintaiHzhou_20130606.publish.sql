

GO
PRINT N'Creating [dbo].[StoreReal]...';


GO
CREATE TABLE [dbo].[StoreReal] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (200) NOT NULL,
    [StoreNo]    VARCHAR (20)   NOT NULL,
    [UpdateDate] DATETIME       NULL,
    [UpdateUser] INT            NULL,
    CONSTRAINT [PK_StoreReal] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[StoreReal].[IX_StoreReal_StoreNo_Name]...';


GO
CREATE NONCLUSTERED INDEX [IX_StoreReal_StoreNo_Name]
    ON [dbo].[StoreReal]([StoreNo] DESC, [Name] DESC);


GO
PRINT N'Creating PK_StoreCoupons...';


GO
ALTER TABLE [dbo].[StoreCoupons]
    ADD CONSTRAINT [PK_StoreCoupons] PRIMARY KEY CLUSTERED ([Id] ASC);


GO
PRINT N'Creating [dbo].[StoreCoupons].[IX_StoreCoupons_Code_Status]...';


GO
CREATE NONCLUSTERED INDEX [IX_StoreCoupons_Code_Status]
    ON [dbo].[StoreCoupons]([Code] DESC, [Status] DESC);


GO
PRINT N'Creating [dbo].[StoreCoupons].[IX_StoreCoupons_Code_StorePromotionId]...';


GO
CREATE NONCLUSTERED INDEX [IX_StoreCoupons_Code_StorePromotionId]
    ON [dbo].[StoreCoupons]([Code] ASC, [StorePromotionId] ASC);


GO
PRINT N'Creating [dbo].[StoreCoupons].[IX_StoreCoupons_UserId_Status]...';


GO
CREATE NONCLUSTERED INDEX [IX_StoreCoupons_UserId_Status]
    ON [dbo].[StoreCoupons]([UserId] DESC, [Status] DESC);


GO
PRINT N'Creating [dbo].[StoreCoupons].[IX_StoreCoupons_UserId_Status_StorePromotionId_CreateDate]...';


GO
CREATE NONCLUSTERED INDEX [IX_StoreCoupons_UserId_Status_StorePromotionId_CreateDate]
    ON [dbo].[StoreCoupons]([UserId] DESC, [Status] DESC, [StorePromotionId] DESC, [CreateDate] DESC);


GO
PRINT N'Creating [dbo].[CouponLog].[IX_CouponLog_Code_Type_CreateDate]...';


GO
CREATE NONCLUSTERED INDEX [IX_CouponLog_Code_Type_CreateDate]
    ON [dbo].[CouponLog]([Code] DESC, [Type] DESC, [CreateDate] DESC);


GO
PRINT N'Creating [dbo].[StorePromotionScope].[IX_StoreCoupons_StorePromotionId_Status]...';


GO
CREATE NONCLUSTERED INDEX [IX_StoreCoupons_StorePromotionId_Status]
    ON [dbo].[StorePromotionScope]([StorePromotionId] DESC, [Status] DESC);


GO
PRINT N'Creating [dbo].[User].[IX_User_Name]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_Name]
    ON [dbo].[User]([Name] ASC);


GO
PRINT N'Creating DF_StoreReal_UpdateDate...';


GO
ALTER TABLE [dbo].[StoreReal]
    ADD CONSTRAINT [DF_StoreReal_UpdateDate] DEFAULT (getdate()) FOR [UpdateDate];


GO
PRINT N'Creating DF_StoreReal_UpdateUser...';


GO
ALTER TABLE [dbo].[StoreReal]
    ADD CONSTRAINT [DF_StoreReal_UpdateUser] DEFAULT ((0)) FOR [UpdateUser];


GO
PRINT N'Update complete.'
GO
