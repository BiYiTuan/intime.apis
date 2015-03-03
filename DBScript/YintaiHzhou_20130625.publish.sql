
GO
ALTER TABLE [dbo].[CouponHistory]
    ADD [IsLimitOnce] BIT NULL;


GO
PRINT N'Altering [dbo].[Product]...';


GO
ALTER TABLE [dbo].[Product]
    ADD [Is4Sale] BIT NULL;


GO
PRINT N'Altering [dbo].[ProductStage]...';


GO
ALTER TABLE [dbo].[ProductStage]
    ADD [UnitPrice] DECIMAL (10, 2) NULL,
        [Is4Sale]   BIT             NULL;


GO
PRINT N'Altering [dbo].[Promotion]...';


GO
ALTER TABLE [dbo].[Promotion]
    ADD [IsCodeUseLimit] BIT NULL;


GO
PRINT N'Altering [dbo].[Resources]...';


GO
ALTER TABLE [dbo].[Resources]
    ADD [IsDimension] BIT NULL;


GO
PRINT N'Altering [dbo].[ResourceStage]...';


GO
ALTER TABLE [dbo].[ResourceStage]
    ADD [IsDimension] BIT NULL;


GO
PRINT N'Creating [dbo].[CategoryProperty]...';


GO
CREATE TABLE [dbo].[CategoryProperty] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [CategoryId]   INT           NOT NULL,
    [PropertyDesc] NVARCHAR (50) NOT NULL,
    [SortOrder]    INT           NOT NULL,
    [Status]       INT           NOT NULL,
    [CreatedUser]  INT           NOT NULL,
    [CreatedDate]  DATETIME      NOT NULL,
    [UpdatedUser]  INT           NOT NULL,
    [UpdatedDate]  DATETIME      NOT NULL,
    [IsVisible]    BIT           NULL,
    CONSTRAINT [PK_CategoryProperty] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[CategoryPropertyValue]...';


GO
CREATE TABLE [dbo].[CategoryPropertyValue] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ValueDesc]  NVARCHAR (200) NOT NULL,
    [SortOrder]  INT            NOT NULL,
    [Status]     INT            NOT NULL,
    [CreateUser] INT            NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    [UpdateUser] INT            NOT NULL,
    [UpdateDate] DATETIME       NOT NULL,
    [PropertyId] INT            NOT NULL,
    CONSTRAINT [PK_CategoryPropertyValue] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[City]...';


GO
CREATE TABLE [dbo].[City] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (100) NOT NULL,
    [IsProvince] BIT            NOT NULL,
    [ParentId]   INT            NULL,
    [Status]     INT            NOT NULL,
    [UpdateDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[City].[IX_City]...';


GO
CREATE NONCLUSTERED INDEX [IX_City]
    ON [dbo].[City]([Status] ASC, [IsProvince] ASC);


GO
PRINT N'Creating [dbo].[City].[IX_City_1]...';


GO
CREATE NONCLUSTERED INDEX [IX_City_1]
    ON [dbo].[City]([Status] ASC, [ParentId] ASC);


GO
PRINT N'Creating [dbo].[InboundPackage]...';


GO
CREATE TABLE [dbo].[InboundPackage] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [SourceNo]    VARCHAR (20) NOT NULL,
    [SourceType]  INT          NOT NULL,
    [ShippingVia] INT          NOT NULL,
    [ShippingNo]  VARCHAR (50) NOT NULL,
    [CreateDate]  DATETIME     NOT NULL,
    [CreateUser]  INT          NOT NULL
);


GO
PRINT N'Creating [dbo].[Order]...';


