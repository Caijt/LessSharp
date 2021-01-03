IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [attach] (
    [id] int NOT NULL IDENTITY,
    [create_time] datetime2 NOT NULL,
    [create_user_id] int NOT NULL,
    [name] nvarchar(max) NULL,
    [size] int NOT NULL,
    [ext] nvarchar(max) NULL,
    [path] nvarchar(max) NULL,
    [entity_name] nvarchar(max) NULL,
    [entity_guid] uniqueidentifier NOT NULL,
    [type] nvarchar(max) NULL,
    [is_public] bit NOT NULL,
    [delete_user_id] int NULL,
    [delete_time] datetime2 NULL,
    CONSTRAINT [PK_attach] PRIMARY KEY ([id])
);

GO

CREATE TABLE [sys_api] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(450) NULL,
    [path] nvarchar(450) NULL,
    [is_common] bit NOT NULL,
    [permission_type] int NOT NULL,
    CONSTRAINT [PK_sys_api] PRIMARY KEY ([id])
);

GO

CREATE TABLE [sys_config] (
    [key] nvarchar(450) NOT NULL,
    [value] nvarchar(max) NULL,
    [type] nvarchar(max) NOT NULL,
    [name] nvarchar(max) NULL,
    CONSTRAINT [PK_sys_config] PRIMARY KEY ([key])
);

GO

CREATE TABLE [sys_menu] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(max) NULL,
    [parent_id] int NULL,
    [path] nvarchar(max) NULL,
    [icon] nvarchar(max) NULL,
    [parent_ids] nvarchar(max) NULL,
    [order] int NOT NULL,
    [is_page] bit NOT NULL,
    [can_multiple_open] bit NOT NULL,
    [has_read] bit NOT NULL,
    [has_write] bit NOT NULL,
    [has_review] bit NOT NULL,
    CONSTRAINT [PK_sys_menu] PRIMARY KEY ([id])
);

GO

CREATE TABLE [sys_role] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(450) NULL,
    [remarks] nvarchar(max) NULL,
    [create_time] datetime2 NOT NULL,
    [update_time] datetime2 NOT NULL,
    CONSTRAINT [PK_sys_role] PRIMARY KEY ([id])
);

GO

