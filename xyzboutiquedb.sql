
create DATABASE xyzboutiquedb2;
GO

USE xyzboutiquedb2;
GO

CREATE SCHEMA Security;
GO

CREATE SCHEMA Administration;
GO

CREATE TABLE [Security].[Roles]
(
  [IdRole] UNIQUEIDENTIFIER NOT NULL,
  [Name] VARCHAR (500) NOT NULL,
  [State] INT NOT NULL,
  [UserCreation] VARCHAR (500) NOT NULL,
  [DateCreation] DATETIME NOT NULL,
  [UserUpdate] VARCHAR (500) NULL,
  [DateUpdate] DATETIME NULL,
  [Deleted] BIT NOT NULL,
  CONSTRAINT [pkRole] PRIMARY KEY CLUSTERED ([IdRole] ASC)
);
GO

insert into [Security].[Roles]
  (IdRole,[Name], [State], UserCreation, DateCreation, UserUpdate, DateUpdate, Deleted)
values
  ('e44d807d-d89b-48f8-b708-319fea2cb245', 'Encargado', 1, 'raulcv', getdate(), null, null, 0),
  ('0b52e9b9-0eca-4197-806e-3a5f844d8fa0', 'Vendedor', 1, 'raulcv', getdate(), null, null, 0),
  ('7bdb77ce-9d44-435a-8b24-3ebd9009bc11', 'Delivery', 1, 'raulcv', getdate(), null, null, 0),
  ('048aa998-99cb-438f-b3e5-b27d23040bd7', 'Repartidor', 1, 'raulcv', getdate(), null, null, 0);
go

select*
from [Security].[Roles];

Create TABLE [Security].[Users]
(
  [IdUser] UNIQUEIDENTIFIER NOT NULL default newid(),
  [IdRole] UNIQUEIDENTIFIER NOT NULL,
  [Code] VARCHAR (20) NOT NULL,
  [Name] VARCHAR (200) NOT NULL,
  [Email] VARCHAR (100) NOT NULL,
  [Phone] VARCHAR (50) NULL,
  [State] INT NOT NULL,
  [UserCreation] VARCHAR (500) NOT NULL,
  [DateCreation] DATETIME NOT NULL,
  [UserUpdate] VARCHAR (500) NULL,
  [DateUpdate] DATETIME NULL,
  [Deleted] BIT NOT NULL,
  CONSTRAINT [pkIdUser] PRIMARY KEY CLUSTERED ([IdUser] ASC),
  CONSTRAINT [fkUserHasRole] FOREIGN KEY ([IdRole]) REFERENCES [Security].[Roles] ([IdRole]),
);
GO

-- insert into [Security].[User]
--   (IdUser, Code, Name, Email, Phone, [State], UserCreation, DateCreation, UserUpdate, DateUpdate, Deleted)
-- values(newid(), 'USER001', 'Raul C.V', 'iraulcv@gmail.com', '92531722', 1, 'raulcv', getdate(), null, null, 0),
--   (newid(), 'USER002', 'Bianch', 'bianchbb@gmail.com', '23778373', 1, 'raulcv', getdate(), null, null, 0)
-- go
select *
from [Security].[Users];


CREATE TABLE [Security].[UserHashes]
(
  [IdUserHash] UNIQUEIDENTIFIER NOT NULL,
  [Hash] VARCHAR (500) NOT NULL,
  [Salt] VARCHAR (500) NOT NULL,
  [IdUser] UNIQUEIDENTIFIER NOT NULL,
  [DateCreation] DATETIME NOT NULL,
  [Deleted] BIT NOT NULL,
  CONSTRAINT [pkUserHash] PRIMARY KEY CLUSTERED ([IdUserHash] ASC),
  CONSTRAINT [fkUserHashHasUser] FOREIGN KEY ([IdUser]) REFERENCES [Security].[Users] ([IdUser])
);
GO
select *
from [Security].[UserHashes];
go

