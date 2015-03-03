

GO
ALTER TABLE [dbo].[NotificationLog] DROP COLUMN [DeviceToken], COLUMN [InDate], COLUMN [Message];


GO
ALTER TABLE [dbo].[NotificationLog]
    ADD [SourceType] INT      NULL,
        [SourceId]   INT      NULL,
        [CreateDate] DATETIME NULL;


GO
PRINT N'Altering [dbo].[Promotion]...';


GO
ALTER TABLE [dbo].[Promotion]
    ADD [IsMain] BIT NULL;


GO
PRINT N'Altering [dbo].[SpecialTopic]...';


GO
ALTER TABLE [dbo].[SpecialTopic]
    ADD [TargetValue] NVARCHAR (500) NULL;

GO
ALTER TABLE [dbo].[StoreCoupons]
    ADD [StoreId] INT NULL;
	GO
ALTER TABLE [dbo].[StorePromotion]
    ADD [UnitPerPoints] BIT NULL;