GO
CREATE TABLE [dbo].[Order] (
    [Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [OrderNo]               VARCHAR (20)    NOT NULL,
    [CustomerId]            INT             NOT NULL,
    [TotalAmount]           DECIMAL (10, 2) NOT NULL,
    [RecAmount]             DECIMAL (10, 2) NULL,
    [Status]                INT             NOT NULL,
    [PaymentMethodCode]     VARCHAR (10)    NOT NULL,
    [PaymentMethodName]     NVARCHAR (20)   NULL,
    [ShippingZipCode]       VARCHAR (20)    NULL,
    [ShippingAddress]       NVARCHAR (500)  NULL,
    [ShippingContactPerson] NVARCHAR (10)   NULL,
    [ShippingContactPhone]  VARCHAR (20)    NULL,
    [NeedInvoice]           BIT             NULL,
    [InvoiceSubject]        NVARCHAR (200)  NULL,
    [InvoiceDetail]         NVARCHAR (200)  NULL,
    [ShippingFee]           DECIMAL (10, 2) NULL,
    [CreateDate]            DATETIME        NOT NULL,
    [CreateUser]            INT             NOT NULL,
    [UpdateDate]            DATETIME        NOT NULL,
    [UpdateUser]            INT             NOT NULL,
    [ShippingNo]            VARCHAR (50)    NULL,
    [ShippingVia]           INT             NULL,
    [StoreId]               INT             NOT NULL,
    [BrandId]               INT             NOT NULL,
    [Memo]                  NVARCHAR (200)  NULL,
    [InvoiceAmount]         DECIMAL (10, 2) NULL,
    [TotalPoints]           INT             NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Order].[IX_Order]...';


GO
CREATE NONCLUSTERED INDEX [IX_Order]
    ON [dbo].[Order]([OrderNo] ASC, [Status] ASC);


GO
PRINT N'Creating [dbo].[OrderItem]...';


GO
CREATE TABLE [dbo].[OrderItem] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [OrderNo]       VARCHAR (20)    NOT NULL,
    [ProductId]     INT             NOT NULL,
    [ProductDesc]   NVARCHAR (200)  NOT NULL,
    [StoreItemNo]   VARCHAR (20)    NULL,
    [StoreItemDesc] NVARCHAR (200)  NULL,
    [Quantity]      INT             NOT NULL,
    [UnitPrice]     DECIMAL (10, 2) NULL,
    [ItemPrice]     DECIMAL (10, 2) NOT NULL,
    [ExtendPrice]   DECIMAL (10, 2) NOT NULL,
    [BrandId]       INT             NOT NULL,
    [StoreId]       INT             NOT NULL,
    [CreateUser]    INT             NOT NULL,
    [CreateDate]    DATETIME        NOT NULL,
    [UpdateUser]    INT             NOT NULL,
    [UpdateDate]    DATETIME        NOT NULL,
    [Status]        INT             NOT NULL,
    [ProductName]   NVARCHAR (200)  NULL,
    [Points]        INT             NULL,
    CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[OrderItem].[IX_OrderItem]...';


GO
CREATE NONCLUSTERED INDEX [IX_OrderItem]
    ON [dbo].[OrderItem]([OrderNo] ASC, [Status] ASC);


GO
PRINT N'Creating [dbo].[OrderLog]...';


GO
CREATE TABLE [dbo].[OrderLog] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [OrderNo]    VARCHAR (20)   NOT NULL,
    [CustomerId] INT            NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    [CreateUser] INT            NOT NULL,
    [Operation]  NVARCHAR (200) NULL,
    [Type]       INT            NOT NULL,
    CONSTRAINT [PK_OrderLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Outbound]...';


GO
CREATE TABLE [dbo].[Outbound] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [OutboundNo]            VARCHAR (20)   NOT NULL,
    [SourceNo]              VARCHAR (20)   NOT NULL,
    [SourceType]            INT            NOT NULL,
    [ShippingVia]           INT            NULL,
    [ShippingAddress]       NVARCHAR (500) NULL,
    [ShippingContactPerson] NVARCHAR (10)  NULL,
    [ShippingContactPhone]  VARCHAR (20)   NULL,
    [ShippingZipCode]       VARCHAR (20)   NULL,
    [Status]                INT            NOT NULL,
    [CreateUser]            INT            NOT NULL,
    [CreateDate]            DATETIME       NOT NULL,
    [UpdateUser]            INT            NOT NULL,
    [UpdateDate]            DATETIME       NOT NULL,
    [ShippingNo]            VARCHAR (50)   NULL,
    CONSTRAINT [PK_Outbound] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[OutboundItem]...';


GO
CREATE TABLE [dbo].[OutboundItem] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [OutboundNo]    VARCHAR (20)    NOT NULL,
    [ProductId]     INT             NOT NULL,
    [ProductDesc]   NVARCHAR (200)  NULL,
    [StoreItemNo]   VARCHAR (50)    NULL,
    [StoreItemDesc] NVARCHAR (200)  NULL,
    [Quantity]      INT             NOT NULL,
    [ItemPrice]     DECIMAL (10, 2) NOT NULL,
    [ExtendPrice]   DECIMAL (10, 2) NOT NULL,
    [CreateDate]    DATETIME        NOT NULL,
    [UpdateDate]    DATETIME        NOT NULL,
    [UnitPrice]     DECIMAL (10, 2) NOT NULL,
    CONSTRAINT [PK_OutboundItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[PaymentMethod]...';


GO
CREATE TABLE [dbo].[PaymentMethod] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Code]       VARCHAR (10)  NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Status]     INT           NOT NULL,
    [UpdateDate] DATETIME      NOT NULL,
    [UpdateUser] INT           NOT NULL,
    [IsCOD]      BIT           NOT NULL,
    CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[ProductProperty]...';


