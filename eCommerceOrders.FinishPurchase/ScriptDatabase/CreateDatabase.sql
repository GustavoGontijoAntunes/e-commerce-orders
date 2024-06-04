CREATE DATABASE [db-eCommerceOrders];
USE [db-eCommerceOrders];

CREATE TABLE Product (
    Id INT IDENTITY(1,1) PRIMARY KEY ,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Price DECIMAL(18, 2) NOT NULL
);

CREATE TABLE [Order] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IsFinished BIT NOT NULL,
    TotalValue DECIMAL(11,2) NOT NULL
);

CREATE TABLE OrderProduct (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT,
    ProductId INT,
    Quantity INT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES [Order](Id),
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);