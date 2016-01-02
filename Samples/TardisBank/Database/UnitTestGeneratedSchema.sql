
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK76553502C8A05760]') AND parent_object_id = OBJECT_ID('Messages'))
alter table Messages  drop constraint FK76553502C8A05760


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK90F02A4EDF10CD7C]') AND parent_object_id = OBJECT_ID('PaymentSchedules'))
alter table PaymentSchedules  drop constraint FK90F02A4EDF10CD7C


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK2045D237DF10CD7C]') AND parent_object_id = OBJECT_ID('Transactions'))
alter table Transactions  drop constraint FK2045D237DF10CD7C


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK436E2330C8A05760]') AND parent_object_id = OBJECT_ID('Child'))
alter table Child  drop constraint FK436E2330C8A05760


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK436E2330DF10CD7C]') AND parent_object_id = OBJECT_ID('Child'))
alter table Child  drop constraint FK436E2330DF10CD7C


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK436E23304ED31B26]') AND parent_object_id = OBJECT_ID('Child'))
alter table Child  drop constraint FK436E23304ED31B26


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK8359A792C8A05760]') AND parent_object_id = OBJECT_ID('Parent'))
alter table Parent  drop constraint FK8359A792C8A05760


    if exists (select * from dbo.sysobjects where id = object_id(N'Accounts') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Accounts

    if exists (select * from dbo.sysobjects where id = object_id(N'Announcements') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Announcements

    if exists (select * from dbo.sysobjects where id = object_id(N'Messages') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Messages

    if exists (select * from dbo.sysobjects where id = object_id(N'PaymentSchedules') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table PaymentSchedules

    if exists (select * from dbo.sysobjects where id = object_id(N'Transactions') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Transactions

    if exists (select * from dbo.sysobjects where id = object_id(N'Users') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Users

    if exists (select * from dbo.sysobjects where id = object_id(N'Child') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Child

    if exists (select * from dbo.sysobjects where id = object_id(N'Parent') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Parent

    if exists (select * from dbo.sysobjects where id = object_id(N'hibernate_unique_key') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table hibernate_unique_key

    create table Accounts (
        AccountId INT not null,
       OldTransactionsBalance DECIMAL(19,5) null,
       primary key (AccountId)
    )

    create table Announcements (
        AnnouncementId INT not null,
       LastModifiedUtc DATETIME null,
       Date DATETIME null,
       Title NVARCHAR(255) null,
       Content NVARCHAR(255) null,
       primary key (AnnouncementId)
    )

    create table Messages (
        MessageId INT not null,
       Date DATETIME null,
       Text NVARCHAR(255) null,
       HasBeenRead BIT null,
       UserId INT null,
       primary key (MessageId)
    )

    create table PaymentSchedules (
        PaymentScheduleId INT not null,
       NextRun DATETIME null,
       Interval NVARCHAR(255) null,
       Amount DECIMAL(19,5) null,
       Description NVARCHAR(255) null,
       AccountId INT null,
       primary key (PaymentScheduleId)
    )

    create table Transactions (
        TransactionId INT not null,
       Description NVARCHAR(255) null,
       Amount DECIMAL(19,5) null,
       Date DATETIME null,
       AccountId INT null,
       primary key (TransactionId)
    )

    create table Users (
        UserId INT not null,
       Name NVARCHAR(255) null,
       UserName NVARCHAR(255) null,
       Password NVARCHAR(255) null,
       IsActive BIT null,
       primary key (UserId)
    )

    create table Child (
        UserId INT not null,
       ParentId INT null,
       AccountId INT null,
       primary key (UserId)
    )

    create table Parent (
        UserId INT not null,
       ActivationKey NVARCHAR(255) null,
       primary key (UserId)
    )

    alter table Messages 
        add constraint FK76553502C8A05760 
        foreign key (UserId) 
        references Users

    alter table PaymentSchedules 
        add constraint FK90F02A4EDF10CD7C 
        foreign key (AccountId) 
        references Accounts

    alter table Transactions 
        add constraint FK2045D237DF10CD7C 
        foreign key (AccountId) 
        references Accounts

    alter table Child 
        add constraint FK436E2330C8A05760 
        foreign key (UserId) 
        references Users

    alter table Child 
        add constraint FK436E2330DF10CD7C 
        foreign key (AccountId) 
        references Accounts

    alter table Child 
        add constraint FK436E23304ED31B26 
        foreign key (ParentId) 
        references Parent

    alter table Parent 
        add constraint FK8359A792C8A05760 
        foreign key (UserId) 
        references Users

    create table hibernate_unique_key (
         next_hi INT 
    )

    insert into hibernate_unique_key values ( 1 )
