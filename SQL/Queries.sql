create database db_FoodOrderingApplication


-----------------Admin Table----------

create table tbl_Admin(
AdmId int primary key identity(1,1),
AdmEmail varchar(50),
AdmPassword varchar(20)
)

alter table tbl_Admin add AdminRole int references tbl_Role(RoleId)


select * from tbl_Admin



---------------- Category Table------------

Create table tbl_Category(
CatId int primary key identity(1,1),
CatName varchar(50) not null,
CatImage varchar(max) not null,
CatStatus char(1) not null
)

---------------Offers Table----------------

Create table tbl_Offers(
OfferId int primary key identity(1,1),
OfferCode varchar(50) not null,
OfferPercentage int not null,
OfferDescription varchar(max)
)

select * from tbl_Offers

----------------Restaurant Table---------

Create table tbl_Restaurant(
RestId int primary key identity(1,1),
RestName varchar(50) not null unique,
RestPhone varchar(20) unique not null,
RestEmail varchar(50) unique not null,
RestPincode varchar(20) not null,
RestImage varchar(max) not null,
RestCountry varchar(50) not null,
RestState varchar(50) not null,
RestDistrict varchar(50) not null,
RestPassword varchar(50) not null,

RestStatus char(1)
)

alter table tbl_Restaurant add Rest_fk_Offer int references tbl_Offers(OfferId) on update cascade on delete cascade
alter table tbl_Restaurant add RestArea varchar(50)
select * from tbl_Restaurant
-----------------Customer Table-----------

create table tbl_Customer(
CusId int primary key identity(1,1),
CusName varchar(50) not null,
CusEmail varchar(50) not null unique,
CusPassword varchar(20) not null,
CusImage varchar(max),
CusPincode varchar(20),
CusStatus char(1)
)


---------------Phone Number Table----------
--This table is used to add alternate phone numbers. 
Create table tbl_PhoneNumbers
(
PhoneId int primary key identity(1,1),
PhoneNumbers varchar(20),
Phn_fk_CusId int references tbl_Customer(CusId) on update cascade on delete cascade
)



--------------Address Table------------

create table tbl_CustomerAddresses(
AddressId int primary key identity(1,1),
AddresssName varchar(max),
DoorOrFlatNo varchar(10),
AddressType varchar(20),
Addr_fk_CusId int references tbl_Customer(CusId) on update cascade on delete cascade
)



--------------Dishes Table--------------

create table tbl_Dishes(
DishId int primary key identity(1,1),
DishName varchar(50) not null,
DishPrice money not null,
DishImage varchar(max) not null,
DishDesc varchar(max),
VegOrNonveg varchar(20) not null,
Dish_fk_Offer int references tbl_Offers(OfferId) on delete cascade on update cascade, 
Dish_fk_Rest int references tbl_Restaurant(RestId) on delete cascade on update cascade not null ,
Dish_fk_Cat int references tbl_Category(CatId) on delete cascade on update cascade not null
)

select * from tbl_Dishes



---------------Delivery Guys Table---------

create table tbl_DeliveryStaffs(
StaffId int primary key identity(1,1),
StaffName varchar(20) not null,
StaffPhone varchar(20) not null,
StaffEmail varchar(50) not null unique,
StaffDob date,
JoinDate date,
Gender varchar(20) not null,
VehicleNo varchar(20) not null,
VehicleType varchar(20) not null,
DrivingLicense varchar(20) not null,
AdhaarNo varchar(20) not null,
StaffAccStatus char(1) not null,
Isfree varchar(5)
)

drop table tbl_DeliveryStaffs



------------------ Favorite Restaurant Table-----------

create table tbl_FavRestaurants(
FavId int primary key identity(1,1),
Fav_fk_CusId int references tbl_Customer(CusId) on update cascade on delete cascade,
Fav_fk_RestId int references tbl_Restaurant(RestId) on update cascade on delete cascade
)
drop table tbl_FavRestaurants


-----------------Customer Review Table---------------

Create table tbl_CusReview(
RevId int primary key identity(1,1),
Review varchar(max),
Rating float(4) default 0,
Rev_fk_CusId int references tbl_Customer(CusId) on update cascade on delete cascade,
Rev_fk_RestId int references tbl_Restaurant(RestId) on update cascade on delete cascade
)

drop table tbl_CusReview

-----------------Cart Table---------------

create table tbl_Cart(
CartId int primary key identity(1,1),
AddedDate date,
Quantity int,
Cart_fk_DishId int references tbl_Dishes(DishId),
Cart_fk_CusId int references tbl_Customer(CusId),
Cart_fk_RestId int references tbl_Restaurant(RestId)
)
drop table tbl_Cart

---------------Contact us table---------------

Create table tbl_ContactUs(
ContId int primary key identity(1,1),
ContName varchar(50) not null,
ContEmail varchar(50) not null,
ContMsg varchar(max) not null
)


---------------Address Table-------------