CREATE TABLE [Security].[UserTokens]
(
  [IdUserToken] UNIQUEIDENTIFIER NOT NULL default newid(),
  [Token] VARCHAR (2000) NULL,
  [DateCreation] DATETIME NOT NULL,
  [DateExpiration] DATETIME NOT NULL,
  [IdUser] UNIQUEIDENTIFIER NOT NULL,
  [Deleted] BIT NOT NULL,
  CONSTRAINT [pkUserToken] PRIMARY KEY CLUSTERED ([IdUserToken] ASC),
  CONSTRAINT [fkUserTokenHasUser] FOREIGN KEY ([IdUser]) REFERENCES [Security].[Users] ([IdUser])
);
GO



CREATE TABLE [Administration].[Customers]
(
  [IdCustomer] UNIQUEIDENTIFIER NOT NULL,
  [Name] VARCHAR (500) NOT NULL,
  [UserCreation] VARCHAR (500) NOT NULL,
  [DateCreation] DATETIME NOT NULL,
  [UserUpdate] VARCHAR (500) NULL,
  [DateUpdate] DATETIME NULL,
  [Deleted] BIT NOT NULL,
  CONSTRAINT [pkCustomer] PRIMARY KEY CLUSTERED ([IdCustomer] ASC)
);
GO

insert into [Administration].[Customers]
  (IdCustomer, [Name], UserCreation, DateCreation, UserUpdate, DateUpdate, Deleted)
values
  ('6e810875-7ec3-4774-aa9f-9677288ab33b'/*newid()*/, 'Ydel Lenon', 'raulcv', getdate(), null, null, 0);
go

select *
from Administration.Customers;
go

CREATE TABLE [Administration].[Employees]
(
  [IdEmployee] UNIQUEIDENTIFIER NOT NULL,
  [Name] VARCHAR (500) NOT NULL,
  [UserCreation] VARCHAR (500) NOT NULL,
  [DateCreation] DATETIME NOT NULL,
  [UserUpdate] VARCHAR (500) NULL,
  [DateUpdate] DATETIME NULL,
  [Deleted] BIT NOT NULL,
  CONSTRAINT [pkEmployee] PRIMARY KEY CLUSTERED ([IdEmployee] ASC)
);
GO

insert into [Administration].[Employees]
  (IdEmployee, [Name], UserCreation, DateCreation, UserUpdate, DateUpdate, Deleted)
values
  ('ccc1628e-2f33-48bc-8bfe-3231a8124640'/*newid()*/, 'Pedrito el que hace otra actividad', 'raulcv', getdate(), null, null, 0),
  ('053f73f6-a6e5-46c3-b19a-9afb14879bdd'/*newid()*/, 'Juancito El que Hace Delivery', 'raulcv', getdate(), null, null, 0);
go

select *
from Administration.Employees;
go

-- Create the Positions table in Security schema
CREATE TABLE Security.Positions
(
  IdPosition UNIQUEIDENTIFIER DEFAULT NEWID(),
  [Name] NVARCHAR(100),
  PRIMARY KEY (IdPosition)
);
GO

-- Insert Positions data
INSERT INTO Security.Positions
  (IdPosition,[Name])
VALUES
  ('2f862e93-f099-4125-9c5c-1a9e629c00a4'/*newid()*/, 'Encargado'),
  ('521644d7-fd08-4cca-8847-24a8527f0c2b'/*newid()*/, 'Vendedor'),
  ('ada81f08-240b-4cd7-95f3-5531b80fd4e8'/*newid()*/, 'Delivery'),
  ('f693ad17-f15c-4f63-ac3b-f4c523ae60a5'/*newid()*/, 'Repartidor');
GO

select *
from [Security].[Positions];
go

create TABLE [Administration].[ProductTypes]
(
  [IdProductType] UNIQUEIDENTIFIER DEFAULT NEWID(),
  [Name] VARCHAR (500) NOT NULL,
  [UserCreation] VARCHAR (500) NOT NULL,
  [DateCreation] DATETIME NOT NULL,
  [UserUpdate] VARCHAR (500) NULL,
  [DateUpdate] DATETIME NULL,
  [Deleted] BIT NOT NULL,
  CONSTRAINT [pkIdProductType] PRIMARY KEY CLUSTERED ([IdProductType] ASC)
);
GO

