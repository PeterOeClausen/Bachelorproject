
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/16/2016 11:14:32
-- Generated from EDMX file: C:\Users\Archigo\Documents\GitHub\Bachelorproject\DCRGraph Case Study\DcrWebAPI\DBSchema.edmx
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


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DCREvents'
CREATE TABLE [dbo].[DCREvents] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EventId] nvarchar(max)  NOT NULL,
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

-- Creating table 'Includes'
CREATE TABLE [dbo].[Includes] (
    [FromEventId] int  NOT NULL,
    [ToEventId] int  NOT NULL
);
GO

-- Creating table 'Excludes'
CREATE TABLE [dbo].[Excludes] (
    [FromEventId] int  NOT NULL,
    [ToDCREventId] int  NOT NULL
);
GO

-- Creating table 'Conditions'
CREATE TABLE [dbo].[Conditions] (
    [ToEventId] int  NOT NULL,
    [FromEventId] int  NOT NULL
);
GO

-- Creating table 'Responses'
CREATE TABLE [dbo].[Responses] (
    [ToEventId] int  NOT NULL,
    [FromEventId] int  NOT NULL
);
GO

-- Creating table 'Milestones'
CREATE TABLE [dbo].[Milestones] (
    [ToEventId] int  NOT NULL,
    [FromEventId] int  NOT NULL
);
GO

-- Creating table 'Children'
CREATE TABLE [dbo].[Children] (
    [ToEventId] int  NOT NULL,
    [FromEventId] int  NOT NULL
);
GO

-- Creating table 'DCRGraphs'
CREATE TABLE [dbo].[DCRGraphs] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'GraphEvents'
CREATE TABLE [dbo].[GraphEvents] (
    [DCRGraphId] int  NOT NULL,
    [DCREventId] int  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderDate] datetime  NOT NULL,
    [Table] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [CustomerId] int  NOT NULL,
    [DCRGraph_Id] int  NOT NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstAndMiddleName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NOT NULL,
    [StreetAndNumber] nvarchar(max)  NULL,
    [Zipcode] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL
);
GO

-- Creating table 'Items'
CREATE TABLE [dbo].[Items] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Price] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CategoryId] int  NOT NULL
);
GO

-- Creating table 'OrderDetails'
CREATE TABLE [dbo].[OrderDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] int  NOT NULL,
    [ItemId] int  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EventUIElemements'
CREATE TABLE [dbo].[EventUIElemements] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IntegerSpecifyingUIElementId] int  NOT NULL,
    [DCREventId] int  NOT NULL
);
GO

-- Creating table 'IntegerSpecifyingUIElements'
CREATE TABLE [dbo].[IntegerSpecifyingUIElements] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Integer] nvarchar(max)  NOT NULL
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

-- Creating primary key on [FromEventId], [ToEventId] in table 'Includes'
ALTER TABLE [dbo].[Includes]
ADD CONSTRAINT [PK_Includes]
    PRIMARY KEY CLUSTERED ([FromEventId], [ToEventId] ASC);
GO

-- Creating primary key on [ToDCREventId], [FromEventId] in table 'Excludes'
ALTER TABLE [dbo].[Excludes]
ADD CONSTRAINT [PK_Excludes]
    PRIMARY KEY CLUSTERED ([ToDCREventId], [FromEventId] ASC);
GO

-- Creating primary key on [ToEventId], [FromEventId] in table 'Conditions'
ALTER TABLE [dbo].[Conditions]
ADD CONSTRAINT [PK_Conditions]
    PRIMARY KEY CLUSTERED ([ToEventId], [FromEventId] ASC);
GO

-- Creating primary key on [ToEventId], [FromEventId] in table 'Responses'
ALTER TABLE [dbo].[Responses]
ADD CONSTRAINT [PK_Responses]
    PRIMARY KEY CLUSTERED ([ToEventId], [FromEventId] ASC);
GO

