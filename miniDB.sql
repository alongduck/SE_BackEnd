IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Categories] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);

CREATE TABLE [NewsArticles] (
    [Id] bigint NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [TimeUp] datetime2 NOT NULL,
    CONSTRAINT [PK_NewsArticles] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [Avatar] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    [Role] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [MinIOs] (
    [Id] bigint NOT NULL,
    [FileName] nvarchar(128) NOT NULL,
    [ETag] nvarchar(max) NOT NULL,
    [Size] bigint NOT NULL,
    [TimeUp] datetime2 NOT NULL,
    [ProductDetailId] bigint NULL,
    [NewsArticleId] bigint NULL,
    CONSTRAINT [PK_MinIOs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MinIOs_NewsArticles_NewsArticleId] FOREIGN KEY ([NewsArticleId]) REFERENCES [NewsArticles] ([Id])
);

CREATE TABLE [ProductDetails] (
    [Id] bigint NOT NULL IDENTITY,
    [Description] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [PricePerSquareMeter] float NOT NULL,
    [Features] nvarchar(max) NOT NULL,
    [Area] float NOT NULL,
    [Length] float NOT NULL,
    [Width] float NOT NULL,
    [Structure] nvarchar(max) NOT NULL,
    [ProductId] bigint NOT NULL,
    CONSTRAINT [PK_ProductDetails] PRIMARY KEY ([Id])
);

CREATE TABLE [Products] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(450) NOT NULL,
    [Thumbnail] nvarchar(max) NULL,
    [Price] float NOT NULL,
    [Hot] bit NOT NULL,
    [TimeUp] datetime2 NOT NULL,
    [CategoryId] bigint NOT NULL,
    [DetailId] bigint NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]),
    CONSTRAINT [FK_Products_ProductDetails_DetailId] FOREIGN KEY ([DetailId]) REFERENCES [ProductDetails] ([Id]),
    CONSTRAINT [FK_Products_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE INDEX [IX_MinIOs_NewsArticleId] ON [MinIOs] ([NewsArticleId]);

CREATE INDEX [IX_MinIOs_ProductDetailId] ON [MinIOs] ([ProductDetailId]);

CREATE INDEX [IX_ProductDetails_ProductId] ON [ProductDetails] ([ProductId]);

CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);

CREATE UNIQUE INDEX [IX_Products_DetailId] ON [Products] ([DetailId]) WHERE [DetailId] IS NOT NULL;

CREATE INDEX [IX_Products_Name] ON [Products] ([Name]);

CREATE INDEX [IX_Products_UserId] ON [Products] ([UserId]);

ALTER TABLE [MinIOs] ADD CONSTRAINT [FK_MinIOs_ProductDetails_ProductDetailId] FOREIGN KEY ([ProductDetailId]) REFERENCES [ProductDetails] ([Id]);

ALTER TABLE [ProductDetails] ADD CONSTRAINT [FK_ProductDetails_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241209080146_Init0', N'9.0.0');

DROP INDEX [IX_Products_DetailId] ON [Products];

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'DetailId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var0 + '];');
UPDATE [Products] SET [DetailId] = CAST(0 AS bigint) WHERE [DetailId] IS NULL;
ALTER TABLE [Products] ALTER COLUMN [DetailId] bigint NOT NULL;
ALTER TABLE [Products] ADD DEFAULT CAST(0 AS bigint) FOR [DetailId];

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ProductDetails]') AND [c].[name] = N'Width');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [ProductDetails] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [ProductDetails] ALTER COLUMN [Width] float NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ProductDetails]') AND [c].[name] = N'Structure');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [ProductDetails] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [ProductDetails] ALTER COLUMN [Structure] nvarchar(max) NULL;

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ProductDetails]') AND [c].[name] = N'PricePerSquareMeter');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [ProductDetails] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [ProductDetails] ALTER COLUMN [PricePerSquareMeter] float NULL;

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ProductDetails]') AND [c].[name] = N'Length');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [ProductDetails] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [ProductDetails] ALTER COLUMN [Length] float NULL;

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ProductDetails]') AND [c].[name] = N'Features');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [ProductDetails] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [ProductDetails] ALTER COLUMN [Features] nvarchar(max) NULL;

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ProductDetails]') AND [c].[name] = N'Description');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [ProductDetails] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [ProductDetails] ALTER COLUMN [Description] nvarchar(max) NULL;

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ProductDetails]') AND [c].[name] = N'Area');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [ProductDetails] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [ProductDetails] ALTER COLUMN [Area] float NULL;

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ProductDetails]') AND [c].[name] = N'Address');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [ProductDetails] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [ProductDetails] ALTER COLUMN [Address] nvarchar(max) NULL;

CREATE UNIQUE INDEX [IX_Products_DetailId] ON [Products] ([DetailId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241210032205_2024121010', N'9.0.0');

ALTER TABLE [ProductDetails] DROP CONSTRAINT [FK_ProductDetails_Products_ProductId];

DROP INDEX [IX_Products_DetailId] ON [Products];

DROP INDEX [IX_ProductDetails_ProductId] ON [ProductDetails];

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ProductDetails]') AND [c].[name] = N'ProductId');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [ProductDetails] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [ProductDetails] DROP COLUMN [ProductId];

CREATE INDEX [IX_Products_DetailId] ON [Products] ([DetailId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241210034626_2024121011', N'9.0.0');

ALTER TABLE [MinIOs] DROP CONSTRAINT [FK_MinIOs_NewsArticles_NewsArticleId];

DROP INDEX [IX_MinIOs_NewsArticleId] ON [MinIOs];

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'Thumbnail');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Products] DROP COLUMN [Thumbnail];

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MinIOs]') AND [c].[name] = N'NewsArticleId');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [MinIOs] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [MinIOs] DROP COLUMN [NewsArticleId];

ALTER TABLE [Products] ADD [ThumbnailImageId] bigint NULL;

ALTER TABLE [NewsArticles] ADD [ThumbnailImageId] bigint NULL;

CREATE INDEX [IX_Products_ThumbnailImageId] ON [Products] ([ThumbnailImageId]);

CREATE INDEX [IX_NewsArticles_ThumbnailImageId] ON [NewsArticles] ([ThumbnailImageId]);

ALTER TABLE [NewsArticles] ADD CONSTRAINT [FK_NewsArticles_MinIOs_ThumbnailImageId] FOREIGN KEY ([ThumbnailImageId]) REFERENCES [MinIOs] ([Id]);

ALTER TABLE [Products] ADD CONSTRAINT [FK_Products_MinIOs_ThumbnailImageId] FOREIGN KEY ([ThumbnailImageId]) REFERENCES [MinIOs] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241211101440_2024121117', N'9.0.0');

COMMIT;
GO