create table tbl_Addresses(
AddId int primary key identity(1,1),
DoorOrFlatNo varchar(20),
LandMark varchar(50),
AddressType int references tbl_AddressType(TypeId) on update cascade on delete cascade,
PinCode varchar(20),
Add_fk_CusId int references tbl_Customer(CusId) on update cascade on delete cascade
)

drop table tbl_Addresses

--------------Address Type --------------
create table tbl_AddressType(
TypeId int primary key identity(1,1),
TypeName varchar(20)
)


----------------- Login table----------
create table tbl_Login(
LoginId int primary key identity(1,1),
UserId varchar(50) unique,
UserPassword varchar(20),
UserRole int references tbl_Role(RoleId) on update cascade on delete cascade
)
drop table  tbl_Login

-----------------Role table----------
create table tbl_Role(
RoleId int primary key identity(1,1),
RollName varchar(20)
)

select * from tbl_Customer
select * from tbl_Role
alter table  tbl_customer add CusRole int references tbl_Role(RoleId) on update cascade on delete cascade
alter table tbl_Login drop column Id 


------------------OrderDetails Table------------

create table tbl_OrderDetails(
OrderId int primary  key identity(1,1),
Order_fk_CusId int references tbl_Customer(CusId),
Order_fk_AddId int references tbl_Addresses(AddId) ,
Order_fk_RestId int references tbl_Restaurant(RestId) ,
Order_fk_StaffId int references tbl_DeliveryStaffs(StaffId) on update cascade ,
TotalAmount money,
Orderdate date,
PaymentMode varchar(20),
IsPaid char(1),
IsDelivered char(1),
IsOrderConfirmed char(1),
OrderOtp varchar(20)

)


------------Ordered food details table------------

create table tbl_OrderedFoodDetails(
id int primary key identity(1,1),
fk_OrderId int  references tbl_OrderDetails(OrderId) on update cascade on delete cascade,
fk_DishId int  references tbl_Dishes(DishId) on update cascade on delete cascade
)

alter table tbl_OrderedFoodDetails add DishQuantity int


------------UsersBankAcc------------------------

Create table tbl_UserBankAcc(
id int  primary key identity(1,1),
User_fk_BankName int  references tbl_BankNames(BankId) on update cascade on delete cascade,
Branch varchar(50),
AccNumber varchar(50) not null unique,
IfscCode varchar(50),
UserEmailId varchar(50),
rder_fk_CusId int references tbl_Customer(CusId) on update cascade on delete cascade

)

alter table tbl_UserBankAcc add UserRole int references tbl_Role(RoleId) 


------------UsersBankAcc------------------------

Create table tbl_ResBankAcc(
id int  primary key identity(1,1),
Rest_fk_BankName int  references tbl_BankNames(BankId) on update cascade on delete cascade,
Branch varchar(50),
AccNumber varchar(50) not null unique,
IfscCode varchar(50),
bank_fk_RestId int references tbl_Restaurant(RestId) on update cascade on delete cascade

)
---------------Common Bank Accounts----------------
create table tbl_BankAccounts(
AccId int primary key identity(1,1),
fk_BankName int  references tbl_BankNames(BankId) on update cascade on delete cascade,
Branch varchar(50),
AccNumber varchar(50) not null unique,
IfscCode varchar(50),
AccBalance money,
PinNumber varchar(20),
EmailId varchar(50)
)

drop table tbl_BankAccounts

------------Bank Names------------------

create table tbl_BankNames(
BankId int primary key identity(1,1),
BankName varchar(50)
)

create table tbl_ReviewAndRating(
RevId int primary key identity(1,1),
ReviewContent varchar(max),
Rating int,
Rev_fk_CusId int references tbl_Customer(CusId) on update cascade on delete cascade,
Rev_fk_RestId int references tbl_Restaurant(RestId) on update cascade on delete cascade

)

alter table tbl_ReviewAndRating add PostedDate date






--------------triggers----------------
--------------Cutomer - Login Triggers----
alter trigger tr_insertcredentials on tbl_Customer for insert
as
begin
declare @UserId as varchar(50),@UserPassword as varchar(30),@UserRole as int,@UserStatus as char(1)
select @userid=(select CusEmail from inserted)
select @UserPassword=(select CusPassword from inserted)
select @UserRole=(select CusRole from inserted)
select @UserStatus=(select CusStatus from inserted)
insert into tbl_Login (UserId,UserPassword,UserRole,UserStatus) values (@userid,@UserPassword,@UserRole,@UserStatus)
end

--------------------------------------------
create trigger tr_deletecredentials on tbl_Customer for delete
as
begin
declare @UserId as varchar(50)
select @UserId=(select CusEmail from deleted)
delete from tbl_Login where UserId=@UserId
end

------------------------------------------------
create trigger tr_Updatedetails on tbl_Customer for update
as
begin
declare @UserPassword as varchar(20),@UserStatus as char(1),@UserId as varchar(50)
select @UserPassword=(select CusPassword from inserted)
select @UserStatus=(select CusStatus from inserted)
select @UserId=(select CusEmail from inserted)


