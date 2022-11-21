create table "Type"(
	TypeID int,
	SubType nvarchar(max),
	MainType nvarchar(max)
	primary key (TypeID)
)


create table Product(
	ProID  int,
	ProName nvarchar(max),
	Price decimal(18,2) ,
	TypeID int,
	Source nvarchar(max),
	StartDate datetime,
	Des	nvarchar(max),
	Unit nvarchar(max),
	isDelet int,
	primary key (ProID)
)


create table Product_Image
(
	ImgID int,
	ProID int,
	ImgData nvarchar(max),
	primary key (ImgID)
)


create table Address
(	
	AddID int,
	CusID int,
	City nvarchar(max),
	District nvarchar(max),
	Ward nvarchar(max),
	SpecificAddress nvarchar(max),
	"Default" int,
	primary key (AddID)
)


create table Transport
(		
	TransID int,
	ShippingDate datetime,
	Transporter nvarchar(max),
	"Status" int,
	Fee decimal(18,2),
	Type int,
	primary key (TransID)
)

create table Store
(	
	StoreID int,
	City nvarchar(max),
	District nvarchar(max),
	Ward nvarchar(max),
	SpecificAddress nvarchar(max),
	primary key (StoreID)
)

create table Discount_All
(	
	DisID int,
	Value float,
	StartDate datetime,
	EndDate datetime,
	primary key (DisID)
)

create table Feedback
(	
	FbID int,
	ReplyID int,
	ProID int,
	CusID int,
	OrderID int,
	"Message" nvarchar(max),
	SendDate datetime,
	Rate int,
	"like" int,
	primary key (FbID)
)

create table Feedback_Image
(		
	FbImgID int,
	ImgData nvarchar(max),
	FbID int,
	primary key (FbImgID)
)

create table Discount_Type
(			
	DisID int,
	TypeID int,
	"Value" float,
	StartDate datetime,
	EndDate datetime,
	primary key (DisID)
)


create table Discount_Store
(			
	DisID int,
	StoreID int,
	Value float,
	StartDate datetime,
	EndDate datetime,
	primary key (DisID)
)


create table "Order"
(				
	OrderID int,
	AddID int,
	TransID	int,
	DisID int,
	StoreID int,
	OrderDate datetime,
	City nvarchar(max),
	District nvarchar(max),
	Ward nvarchar(max),
	SpecificAddr nvarchar(max),
	PreTotal decimal(18,2),
	OrderTotal decimal(18,2),
	PayBy nvarchar(max),
	Status int,
	primary key (OrderID)
)


create table Employee
(				
	EmpID int,
	UserID int,
	StoreID int,
	FirstName nvarchar(max),
	LastName nvarchar(max),
	Gender int,
	City nvarchar(max),
	District nvarchar(max),
	Ward nvarchar(max),
	SpecificAddress nvarchar(max),
	StartDate datetime,
	EndDate datetime,
	Phone nvarchar(max),
	Email nvarchar(max),
	primary  key (EmpID)
)


create table Discount_Product
(		
	DisID int,
	ProID int,
	Value float,
	StartDate datetime,
	EndDate datetime,
	primary key (DisID)
)


create table Discount_order
(		
	DisID int,
	DisCode nvarchar(max),
	DisRate float,
	MaxDis decimal,
	StartDate datetime,
	EndDate datetime,
	CusType int,
	primary key (DisID)
)

create table Reward
(		
	RewardID int,
	CusType int,
	Point int,
	primary key (RewardID)
)

create table Customer
(			
	CusID int,
	UserID int,
	RewardID int,
	FirstName nvarchar(max),
	LastName nvarchar(max),
	Gender int,
	Phone nvarchar(max),
	Email nvarchar(max),
	primary key (CusID)
)


create table QnA
(			
	QnAID int,
	ReplyID int,
	ProID int,
	CusID int,
	"Message" nvarchar(max),
	SendDate datetime,
	"like" int,
	primary key (QnAID)
)


create table QnA_Image
(			
	QnAImgID int,
	QnAID int,
	ImgData nvarchar(max),
	primary key (QnAImgID)
)


create table "User"
(				
	UserID int,
	UserName nvarchar(max),
	UserPassword nvarchar(max),
	UserRole int,
	primary key (UserID)
)

create table Order_Details
(					
	OrderID int,
	ProID int,
	Quantity int
	primary key (OrderID,ProID)
)


create table Stock
(					
	StoreID int,
	ProID int,
	Quantity int
	primary key (StoreID,ProID)
)


