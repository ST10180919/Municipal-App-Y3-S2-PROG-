
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/17/2024 20:18:08
-- Generated from EDMX file: C:\Users\joshu\source\repos\Municipal App\MunicipalDatabaseModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [C:\USERS\JOSHU\SOURCE\REPOS\MUNICIPAL APP\MUNICIPALDATABASE.MDF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ATTACHMENT'
CREATE TABLE [dbo].[ATTACHMENT] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [NAME_OF_FILE] nvarchar(100)  NULL,
    [FILE_BINARY] varbinary(max)  NULL,
    [ISSUE_REPORT_ID] varchar(255) NOT NULL
);
GO

-- Creating table 'ISSUE_REPORT'
CREATE TABLE [dbo].[ISSUE_REPORT] (
    [IDENTIFIER] varchar(255)  NOT NULL,
    [LOCATION] nvarchar(max)  NULL,
    [DESCRIPTION] nvarchar(max)  NULL,
    [CATEGORY] nvarchar(100)  NULL,
    [SOLUTION] nvarchar(max)  NULL,
    [STATUS_STRING] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'ATTACHMENTs'
ALTER TABLE [dbo].[ATTACHMENT]
ADD CONSTRAINT [PK_ATTACHMENT]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [IDENTIFIER] in table 'ISSUE_REPORT'
ALTER TABLE [dbo].[ISSUE_REPORT]
ADD CONSTRAINT [PK_ISSUE_REPORT]
    PRIMARY KEY CLUSTERED ([IDENTIFIER] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ISSUE_REPORT_ID] in table 'ATTACHMENTs'
ALTER TABLE [dbo].[ATTACHMENT]
ADD CONSTRAINT [FK__ATTACHMEN__ISSUE__49C3F6B7]
    FOREIGN KEY ([ISSUE_REPORT_ID])
    REFERENCES [dbo].[ISSUE_REPORT]
        ([IDENTIFIER])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__ATTACHMEN__ISSUE__49C3F6B7'
CREATE INDEX [IX_FK__ATTACHMEN__ISSUE__49C3F6B7]
ON [dbo].[ATTACHMENT]
    ([ISSUE_REPORT_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------