CREATE TABLE [sys_menu_api] (
    [api_id] int NOT NULL,
    [menu_id] int NOT NULL,
    CONSTRAINT [PK_sys_menu_api] PRIMARY KEY ([api_id], [menu_id]),
    CONSTRAINT [FK_sys_menu_api_sys_api_api_id] FOREIGN KEY ([api_id]) REFERENCES [sys_api] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_sys_menu_api_sys_menu_menu_id] FOREIGN KEY ([menu_id]) REFERENCES [sys_menu] ([id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [sys_role_menu] (
    [role_id] int NOT NULL,
    [menu_id] int NOT NULL,
    [can_read] bit NOT NULL,
    [can_write] bit NOT NULL,
    [can_review] bit NOT NULL,
    CONSTRAINT [PK_sys_role_menu] PRIMARY KEY ([role_id], [menu_id]),
    CONSTRAINT [FK_sys_role_menu_sys_menu_menu_id] FOREIGN KEY ([menu_id]) REFERENCES [sys_menu] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_sys_role_menu_sys_role_role_id] FOREIGN KEY ([role_id]) REFERENCES [sys_role] ([id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [sys_user] (
    [id] int NOT NULL IDENTITY,
    [login_name] nvarchar(450) NULL,
    [login_password] nvarchar(max) NULL,
    [role_id] int NOT NULL,
    [is_disabled] bit NOT NULL,
    [create_time] datetime2 NOT NULL,
    [update_time] datetime2 NOT NULL,
    CONSTRAINT [PK_sys_user] PRIMARY KEY ([id]),
    CONSTRAINT [FK_sys_user_sys_role_role_id] FOREIGN KEY ([role_id]) REFERENCES [sys_role] ([id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [sys_user_login_log] (
    [id] int NOT NULL IDENTITY,
    [ip_address] nvarchar(max) NULL,
    [create_time] datetime2 NOT NULL,
    [create_user_id] int NOT NULL,
    CONSTRAINT [PK_sys_user_login_log] PRIMARY KEY ([id]),
    CONSTRAINT [FK_sys_user_login_log_sys_user_create_user_id] FOREIGN KEY ([create_user_id]) REFERENCES [sys_user] ([id]) ON DELETE NO ACTION
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'key', N'name', N'type', N'value') AND [object_id] = OBJECT_ID(N'[sys_config]'))
    SET IDENTITY_INSERT [sys_config] ON;
INSERT INTO [sys_config] ([key], [name], [type], [value])
VALUES (N'SYSTEM_TITLE', N'系统标题', N'STRING', N'LessAdmin快速开发框架'),
(N'MENU_BAR_TITLE', N'菜单栏标题', N'STRING', N'LessAdmin'),
(N'VERSION', N'版本号', N'STRING', N'20200414001'),
(N'IS_REPAIR', N'网站维护', N'BOOL', N'OFF'),
(N'LAYOUT', N'后台布局', N'STRING', N'leftRight'),
(N'LIST_DEFAULT_PAGE_SIZE', N'列表默认页容量', N'NUMBER', N'10'),
(N'MENU_DEFAULT_ICON', N'菜单默认图标', N'STRING', N'el-icon-menu'),
(N'SHOW_MENU_ICON', N'是否显示菜单图标', N'BOOL', N'OFF'),
(N'LOGIN_VCODE', N'登录需要验证码', N'BOOL', N'OFF'),
(N'PAGE_TABS', N'使用多页面标签', N'BOOL', N'ON');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'key', N'name', N'type', N'value') AND [object_id] = OBJECT_ID(N'[sys_config]'))
    SET IDENTITY_INSERT [sys_config] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'can_multiple_open', N'has_read', N'has_review', N'has_write', N'icon', N'is_page', N'name', N'order', N'parent_id', N'parent_ids', N'path') AND [object_id] = OBJECT_ID(N'[sys_menu]'))
    SET IDENTITY_INSERT [sys_menu] ON;
INSERT INTO [sys_menu] ([id], [can_multiple_open], [has_read], [has_review], [has_write], [icon], [is_page], [name], [order], [parent_id], [parent_ids], [path])
VALUES (1, CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, CAST(0 AS bit), N'系统管理', 99, NULL, NULL, N'sys'),
(2, CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, CAST(0 AS bit), N'用户管理', 1, 1, N'1', N'user'),
(3, CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, CAST(0 AS bit), N'角色管理', 2, 1, N'1', N'role'),
(4, CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, CAST(0 AS bit), N'菜单管理', 3, 1, N'1', N'menu'),
(5, CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, CAST(0 AS bit), N'接口管理', 4, 1, N'1', N'api');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'can_multiple_open', N'has_read', N'has_review', N'has_write', N'icon', N'is_page', N'name', N'order', N'parent_id', N'parent_ids', N'path') AND [object_id] = OBJECT_ID(N'[sys_menu]'))
    SET IDENTITY_INSERT [sys_menu] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'create_time', N'name', N'remarks', N'update_time') AND [object_id] = OBJECT_ID(N'[sys_role]'))
    SET IDENTITY_INSERT [sys_role] ON;
INSERT INTO [sys_role] ([id], [create_time], [name], [remarks], [update_time])
VALUES (-1, '0001-01-01T00:00:00.0000000', N'超级角色', NULL, '0001-01-01T00:00:00.0000000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'create_time', N'name', N'remarks', N'update_time') AND [object_id] = OBJECT_ID(N'[sys_role]'))
    SET IDENTITY_INSERT [sys_role] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'create_time', N'is_disabled', N'login_name', N'login_password', N'role_id', N'update_time') AND [object_id] = OBJECT_ID(N'[sys_user]'))
    SET IDENTITY_INSERT [sys_user] ON;