INSERT INTO Administration.ProductTypes
  (IdProductType,[Name],UserCreation, DateCreation, UserUpdate, DateUpdate, Deleted)
VALUES
  ('f93beae9-46d7-446c-9567-4592efe72f6c', 'Liquid', 'raulcv', getdate(), null, null, 0),
  ('6e299beb-1707-434e-b05c-7446e17d45aa', 'Solid', 'raulcv', getdate(), null, null, 0),
  ('0dc860d9-d64a-4534-9a12-8c472bf12083', 'Gas', 'raulcv', getdate(), null, null, 0),
  ('30492ef6-faa2-4a00-a919-d822d234351f', 'Tablet', 'raulcv', getdate(), null, null, 0);
GO

select *
from Administration.ProductTypes;
go

CREATE TABLE [Administration].[Measurements]
(
  [IdMeasurement] UNIQUEIDENTIFIER default newid(),
  [Name] VARCHAR (500) NOT NULL,
  [UserCreation] VARCHAR (500) NOT NULL,
  [DateCreation] DATETIME NOT NULL,
  [UserUpdate] VARCHAR (500) NULL,
  [DateUpdate] DATETIME NULL,
  [Deleted] BIT NOT NULL,
  CONSTRAINT [pkIdMeasurement] PRIMARY KEY CLUSTERED ([IdMeasurement] ASC)
);
GO

INSERT INTO Administration.Measurements
  (IdMeasurement,[Name],UserCreation, DateCreation, UserUpdate, DateUpdate, Deleted)
VALUES
  ('16d87972-0102-42f3-8ccd-af685826c606', 'Liters', 'raulcv', getdate(), null, null, 0),
  ('2eeb7ead-8c1b-4b16-8154-80deb6ba9a45', 'Kilograms', 'raulcv', getdate(), null, null, 0),
  ('9b6211e2-6cb1-4d36-b54d-b5cbbca840c6', 'CM', 'raulcv', getdate(), null, null, 0),
  ('9784a764-3570-48cc-b507-7ed501e065b7', 'Box', 'raulcv', getdate(), null, null, 0);
GO
select *
from Administration.Measurements;
go

CREATE TABLE [Administration].[Products]
(
  IdProduct UNIQUEIDENTIFIER DEFAULT NEWID(),
  IdProductType UNIQUEIDENTIFIER,
  IdMeasurement UNIQUEIDENTIFIER,
  Sku varchar(100) not null,
  [Name] NVARCHAR(100) not null,
  Labels NVARCHAR(MAX) not null,
  Price DECIMAL(18, 2) not null,
  [UserCreation] VARCHAR (500) NOT NULL,
  [DateCreation] DATETIME NOT NULL,
  [UserUpdate] VARCHAR (500) NULL,
  [DateUpdate] DATETIME NULL,
  [Deleted] BIT NOT NULL,
  PRIMARY KEY (IdProduct),
  FOREIGN KEY (IdProductType) REFERENCES Administration.ProductTypes(IdProductType),
  FOREIGN KEY (IdMeasurement) REFERENCES Administration.Measurements(IdMeasurement)
);
GO

INSERT INTO Administration.Products
  (IdProduct,[Sku], [Name],Labels, Price, IdProductType, IdMeasurement, UserCreation, DateCreation, UserUpdate, DateUpdate, Deleted)
VALUES
  ('5c3deeb3-e420-4387-987b-2189ef23584f', 'XYZ0001PRO', 'Milk', 'Milk,Natural,FreeOil,Low', 12.25, 'f93beae9-46d7-446c-9567-4592efe72f6c', '16d87972-0102-42f3-8ccd-af685826c606', 'raulcv', getdate(), null, null, 0),
  ('1327c2f7-eb1d-4e75-aa97-a6b69c36fc03', 'XYZ0002PRO', 'fragrance', 'Smell,Clean,Cool,Pleasant', 234.25, '30492ef6-faa2-4a00-a919-d822d234351f', '9784a764-3570-48cc-b507-7ed501e065b7', 'raulcv', getdate(), null, null, 0)