GO
CREATE TABLE [dbo].[ProductProperty] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [ProductId]    INT           NOT NULL,
    [PropertyDesc] NVARCHAR (50) NOT NULL,
    [Status]       INT           NOT NULL,
    [UpdateDate]   DATETIME      NOT NULL,
    [UpdateUser]   INT           NOT NULL,
    [SortOrder]    INT           NULL,
    CONSTRAINT [PK_ProductProperty] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[ProductPropertyStage]...';


GO
CREATE TABLE [dbo].[ProductPropertyStage] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ItemCode]      VARCHAR (100)  NOT NULL,
    [PropertyDesc]  NVARCHAR (50)  NOT NULL,
    [ValueDesc]     NVARCHAR (200) NOT NULL,
    [SortOrder]     INT            NOT NULL,
    [UploadGroupId] INT            NOT NULL,
    [Status]        INT            NULL,
    CONSTRAINT [PK_ProductPropertyStage] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[ProductPropertyStage].[IX_ProductPropertyStage]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductPropertyStage]
    ON [dbo].[ProductPropertyStage]([UploadGroupId] ASC, [ItemCode] ASC);


GO
PRINT N'Creating [dbo].[ProductPropertyValue]...';


GO
CREATE TABLE [dbo].[ProductPropertyValue] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [PropertyId] INT           NOT NULL,
    [ValueDesc]  NVARCHAR (50) NOT NULL,
    [CreateDate] DATETIME      NOT NULL,
    [UpdateDate] DATETIME      NOT NULL,
    [Status]     INT           NOT NULL,
    CONSTRAINT [PK_ProductPropertyValue] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[RMA]...';


GO
CREATE TABLE [dbo].[RMA] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [RMANo]             VARCHAR (20)    NOT NULL,
    [OrderNo]           VARCHAR (20)    NOT NULL,
    [RMAType]           INT             NOT NULL,
    [Status]            INT             NOT NULL,
    [Reason]            NVARCHAR (500)  NULL,
    [RMAAmount]         DECIMAL (10, 2) NOT NULL,
    [CreateUser]        INT             NOT NULL,
    [CreateDate]        DATETIME        NOT NULL,
    [UpdateUser]        INT             NOT NULL,
    [UpdateDate]        DATETIME        NOT NULL,
    [BankName]          NVARCHAR (200)  NULL,
    [BankAccount]       NVARCHAR (20)   NULL,
    [BankCard]          VARCHAR (50)    NULL,
    [RejectReason]      NVARCHAR (500)  NULL,
    [rebatepostfee]     DECIMAL (10, 2) NULL,
    [chargepostfee]     DECIMAL (10, 2) NULL,
    [ActualAmount]      DECIMAL (10, 2) NULL,
    [GiftReason]        NVARCHAR (100)  NULL,
    [InvoiceReason]     NVARCHAR (100)  NULL,
    [RebatePointReason] NVARCHAR (100)  NULL,
    [PostalFeeReason]   NVARCHAR (100)  NULL,
    [ChargeGiftFee]     DECIMAL (10, 2) NULL,
    CONSTRAINT [PK_RMA] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[RMAItem]...';