INSERT INTO [sys_user] ([id], [create_time], [is_disabled], [login_name], [login_password], [role_id], [update_time])
VALUES (-1, '0001-01-01T00:00:00.0000000', CAST(0 AS bit), N'superadmin', N'admin', -1, '0001-01-01T00:00:00.0000000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'create_time', N'is_disabled', N'login_name', N'login_password', N'role_id', N'update_time') AND [object_id] = OBJECT_ID(N'[sys_user]'))
    SET IDENTITY_INSERT [sys_user] OFF;

GO

CREATE UNIQUE INDEX [IX_sys_api_name] ON [sys_api] ([name]) WHERE [name] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_sys_api_path] ON [sys_api] ([path]) WHERE [path] IS NOT NULL;

GO

CREATE INDEX [IX_sys_menu_api_menu_id] ON [sys_menu_api] ([menu_id]);

GO

CREATE UNIQUE INDEX [IX_sys_role_name] ON [sys_role] ([name]) WHERE [name] IS NOT NULL;

GO

CREATE INDEX [IX_sys_role_menu_menu_id] ON [sys_role_menu] ([menu_id]);

GO

CREATE UNIQUE INDEX [IX_sys_user_login_name] ON [sys_user] ([login_name]) WHERE [login_name] IS NOT NULL;

GO

CREATE INDEX [IX_sys_user_role_id] ON [sys_user] ([role_id]);

GO

CREATE INDEX [IX_sys_user_login_log_create_user_id] ON [sys_user_login_log] ([create_user_id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200713151816_faf07d62-7868-4e77-b361-38d4668bbaa6', N'3.1.5');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200725020916_5212cf25-dd3e-430e-b2f3-6075a29591d8', N'3.1.5');

GO

CREATE TABLE [sys_token] (
    [access_token] nvarchar(450) NOT NULL,
    [access_expire] datetime2 NOT NULL,
    [user_id] int NOT NULL,
    [ip] nvarchar(max) NULL,
    [refresh_token] nvarchar(max) NULL,
    [refresh_expire] datetime2 NOT NULL,
    [create_time] datetime2 NOT NULL,
    CONSTRAINT [PK_sys_token] PRIMARY KEY ([access_token]),
    CONSTRAINT [FK_sys_token_sys_user_user_id] FOREIGN KEY ([user_id]) REFERENCES [sys_user] ([id]) ON DELETE NO ACTION
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'can_multiple_open', N'has_read', N'has_review', N'has_write', N'icon', N'is_page', N'name', N'order', N'parent_id', N'parent_ids', N'path') AND [object_id] = OBJECT_ID(N'[sys_menu]'))
    SET IDENTITY_INSERT [sys_menu] ON;
INSERT INTO [sys_menu] ([id], [can_multiple_open], [has_read], [has_review], [has_write], [icon], [is_page], [name], [order], [parent_id], [parent_ids], [path])
VALUES (6, CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, CAST(0 AS bit), N'配置管理', 5, 1, N'1', N'config');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'can_multiple_open', N'has_read', N'has_review', N'has_write', N'icon', N'is_page', N'name', N'order', N'parent_id', N'parent_ids', N'path') AND [object_id] = OBJECT_ID(N'[sys_menu]'))
    SET IDENTITY_INSERT [sys_menu] OFF;

GO

CREATE INDEX [IX_sys_token_user_id] ON [sys_token] ([user_id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200806054111_8edf4b53-a0cc-4b7e-a9c4-682d6e0c69e9', N'3.1.5');

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'can_multiple_open', N'has_read', N'has_review', N'has_write', N'icon', N'is_page', N'name', N'order', N'parent_id', N'parent_ids', N'path') AND [object_id] = OBJECT_ID(N'[sys_menu]'))
    SET IDENTITY_INSERT [sys_menu] ON;
INSERT INTO [sys_menu] ([id], [can_multiple_open], [has_read], [has_review], [has_write], [icon], [is_page], [name], [order], [parent_id], [parent_ids], [path])
VALUES (7, CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), CAST(0 AS bit), NULL, CAST(0 AS bit), N'Token管理', 6, 1, N'1', N'token');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'can_multiple_open', N'has_read', N'has_review', N'has_write', N'icon', N'is_page', N'name', N'order', N'parent_id', N'parent_ids', N'path') AND [object_id] = OBJECT_ID(N'[sys_menu]'))
    SET IDENTITY_INSERT [sys_menu] OFF;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200806060253_467d9e47-3475-40a1-b999-5fa88e3a1126', N'3.1.5');

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'is_common', N'name', N'path', N'permission_type') AND [object_id] = OBJECT_ID(N'[sys_api]'))
    SET IDENTITY_INSERT [sys_api] ON;