go

select *
from Administration.Products;
go

CREATE TABLE Administration.OrderStates
(
  IdOrderState UNIQUEIDENTIFIER DEFAULT NEWID(),
  [Name] NVARCHAR(50),
  PRIMARY KEY (IdOrderState)
);
GO

INSERT INTO Administration.OrderStates
  ([IdOrderState],[Name])
VALUES
  ('494232de-bf20-4840-ab47-f2b167545b1a', 'Por atender'),
  ('21ab948a-5b3a-45c7-b4ba-e2e835c0d547', 'En Proceso'),
  ('a0628340-1693-4b10-a14f-5a5a96b9e07f', 'En Delivery'),
  ('50a6f2ff-745c-4588-b50c-00d8096aa24b', 'Recibido');
GO

select *
from Administration.OrderStates;
go

CREATE TABLE Administration.Orders
(
  IdOrder UNIQUEIDENTIFIER DEFAULT NEWID(),
  IdCustomer UNIQUEIDENTIFIER not null,
  IdEmployee UNIQUEIDENTIFIER not null,
  [IdOrderState] UNIQUEIDENTIFIER not null,
  OrderNumber varchar(100) not null,
  OrderDate DATETIME null,
  ReceptionDate DATETIME null,
  ShippingDate DATETIME null,
  DeliveryDate DATETIME null,

  [UserCreation] VARCHAR (500) NOT NULL,
  [DateCreation] DATETIME NOT NULL,
  [UserUpdate] VARCHAR (500) NULL,
  [DateUpdate] DATETIME NULL,
  [Deleted] BIT NOT NULL,
  PRIMARY KEY (IdOrder),
  FOREIGN KEY (IdCustomer) REFERENCES Administration.Customers(IdCustomer),
  FOREIGN KEY (IdEmployee) REFERENCES Administration.Employees(IdEmployee),
  FOREIGN KEY (IdOrderState) REFERENCES Administration.OrderStates(IdOrderState),
);
GO
insert into [Administration].[Orders]
  (IdOrder, IdCustomer, IdEmployee, IdOrderState, OrderNumber, OrderDate, ReceptionDate, ShippingDate, DeliveryDate, UserCreation, DateCreation, UserUpdate, DateUpdate, Deleted)
values
  ('03463df9-7533-4446-bd73-076a5ab8a55c', '6e810875-7ec3-4774-aa9f-9677288ab33b', '053f73f6-a6e5-46c3-b19a-9afb14879bdd', '494232de-bf20-4840-ab47-f2b167545b1a', 'ORDER0001', getdate(), null, null, null, 'raulcv', getdate(), null, null, 0),
  ('bd6ef7a2-41c5-4d93-9690-4fe5dda74a44', '6e810875-7ec3-4774-aa9f-9677288ab33b', 'ccc1628e-2f33-48bc-8bfe-3231a8124640', '21ab948a-5b3a-45c7-b4ba-e2e835c0d547', 'ORDER0002', getdate(), getdate(), null, null, 'raulcv', getdate(), null, null, 0)
go

select *
from Administration.Orders;
go


CREATE TABLE Administration.OrderProducts
(
  [IdOrderProducts] UNIQUEIDENTIFIER DEFAULT NEWID(),
  [IdProduct] UNIQUEIDENTIFIER NOT NULL,
  [IdOrder] UNIQUEIDENTIFIER NOT NULL,
  [UserCreation] VARCHAR (500) NOT NULL,
  [DateCreation] DATETIME NOT NULL,
  [UserUpdate] VARCHAR (500) NULL,
  [DateUpdate] DATETIME NULL,
  [Deleted] BIT NOT NULL,
  PRIMARY KEY (IdOrderProducts),
  FOREIGN KEY (IdProduct) REFERENCES Administration.Products(IdProduct),
  FOREIGN KEY (IdOrder) REFERENCES Administration.Orders(IdOrder),
);
GO
select *
from [Administration].[OrderProducts];
go