-- Creating primary key on [FromEventId], [ToEventId] in table 'Milestones'
ALTER TABLE [dbo].[Milestones]
ADD CONSTRAINT [PK_Milestones]
    PRIMARY KEY CLUSTERED ([FromEventId], [ToEventId] ASC);
GO

-- Creating primary key on [ToEventId], [FromEventId] in table 'Children'
ALTER TABLE [dbo].[Children]
ADD CONSTRAINT [PK_Children]
    PRIMARY KEY CLUSTERED ([ToEventId], [FromEventId] ASC);
GO

-- Creating primary key on [Id] in table 'DCRGraphs'
ALTER TABLE [dbo].[DCRGraphs]
ADD CONSTRAINT [PK_DCRGraphs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [DCRGraphId], [DCREventId] in table 'GraphEvents'
ALTER TABLE [dbo].[GraphEvents]
ADD CONSTRAINT [PK_GraphEvents]
    PRIMARY KEY CLUSTERED ([DCRGraphId], [DCREventId] ASC);
GO

-- Creating primary key on [Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [PK_Items]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [PK_OrderDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EventUIElemements'
ALTER TABLE [dbo].[EventUIElemements]
ADD CONSTRAINT [PK_EventUIElemements]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'IntegerSpecifyingUIElements'
ALTER TABLE [dbo].[IntegerSpecifyingUIElements]
ADD CONSTRAINT [PK_IntegerSpecifyingUIElements]
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

-- Creating foreign key on [FromEventId] in table 'Includes'
ALTER TABLE [dbo].[Includes]
ADD CONSTRAINT [FK_DCREventEntity1]
    FOREIGN KEY ([FromEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ToEventId] in table 'Includes'
ALTER TABLE [dbo].[Includes]
ADD CONSTRAINT [FK_DCREventIncludes]
    FOREIGN KEY ([ToEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCREventIncludes'
CREATE INDEX [IX_FK_DCREventIncludes]
ON [dbo].[Includes]
    ([ToEventId]);
GO

-- Creating foreign key on [FromEventId] in table 'Excludes'
ALTER TABLE [dbo].[Excludes]
ADD CONSTRAINT [FK_DCREventExcludes]
    FOREIGN KEY ([FromEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCREventExcludes'
CREATE INDEX [IX_FK_DCREventExcludes]
ON [dbo].[Excludes]
    ([FromEventId]);
GO

-- Creating foreign key on [ToDCREventId] in table 'Excludes'
ALTER TABLE [dbo].[Excludes]
ADD CONSTRAINT [FK_DCREventExcludes1]
    FOREIGN KEY ([ToDCREventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ToEventId] in table 'Conditions'
ALTER TABLE [dbo].[Conditions]
ADD CONSTRAINT [FK_DCREventConditions]
    FOREIGN KEY ([ToEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [FromEventId] in table 'Conditions'
ALTER TABLE [dbo].[Conditions]
ADD CONSTRAINT [FK_DCREventConditions1]
    FOREIGN KEY ([FromEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCREventConditions1'
CREATE INDEX [IX_FK_DCREventConditions1]
ON [dbo].[Conditions]
    ([FromEventId]);
GO

-- Creating foreign key on [ToEventId] in table 'Responses'
ALTER TABLE [dbo].[Responses]
ADD CONSTRAINT [FK_DCREventResponses]
    FOREIGN KEY ([ToEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [FromEventId] in table 'Responses'
ALTER TABLE [dbo].[Responses]
ADD CONSTRAINT [FK_DCREventResponses1]
    FOREIGN KEY ([FromEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCREventResponses1'
CREATE INDEX [IX_FK_DCREventResponses1]
ON [dbo].[Responses]
    ([FromEventId]);
GO

-- Creating foreign key on [ToEventId] in table 'Milestones'
ALTER TABLE [dbo].[Milestones]
ADD CONSTRAINT [FK_DCREventMilestones]
    FOREIGN KEY ([ToEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCREventMilestones'
CREATE INDEX [IX_FK_DCREventMilestones]
ON [dbo].[Milestones]
    ([ToEventId]);
GO

-- Creating foreign key on [FromEventId] in table 'Milestones'
ALTER TABLE [dbo].[Milestones]
ADD CONSTRAINT [FK_DCREventMilestones1]
    FOREIGN KEY ([FromEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ToEventId] in table 'Children'
ALTER TABLE [dbo].[Children]
ADD CONSTRAINT [FK_DCREventChildren]
    FOREIGN KEY ([ToEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [FromEventId] in table 'Children'
ALTER TABLE [dbo].[Children]
ADD CONSTRAINT [FK_DCREventChildren1]
    FOREIGN KEY ([FromEventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCREventChildren1'
CREATE INDEX [IX_FK_DCREventChildren1]
ON [dbo].[Children]
    ([FromEventId]);
GO

-- Creating foreign key on [DCRGraphId] in table 'GraphEvents'
ALTER TABLE [dbo].[GraphEvents]
ADD CONSTRAINT [FK_DCRGraphGraphEvent]
    FOREIGN KEY ([DCRGraphId])
    REFERENCES [dbo].[DCRGraphs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [DCREventId] in table 'GraphEvents'
ALTER TABLE [dbo].[GraphEvents]
ADD CONSTRAINT [FK_DCREventGraphEvent]
    FOREIGN KEY ([DCREventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCREventGraphEvent'
CREATE INDEX [IX_FK_DCREventGraphEvent]
ON [dbo].[GraphEvents]
    ([DCREventId]);
GO

-- Creating foreign key on [CustomerId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_CustomerOrder]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerOrder'
CREATE INDEX [IX_FK_CustomerOrder]
ON [dbo].[Orders]
    ([CustomerId]);
GO

-- Creating foreign key on [DCRGraph_Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_OrderDCRGraph]
    FOREIGN KEY ([DCRGraph_Id])
    REFERENCES [dbo].[DCRGraphs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderDCRGraph'
CREATE INDEX [IX_FK_OrderDCRGraph]
ON [dbo].[Orders]
    ([DCRGraph_Id]);
GO

-- Creating foreign key on [OrderId] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [FK_OrderOrderDetail]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderOrderDetail'
CREATE INDEX [IX_FK_OrderOrderDetail]
ON [dbo].[OrderDetails]
    ([OrderId]);
GO

-- Creating foreign key on [ItemId] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [FK_ItemOrderDetail]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemOrderDetail'
CREATE INDEX [IX_FK_ItemOrderDetail]
ON [dbo].[OrderDetails]
    ([ItemId]);
GO

-- Creating foreign key on [CategoryId] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_CategoryItem]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryItem'
CREATE INDEX [IX_FK_CategoryItem]
ON [dbo].[Items]
    ([CategoryId]);
GO

-- Creating foreign key on [IntegerSpecifyingUIElementId] in table 'EventUIElemements'
ALTER TABLE [dbo].[EventUIElemements]
ADD CONSTRAINT [FK_IntegerSpecifyingUIElementEventUIElemement]
    FOREIGN KEY ([IntegerSpecifyingUIElementId])
    REFERENCES [dbo].[IntegerSpecifyingUIElements]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IntegerSpecifyingUIElementEventUIElemement'
CREATE INDEX [IX_FK_IntegerSpecifyingUIElementEventUIElemement]
ON [dbo].[EventUIElemements]
    ([IntegerSpecifyingUIElementId]);
GO

-- Creating foreign key on [DCREventId] in table 'EventUIElemements'
ALTER TABLE [dbo].[EventUIElemements]
ADD CONSTRAINT [FK_DCREventEventUIElemement]
    FOREIGN KEY ([DCREventId])
    REFERENCES [dbo].[DCREvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCREventEventUIElemement'
CREATE INDEX [IX_FK_DCREventEventUIElemement]
ON [dbo].[EventUIElemements]
    ([DCREventId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------