GO
CREATE TABLE [dbo].[RMAItem] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [RMANo]       VARCHAR (20)    NOT NULL,
    [ProductId]   INT             NOT NULL,
    [ProductDesc] NVARCHAR (200)  NOT NULL,
    [StoreItem]   VARCHAR (50)    NULL,
    [StoreDesc]   NVARCHAR (200)  NULL,
    [ItemPrice]   DECIMAL (10, 2) NOT NULL,
    [Quantity]    INT             NOT NULL,
    [ExtendPrice] DECIMAL (10, 2) NOT NULL,
    [CreateDate]  DATETIME        NOT NULL,
    [UpdateDate]  DATETIME        NOT NULL,
    [Status]      INT             NOT NULL,
    [UnitPrice]   DECIMAL (10, 2) NULL,
    CONSTRAINT [PK_RMAItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[RMALog]...';


GO
CREATE TABLE [dbo].[RMALog] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RMANo]      VARCHAR (20)   NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    [CreateUser] INT            NOT NULL,
    [Operation]  NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_RMALog] PRIMARY KEY CLUSTERED ([Id] ASC)
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
    [ContactPerson] NVARCHAR (20)  NULL,
    [Name]          NVARCHAR (200) NOT NULL,
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_Section] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[ShippingAddress]...';


GO
CREATE TABLE [dbo].[ShippingAddress] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [UserId]                INT            NOT NULL,
    [ShippingZipCode]       VARCHAR (20)   NULL,
    [ShippingAddress]       NVARCHAR (500) NULL,
    [ShippingContactPerson] NVARCHAR (10)  NULL,
    [ShippingContactPhone]  VARCHAR (20)   NULL,
    [UpdateDate]            DATETIME       NULL,
    [UpdateUser]            INT            NULL,
    [Status]                INT            NULL,
    [ShippingProvinceId]    INT            NULL,
    [ShippingCityId]        INT            NULL,
    [ShippingProvince]      NVARCHAR (50)  NULL,
    [ShippingCity]          NVARCHAR (50)  NULL,
    CONSTRAINT [PK_ShippingAddress] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[ShippingAddress].[IX_ShippingAddress]...';


GO
CREATE NONCLUSTERED INDEX [IX_ShippingAddress]
    ON [dbo].[ShippingAddress]([UserId] ASC, [Status] ASC);


GO
PRINT N'Creating [dbo].[ShipVia]...';


GO
CREATE TABLE [dbo].[ShipVia] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50)  NOT NULL,
    [Url]        NVARCHAR (200) NULL,
    [Status]     INT            NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    [UpdateDate] DATETIME       NOT NULL
);