/*****
Srore procedures and functions
****/


IF OBJECT_ID('uspUserSearchByUserName', 'P') IS NOT NULL
DROP PROC uspUserSearchByUserName  
GO

create proc [dbo].[uspUserSearchByUserName]     
@usuario varchar(200)      
as
select        
convert(varchar(36), u.IdUser) as id,          
u.Code as code,          
u.Name  as [user],       
u.email as email,
r.Name as role,
case when r.State = 1 then 'Active' when r.State = 0 then 'Inactive' else 'No defined' end as [state],

u.DateCreation,

isnull(dbo.ufnIsPasswordExpired(convert(varchar(36), u.IdUser),1),0) as isExpiredPassword

from [Security].[Users] u   
inner join Security.Roles r on r.IdRole=u.IdRole 
Where u.Deleted=0  and r.Deleted=0 and (u.Code= @usuario or u.email=@usuario)
go

--select * from [dbo].[ufnIsPasswordExpired]('RCV0001', 2)
-- exec uspUserSearchByUserName 'RCV0001'--'raulcv@gmail.com'



create proc [dbo].[uspUserHashSearchByIdUser]
  @id varchar(36)
as
begin
  select top(1)
    convert(varchar(36),usu.IdUser) as id,
    has.[Hash] as [hash],
    has.Salt as salt,
    has.[DateCreation] as fechaCreacion
  from [Security].[UserHashes] has
    inner join [Security].[Users] usu
    on has.IdUser=usu.IdUser
  where usu.Deleted=0 and has.Deleted=0
    and convert(varchar(36),usu.IdUser)=@id
  order by has.DateCreation desc;

end

/*
select * from Security.Users;
[dbo].[uspUserHashSearchByIdUser] '063cfc58-52a0-459c-afcf-08dbec4f8a4f'
*/
go


create function [dbo].[ufnIsPasswordExpired]             
(            
@id varchar(2000),  
@tipo int -- 1:id , 2:code , 3:token  
)            
returns bit   
as              
begin
  declare @esContraseniaExpirada bit=0;
  declare @fechaContraseania datetime
  declare @idUsuarioConsultar varchar(36)=''
  declare @fechaHoy datetime
  set @fechaHoy = getdate();


  if(@tipo=1)  
 begin
    set @idUsuarioConsultar = @id
  end  
 else if(@tipo=2)  
 begin
    set @idUsuarioConsultar = ISNULL((select convert(varchar(36), IdUser)
    from Security.Users
    where Code = @id and Deleted=0 ),0)
  end  
 else if(@tipo=3)  
  begin

    declare @idUsuario varchar(36)='';
    set @idUsuario= ( select convert(varchar(36),IdUser)
    from [Security].[UserTokens]
    where Deleted =0 and Token = @id and @fechaHoy between DateCreation and DateExpiration );

    set @idUsuarioConsultar =@idUsuario
    end

  set @fechaContraseania = (select DateCreation
  from Security.UserHashes
  where convert(varchar(36),IdUser)=@idUsuarioConsultar and Deleted=0)

  declare @cantidadDias int=0;
  set @cantidadDias= (SELECT DATEDIFF(DAY, @fechaContraseania, @fechaHoy))

  if(@cantidadDias >= 90 )  
  begin
    set @esContraseniaExpirada=1;
  end

  --set @esContraseniaExpirada=0; 
  return @esContraseniaExpirada
end 
go


IF OBJECT_ID('uspRoleSearch', 'P') IS NOT NULL
DROP PROC uspRoleSearch  
GO

--[uspRoleSearch] '',1,'',1,0
create proc [dbo].[uspRoleSearch]
  @text varchar(100),
  @state int,
  @orderby varchar(50),
  @quantity int,
  @page int,
  @total int output
as