INSERT INTO [sys_api] ([id], [is_common], [name], [path], [permission_type])
VALUES (1, CAST(0 AS bit), N'获取接口分页列表', N'/Sys/Api/GetPageList', 0),
(2, CAST(0 AS bit), N'删除接口', N'/Sys/Api/DeleteById', 1),
(3, CAST(0 AS bit), N'保存接口', N'/Sys/Api/Save', 1),
(4, CAST(1 AS bit), N'获取接口公共分页列表', N'/Sys/Api/GetCommonPageList', 0),
(5, CAST(1 AS bit), N'获取角色公共选项列表', N'/Sys/Role/GetCommonOptionList', 0),
(6, CAST(0 AS bit), N'获取角色分页列表', N'/Sys/Role/GetPageList', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'is_common', N'name', N'path', N'permission_type') AND [object_id] = OBJECT_ID(N'[sys_api]'))
    SET IDENTITY_INSERT [sys_api] OFF;

GO

UPDATE [sys_menu] SET [has_read] = CAST(1 AS bit), [has_write] = CAST(1 AS bit)
WHERE [id] = 2;
SELECT @@ROWCOUNT;


GO

UPDATE [sys_menu] SET [has_read] = CAST(1 AS bit), [has_write] = CAST(1 AS bit)
WHERE [id] = 3;
SELECT @@ROWCOUNT;


GO

UPDATE [sys_menu] SET [has_read] = CAST(1 AS bit), [has_write] = CAST(1 AS bit)
WHERE [id] = 4;
SELECT @@ROWCOUNT;


GO

UPDATE [sys_menu] SET [has_read] = CAST(1 AS bit), [has_write] = CAST(1 AS bit)
WHERE [id] = 5;
SELECT @@ROWCOUNT;


GO

UPDATE [sys_menu] SET [has_read] = CAST(1 AS bit), [has_write] = CAST(1 AS bit)
WHERE [id] = 6;
SELECT @@ROWCOUNT;


GO

UPDATE [sys_menu] SET [has_read] = CAST(1 AS bit), [has_write] = CAST(1 AS bit)
WHERE [id] = 7;
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200806151457_f7f668c8-7137-4b4a-ad57-80b76ac4899e', N'3.1.5');

GO

ALTER TABLE [sys_token] ADD [is_invalid] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200808013522_af7ba3d2-fd8c-487d-9c6a-9b9d34353499', N'3.1.5');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[sys_token]') AND [c].[name] = N'is_invalid');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [sys_token] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [sys_token] DROP COLUMN [is_invalid];

GO

ALTER TABLE [sys_token] ADD [is_disabled] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200808023201_b7a3043e-5dcd-498c-b203-d3475b799c5b', N'3.1.5');

GO

ALTER TABLE [sys_user_login_log] DROP CONSTRAINT [FK_sys_user_login_log_sys_user_create_user_id];

GO

DROP INDEX [IX_sys_user_login_log_create_user_id] ON [sys_user_login_log];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[sys_user_login_log]') AND [c].[name] = N'create_user_id');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [sys_user_login_log] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [sys_user_login_log] DROP COLUMN [create_user_id];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200809151934_ddcdceab-13a7-435f-a521-7a6b77cd59cd', N'3.1.5');

GO

ALTER TABLE [sys_user_login_log] ADD [user_id] int NOT NULL DEFAULT 0;

GO

CREATE INDEX [IX_sys_user_login_log_user_id] ON [sys_user_login_log] ([user_id]);

GO

ALTER TABLE [sys_user_login_log] ADD CONSTRAINT [FK_sys_user_login_log_sys_user_user_id] FOREIGN KEY ([user_id]) REFERENCES [sys_user] ([id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200809153324_e41f3df9-a3f4-4824-ac99-76737adf8943', N'3.1.5');

GO