if UPDATE(CusPassword)
begin
update tbl_Login set UserPassword=@UserPassword where UserId=@UserId
end
if UPDATE(CusStatus)
begin
update tbl_Login set UserStatus=@UserStatus where UserId=@UserId
end
end



--------------Restaurant - Login Triggers----


alter trigger tr_insertcredentialsRestaurant on tbl_Restaurant  for insert
as
begin
declare @UserId as varchar(50),@UserPassword as varchar(30),@UserRole as int,@UserStatus as char(1)
select @userid=(select RestEmail from inserted)
select @UserPassword=(select RestPassword from inserted)
select @UserRole=(select RestRole from inserted)
select @UserStatus=(select RestStatus from inserted)
insert into tbl_Login (UserId,UserPassword,UserRole,UserStatus) values (@userid,@UserPassword,@UserRole,@UserStatus)
end

---------------------------------

create trigger tr_deletecredentialsRestaurant on tbl_Restaurant for delete
as
begin
declare @UserId as varchar(50)
select @UserId=(select RestEmail from deleted)
delete from tbl_Login where UserId=@UserId
end

------------------------------------------------

create trigger tr_UpdatedetailsRestaurant on tbl_Restaurant for update
as
begin
declare @UserPassword as varchar(20),@UserStatus as char(1),@UserId as varchar(50)
select @UserPassword=(select RestPassword from inserted)
select @UserStatus=(select RestStatus from inserted)
select @UserId=(select RestEmail from inserted)


if UPDATE(RestPassword)
begin
update tbl_Login set UserPassword=@UserPassword where UserId=@UserId
end
if UPDATE(RestStatus)
begin
update tbl_Login set UserStatus=@UserStatus where UserId=@UserId
end
end




--------------Delivery boy - Login Triggers----

create trigger tr_insertcredentialsDelBoy on tbl_DeliveryStaffs  for insert
as
begin
declare @UserId as varchar(50),@UserPassword as varchar(30),@UserRole as int,@UserStatus as char(1)
select @userid=(select StaffEmail from inserted)
select @UserPassword=(select StaffPassword from inserted)
select @UserRole=(select StaffRole from inserted)
select @UserStatus=(select StaffAccStatus from inserted)
insert into tbl_Login (UserId,UserPassword,UserRole,UserStatus) values (@userid,@UserPassword,@UserRole,@UserStatus)
end

---------------------------------

create trigger tr_deletecredentialsDelBoy on tbl_DeliveryStaffs for delete
as
begin
declare @UserId as varchar(50)
select @UserId=(select StaffEmail from deleted)
delete from tbl_Login where UserId=@UserId
end

------------------------------------------------

create trigger tr_UpdatedetailsDelBoy on tbl_DeliveryStaffs for update
as
begin
declare @UserPassword as varchar(20),@UserStatus as char(1),@UserId as varchar(50)
select @UserPassword=(select StaffPassword from inserted)
select @UserStatus=(select StaffAccStatus from inserted)
select @UserId=(select StaffEmail from inserted)


if UPDATE(StaffPassword)
begin
update tbl_Login set UserPassword=@UserPassword where UserId=@UserId
end
if UPDATE(StaffAccStatus)
begin
update tbl_Login set UserStatus=@UserStatus where UserId=@UserId
end
end



select * from tbl_OrderDetails
select * from tbl_Restaurant
delete from tbl_Customer where CusEmail='Adharsh@gmail.com'

select * from tbl_DeliveryStaffs

alter table tbl_DeliveryStaffs add staffImage varchar(max)
alter table tbl_Restaurant add IsValid varchar(10)


update tbl_Customer set CusStatus='D' where CusEmail='Adharsh@gmail.com'


--------------Stored procedure to accept order--------------------

create proc sp_AcceptOrder(@staffemail as varchar(50),
@orderId as int)
as
begin
begin transaction
declare @result as varchar(20)
declare @staffId as int
set @staffId= (select staffId from tbl_DeliveryStaffs where staffEmail=@staffemail)
update tbl_OrderDetails set Order_fk_StaffId=@staffId where OrderId=@orderId
update tbl_DeliveryStaffs set Isfree='No' where StaffId=@staffId
if(@@ERROR>0)
begin
set @result='failed'
rollback transaction
end
else
begin
set @result='success'
end
select @result
commit transaction
end



--------------------Payment stored procedure--------------------

alter proc sp_Payment(
@CusAccNum as varchar(20),
@RestAccNum as varchar(20),
@amount as money
)
as 
begin
begin transaction
declare @result as varchar(20)
update tbl_BankAccounts set AccBalance=AccBalance-@amount where AccNumber=@CusAccNum
update tbl_BankAccounts set AccBalance=AccBalance+@amount where AccNumber=@RestAccNum
if(@@ERROR>0)
begin
set @result='failed'
rollback transaction
end
else
begin
set @result='success'
end
select @result
commit transaction
end