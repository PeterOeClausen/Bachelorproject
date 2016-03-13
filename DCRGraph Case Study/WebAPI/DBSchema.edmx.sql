
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/09/2016 15:37:42
-- Generated from EDMX file: C:\Users\Archigo\Documents\GitHub\Bachelorproject\DCRGraph Case Study\WebAPI\DBSchema.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WebAPISQLDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_EventRoleDCREvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventRoles] DROP CONSTRAINT [FK_EventRoleDCREvent];
GO
IF OBJECT_ID(N'[dbo].[FK_EventRoleRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventRoles] DROP CONSTRAINT [FK_EventRoleRole];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupEventGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventGroups] DROP CONSTRAINT [FK_GroupEventGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_DCREventEventGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventGroups] DROP CONSTRAINT [FK_DCREventEventGroup];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DCREvents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DCREvents];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[EventRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventRoles];
GO
IF OBJECT_ID(N'[dbo].[EventGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventGroups];
GO
IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DCREvents'
CREATE TABLE [dbo].[DCREvents] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EventId] int  NOT NULL,
    [Label] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [StatusMessageAfterExecution] nvarchar(max)  NULL,
    [Included] bit  NOT NULL,
    [Pending] bit  NOT NULL,
    [Executed] bit  NOT NULL,
    [Parent] bit  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EventRoles'
CREATE TABLE [dbo].[EventRoles] (
    [DCREventId] int  NOT NULL,
    [RoleId] int  NOT NULL
);
GO

-- Creating table 'EventGroups'
CREATE TABLE [dbo].[EventGroups] (
    [GroupId] int  NOT NULL,
    [DCREventId] int  NOT NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'DCREvents'
ALTER TABLE [dbo].[DCREvents]
ADD CONSTRAINT [PK_DCREvents]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [DCREventId], [RoleId] in table 'EventRoles'
ALTER TABLE [dbo].[EventRoles]
ADD CONSTRAINT [PK_EventRoles]
    PRIMARY KEY CLUSTERED ([DCREventId], [RoleId] ASC);
GO

-- Creating primary key on [GroupId], [DCREventId] in table 'EventGroups'
ALTER TABLE [dbo].[EventGroups]
ADD CONSTRAINT [PK_EventGroups]
    PRIMARY KEY CLUSTERED ([GroupId], [DCREventId] ASC);
GO

-- Creating primary key on [Id] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DCREventId] in table 'EventRoles'
ALTER TABLE [dbo].[EventRoles]
ADD CONSTRAINT [FK_EventRoleDCREvent]
    FOREIGN KEY ([DCREventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RoleId] in table 'EventRoles'
ALTER TABLE [dbo].[EventRoles]
ADD CONSTRAINT [FK_EventRoleRole]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventRoleRole'
CREATE INDEX [IX_FK_EventRoleRole]
ON [dbo].[EventRoles]
    ([RoleId]);
GO

-- Creating foreign key on [GroupId] in table 'EventGroups'
ALTER TABLE [dbo].[EventGroups]
ADD CONSTRAINT [FK_GroupEventGroup]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [DCREventId] in table 'EventGroups'
ALTER TABLE [dbo].[EventGroups]
ADD CONSTRAINT [FK_DCREventEventGroup]
    FOREIGN KEY ([DCREventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCREventEventGroup'
CREATE INDEX [IX_FK_DCREventEventGroup]
ON [dbo].[EventGroups]
    ([DCREventId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------