GO
PRINT N'Altering [dbo].[ProductStagePublish2]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[ProductStagePublish2]
(
	@inUser int,
	@jobId int
)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON
	IF (@inUser IS NULL OR
		@inUser = 0)
		RETURN
	DECLARE @Id int, @Promotions varchar(200),@Subjects varchar(200), @ItemCode varchar(100)
	DECLARE @PId int
	DECLARE @OutTable TABLE(ItemCode varchar(64), Status int,PublishMemo varchar(100))
	DECLARE stage_cursor CURSOR FOR 
	SELECT id 
		,Promotions
		,ItemCode
		,Subjects
	FROM DBO.ProductStage WITH(NOLOCK)
	WHERE inUserId = @inUser
	AND UploadGroupId = @jobId
	AND Status = 4
	ORDER BY ItemCode asc

	OPEN stage_cursor
	FETCH NEXT FROM stage_cursor 
	INTO @Id,@Promotions, @ItemCode,@Subjects

	WHILE @@FETCH_STATUS = 0
	BEGIN
		BEGIN TRY
		BEGIN TRANSACTION
			--CREATE PRODUCT
			INSERT INTO DBO.Product
			(Name,Brand_Id,Description,CreatedDate,CreatedUser,
				UpdatedDate,UpdatedUser,Price,RecommendedReason,
				RecommendUser,Store_Id,Tag_Id,SortOrder,Status,UnitPrice,Is4Sale)
			SELECT ps.name,
			B.Id,
			ps.Description+ISNULL(ps.DescripOfPromotion,''),
			GETDATE(),
			ps.InUserId,
			GETDATE(),
			ps.InUserId,
			ISNULL(ps.Price,0),
			ps.Description+ISNULL(ps.DescripOfPromotion,''),
			ps.InUserId,
			s.Id,
			t.Id,
			0,
			1,
			UnitPrice,
			Is4Sale
			 FROM DBO.ProductStage ps WITH(NOLOCK)
			Inner join DBO.Brand b WITH(NOLOCK)
				ON B.Name = ps.BrandName
			LEFT JOIN DBO.Tag t with(NOLOCK)
				ON t.Name = ps.Tag
			INNER JOIN DBO.Store s with(NOLOCK)
				ON s.Name = PS.Store
			WHERE ps.id = @Id
			SET @PId = SCOPE_IDENTITY();
			--CREATE RESOURCE
			DECLARE @resourceInsert TABLE(Id int)
			DELETE FROM @resourceInsert
			UPDATE DBO.ResourceStage
			SET Status= 1
			OUTPUT deleted.id INTO @resourceInsert(Id)
			WHERE Status = 0
				AND InUser = @inUser
				AND UploadGroupId = @jobId
				AND ItemCode = @ItemCode
			 
			 INSERT INTO Resources
			(SourceId,SourceType,Name,CreatedUser,CreatedDate,
			 UpdatedDate,UpdatedUser,SortOrder,Type,Status,Size
			 ,Width,Height,ContentSize,ExtName,IsDimension)
			 SELECT @PId,
				1,
				rs.Name,
				rs.InUser,
				GETDATE(),
				GETDATE(),
				rs.InUser,
				rs.SortOrder,
				1,
				1,
				rs.Size,
				rs.Width,
				rs.Height,
				rs.ContentSize,
				rs.ExtName,
				rs.IsDimension
			 FROM dbo.ResourceStage rs WITH(NOLOCK)
			 WHERE EXISTS (SELECT 1 FROM @resourceInsert ri
							WHERE ri.Id = rs.id)
			--CREATE Subject-Prod relation
			IF (@Subjects IS NOT NULL 
			AND LEN(LTRIM(RTRIM(@Subjects)))>0 )
				INSERT INTO DBO.SpecialTopicProductRelations
				(SpecialTopic_Id,Product_Id,Status,CreatedUser,CreatedDate,UpdatedUser,UpdatedDate)
				SELECT PS.ID,
						@PId,
						1,
						@inUser,
						GETDATE(),
						@inUser,
						GETDATE()
				FROM DBO.fn_ParseDelimString(@Subjects,',') PS
				WHERE PS.ID>0
			--CREATE Promotion-Prod relation
			IF (@Promotions IS NOT NULL 
			AND LEN(LTRIM(RTRIM(@Promotions)))>0 )
				INSERT INTO DBO.Promotion2Product
				(ProdID,ProId,Status)
				SELECT @PId,
						PP.ID,
						1
				FROM DBO.fn_ParseDelimString(@Promotions,',') PP
				WHERE PP.ID >0
				
			--insert property
			DECLARE @propertyInsert TABLE(Id int,PropertyDesc nvarchar(50))
			DELETE FROM @propertyInsert
			INSERT INTO DBO.ProductProperty
			(ProductId,PropertyDesc,Status,UpdateDate,UpdateUser,SortOrder)
			OUTPUT inserted.Id,inserted.PropertyDesc into @propertyInsert(Id,PropertyDesc)
			SELECT @PId,PropertyDesc,1,GETDATE(),0,0
			FROM (
			SELECT DISTINCT PropertyDesc
			FROM ProductPropertyStage WITH(NOLOCK)
			WHERE ItemCode = @ItemCode and UploadGroupId = @jobId) AS DP
			
			INSERT INTO DBO.ProductPropertyValue
			(PropertyId,ValueDesc,CreateDate,UpdateDate,Status)
			SELECT Id,DPV.ValueDesc,GETDATE(),GETDATE(),1
			FROM (
			SELECT DISTINCT P.Id,PPS.ValueDesc
			FROM DBO.ProductPropertyStage AS PPS 
			JOIN @propertyInsert as P on p.PropertyDesc = PPS.PropertyDesc
			WHERE PPS.UploadGroupId = @jobId AND PPS.ItemCode = @ItemCode) AS DPV
			
			--UPDATE STAGE STATUS
			UPDATE DBO.ProductStage
			SET Status= 5
			WHERE id = @Id
			UPDATE DBO.ProductUploadJob
			SET Status = 5
			WHERE id = @jobId
			INSERT INTO @OutTable(ItemCode,Status,PublishMemo)
			VALUES
			(
				@ItemCode,
				5,
				''
			)

			
		  COMMIT TRANSACTION
		END TRY
		BEGIN CATCH
			PRINT ERROR_NUMBER();
			PRINT ERROR_SEVERITY();
			PRINT ERROR_STATE();
			PRINT ERROR_PROCEDURE() 
			PRINT ERROR_LINE() 
			PRINT ERROR_MESSAGE() 
			IF (XACT_STATE()) = -1
			BEGIN
				ROLLBACK TRANSACTION;
			END;
			IF (XACT_STATE()) = 1
			BEGIN
				COMMIT TRANSACTION;   
			END;
			INSERT INTO @OutTable(ItemCode,Status,PublishMemo)
			VALUES
			(
				@ItemCode,
				4,
				''
			)
		END CATCH
		-- Get the next vendor.
		FETCH NEXT FROM stage_cursor 
		INTO @Id, @Promotions, @ItemCode,@Subjects
	END 

	CLOSE stage_cursor;
	DEALLOCATE stage_cursor;
	SELECT * FROM @OutTable