-------------------------------
-------------------------------
ALTER TABLE Product
	ADD CONSTRAINT fk_Product_1
	FOREIGN KEY (TypeID)
	REFERENCES "Type" (TypeID);


ALTER TABLE Product_Image
	ADD CONSTRAINT fk_Product_Image_1
	FOREIGN KEY (ProID)
	REFERENCES Product (ProID);


ALTER TABLE Address
	ADD CONSTRAINT fk_Address_1
	FOREIGN KEY (CusID)
	REFERENCES Customer (CusID);


ALTER TABLE Feedback
	ADD CONSTRAINT fk_Feedback_1
	FOREIGN KEY (ReplyID)
	REFERENCES Feedback (FbID);
ALTER TABLE Feedback
	ADD CONSTRAINT fk_Feedback_2
	FOREIGN KEY (ProID)
	REFERENCES Product (ProID);
ALTER TABLE Feedback
	ADD CONSTRAINT fk_Feedback_3
	FOREIGN KEY (CusID)
	REFERENCES Customer (CusID);
ALTER TABLE Feedback
	ADD CONSTRAINT fk_Feedback_4
	FOREIGN KEY (OrderID)
	REFERENCES "Order" (OrderID);


ALTER TABLE Feedback_Image
	ADD CONSTRAINT fk_Feedback_Image_1
	FOREIGN KEY (FbID)
	REFERENCES Feedback (FbID);


ALTER TABLE Discount_Type
	ADD CONSTRAINT fk_Discount_Type_1
	FOREIGN KEY (TypeID)
	REFERENCES "Type" (TypeID);


ALTER TABLE Discount_Store
	ADD CONSTRAINT fk_Discount_Store_1
	FOREIGN KEY (StoreID)
	REFERENCES Store (StoreID);


ALTER TABLE "Order"
	ADD CONSTRAINT fk_Order_1
	FOREIGN KEY (AddID)
	REFERENCES "Address" (AddID);
ALTER TABLE "Order"
	ADD CONSTRAINT fk_Order_2
	FOREIGN KEY (TransID)
	REFERENCES Transport (TransID);
ALTER TABLE "Order"
	ADD CONSTRAINT fk_Order_3
	FOREIGN KEY (DisID)
	REFERENCES Discount_order (DisID);
ALTER TABLE "Order"
	ADD CONSTRAINT fk_Order_4
	FOREIGN KEY (StoreID)
	REFERENCES Store (StoreID);


ALTER TABLE Employee
	ADD CONSTRAINT fk_Employee_1
	FOREIGN KEY (UserID)
	REFERENCES "User" (UserID);
ALTER TABLE Employee
	ADD CONSTRAINT fk_Employee_2
	FOREIGN KEY (StoreID)
	REFERENCES Store (StoreID);


ALTER TABLE Discount_Product
	ADD CONSTRAINT fk_Discount_Product_1
	FOREIGN KEY (ProID)
	REFERENCES Product (ProID);


ALTER TABLE Customer
	ADD CONSTRAINT fk_Customer_1
	FOREIGN KEY (UserID)
	REFERENCES "User" (UserID);
ALTER TABLE Customer
	ADD CONSTRAINT fk_Customer_2
	FOREIGN KEY (RewardID)
	REFERENCES Reward (RewardID);


ALTER TABLE QnA
	ADD CONSTRAINT fk_QnA_1
	FOREIGN KEY (ReplyID)
	REFERENCES QnA (QnAID);
ALTER TABLE QnA
	ADD CONSTRAINT fk_QnA_2
	FOREIGN KEY (ProID)
	REFERENCES Product (ProID);
ALTER TABLE QnA
	ADD CONSTRAINT fk_QnA_3
	FOREIGN KEY (CusID)
	REFERENCES Customer (CusID);


ALTER TABLE QnA_Image
	ADD CONSTRAINT fk_QnA_Image_1
	FOREIGN KEY (QnAID)
	REFERENCES QnA (QnAID);


ALTER TABLE Order_Details
	ADD CONSTRAINT fk_Order_Details_1
	FOREIGN KEY (OrderID)
	REFERENCES "Order" (OrderID);
ALTER TABLE Order_Details
	ADD CONSTRAINT fk_Order_Details_2
	FOREIGN KEY (ProID)
	REFERENCES Product (ProID);


ALTER TABLE Stock
	ADD CONSTRAINT fk_Stock_1
	FOREIGN KEY (StoreID)
	REFERENCES Store (StoreID);
ALTER TABLE Stock
	ADD CONSTRAINT fk_Stock_2
	FOREIGN KEY (ProID)
	REFERENCES Product (ProID);

