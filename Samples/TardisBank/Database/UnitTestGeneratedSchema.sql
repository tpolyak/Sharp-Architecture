
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK90343ACC91A88C85]') AND parent_object_id = OBJECT_ID('Messages'))
alter table Messages  drop constraint FK90343ACC91A88C85


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKFE394DF867314A17]') AND parent_object_id = OBJECT_ID('PaymentSchedules'))
alter table PaymentSchedules  drop constraint FKFE394DF867314A17


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK33A9128B67314A17]') AND parent_object_id = OBJECT_ID('Transactions'))
alter table Transactions  drop constraint FK33A9128B67314A17


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK19AC068491A88C85]') AND parent_object_id = OBJECT_ID('Child'))
alter table Child  drop constraint FK19AC068491A88C85


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK19AC068467314A17]') AND parent_object_id = OBJECT_ID('Child'))
alter table Child  drop constraint FK19AC068467314A17


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK19AC0684CA5AC310]') AND parent_object_id = OBJECT_ID('Child'))
alter table Child  drop constraint FK19AC0684CA5AC310


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKBFADD8AC91A88C85]') AND parent_object_id = OBJECT_ID('Parent'))
alter table Parent  drop constraint FKBFADD8AC91A88C85


    if exists (select * from dbo.sysobjects where id = object_id(N'Accounts') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Accounts

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
        add constraint FK90343ACC91A88C85 
        foreign key (UserId) 
        references Users

    alter table PaymentSchedules 
        add constraint FKFE394DF867314A17 
        foreign key (AccountId) 
        references Accounts

    alter table Transactions 
        add constraint FK33A9128B67314A17 
        foreign key (AccountId) 
        references Accounts

    alter table Child 
        add constraint FK19AC068491A88C85 
        foreign key (UserId) 
        references Users

    alter table Child 
        add constraint FK19AC068467314A17 
        foreign key (AccountId) 
        references Accounts

    alter table Child 
        add constraint FK19AC0684CA5AC310 
        foreign key (ParentId) 
        references Parent

    alter table Parent 
        add constraint FKBFADD8AC91A88C85 
        foreign key (UserId) 
        references Users

    create table hibernate_unique_key (
         next_hi INT 
    )

    insert into hibernate_unique_key values ( 1 )