END
GO
PRINT N'Altering [dbo].[ProductBulkDelete]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[ProductBulkDelete]
(
	@jobid int
)
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRY
		DELETE FROM DBO.ResourceStage 
		WHERE UploadGroupId = @jobId
		DELETE FROM DBO.ProductStage
		WHERE UploadGroupId = @jobId
		DELETE FROM DBO.ProductUploadJob
		WHERE ID =@jobId
		DELETE FROM DBO.ProductPropertyStage
		WHERE UploadGroupId = @jobId
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
			
			IF (XACT_STATE()) = -1
			BEGIN
				ROLLBACK TRANSACTION;
			END;
			IF (XACT_STATE()) = 1
			BEGIN
				COMMIT TRANSACTION;   
			END;
			
	END CATCH
END
GO
PRINT N'Refreshing [dbo].[ProductStageValidate]...';


GO
EXECUTE sp_refreshsqlmodule N'dbo.ProductStageValidate';


GO
PRINT N'Refreshing [dbo].[Promotion_GetPagedListByCoordinateAndTs]...';


GO
EXECUTE sp_refreshsqlmodule N'dbo.Promotion_GetPagedListByCoordinateAndTs';


GO
PRINT N'Refreshing [dbo].[Promotion_GetPagedListByCoordinate]...';


GO
EXECUTE sp_refreshsqlmodule N'dbo.Promotion_GetPagedListByCoordinate';


GO
PRINT N'Refreshing [dbo].[Promotion_GetListByCoordinate]...';


GO
EXECUTE sp_refreshsqlmodule N'dbo.Promotion_GetListByCoordinate';


GO
PRINT N'Update complete.'
GO