if(@quantity =0)  set @quantity = 10;

IF OBJECT_ID('#rolequery') IS NOT NULL         
  begin
  DROP TABLE #rolequery
end

if(@orderby<> null or len(@orderby)=0 )
begin
  set @orderby='id'
end

SET NOCOUNT OFF;

create table #rolequery
(
  id varchar(36),
  name varchar(200) ,
  dateupdate datetime,
  idstate int,
  state varchar(50)
)


DECLARE @query NVARCHAR(MAX);
SET @query = ''
SET @query = @query + ' insert into #rolequery( id, name, dateupdate,idstate, state )'
SET @query = @query + ' select convert(varchar(36), r.IdRole) as id, r.name,'
SET @query = @query + ' r.DateUpdate , r.State, case when r.State = 1 then ''Active'' when r.State = 2 then ''Inactive'' else ''Other State'' end'
SET @query = @query + ' from [Security].[Roles] r '
SET @query = @query + ' where r.Name like ''%' + rtrim(ltrim(@text)) + '%'' and '
SET @query = @query + ' ('+ltrim(rtrim(str(@state)))+' = 0 or r.state= '+ltrim(rtrim(str(@state)))+') and '
SET @query = @query + ' r.Deleted=0 '
EXECUTE(@query)
set @total = @@ROWCOUNT
/*get total*/
print @query
SET NOCOUNT ON;

/*Pagination*/

SET @query = ''

if @page <> 0         
begin
  SET @query = 'WITH C AS '
  SET @query = @query + '( '
  SET @query = @query + 'SELECT ROW_NUMBER() OVER(ORDER BY '+rtrim(ltrim(@orderby))+' ) AS rownum, '
  SET @query = @query + '  id, name, dateupdate, idstate, state '
  SET @query = @query + 'FROM #rolequery '
  SET @query = @query + ') '
  SET @query = @query + 'SELECT id, name, isnull(convert(varchar, dateupdate, 103)+ '' ''+ convert(varchar, dateupdate, 108),'''') as dateupdate, idstate, state '
  SET @query = @query + 'FROM C '
  SET @query = @query + 'WHERE rownum BETWEEN ('+ltrim(rtrim(str(@page))) +'- 1) * '        
   +ltrim(rtrim(str(@quantity))) +' + 1 AND '+ltrim(rtrim(str(@page))) +' * '+ltrim(rtrim(str(@quantity)))

end        
else        
begin

  SET @query = @query + 'SELECT '
  SET @query = @query + ' id, name, isnull(convert(varchar, dateupdate, 103)+ '' ''+ convert(varchar, dateupdate, 108),'''') as dateupdate ,idstate, state'
  SET @query = @query + ' FROM #rolequery '
  SET @query = @query + ' order by '+rtrim(ltrim(@orderby))
end

EXECUTE(@query) 
--go
/*
declare @total int
exec [uspRoleSearch] @text='en',@state=1,@orderby='name',@quantity=2,@page=0, @total=@total output
print @total
*/
-- select * from security.role;


IF OBJECT_ID('uspUserInd', 'P') IS NOT NULL
DROP PROC uspUserInd  
GO

create proc [dbo].[uspUserInd]  
(  
@id varchar(36)  
)  
as

select   
convert(varchar(36),u.IdUser) as id,
convert(varchar(36),r.IdRole) as idRole, r.[Name] as [role],
u.Code as code,
u.[Name] as [name],
r.Name as rol,  
u.Email as email,  
u.State as idstate,
u.Phone as phone,
case when r.State = 1 then 'Active' when r.State = 0 then 'Inactive' else 'No defined' end as [state],
convert(varchar, u.DateUpdate, 103)+ ' '+ convert(varchar, u.DateUpdate, 108) as dateupdate
from [Security].[Users] u   
inner join Security.Roles r on r.IdRole=u.IdRole 

Where  convert(varchar(36),u.IdUser)=@id  and  u.Deleted=0  and r.Deleted=0;
go

--exec [dbo].[uspUserInd] ''