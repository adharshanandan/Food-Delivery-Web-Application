create database db_FoodOrderingApplication


-----------------Admin Table----------

create table tbl_Admin(
AdmId int primary key identity(1,1),
AdmEmail varchar(50),
AdmPassword varchar(20)
)




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

select * from tbl_Login
select * from tbl_Customer
alter table  tbl_customer add CusRole int references tbl_Role(RoleId) on update cascade on delete cascade


--------------triggers----------------

create trigger tr_insertcredentials on tbl_Customer for insert
as
begin
declare @UserId as varchar(50),@UserPassword as varchar(30),@UserRole as int
select @userid=(select CusEmail from inserted)
select @UserPassword=(select CusPassword from inserted)
select @UserRole=(select CusRole from inserted)
insert into tbl_Login (UserId,UserPassword,UserRole) values (@userid,@UserPassword,@UserRole)
end