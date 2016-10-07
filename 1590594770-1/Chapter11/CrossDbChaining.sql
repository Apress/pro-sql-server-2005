sp_configure 'cross db ownership chaining', 1
GO
RECONFIGURE
GO

USE AdventureWorks
GO

CREATE DATABASE CustomerData
WITH DB_CHAINING ON

CREATE DATABASE MailingList
WITH DB_CHAINING ON
GO
CREATE LOGIN William WITH PASSWORD = '?sdj7JS3&*(%sdp_';

USE CustomerData
CREATE TABLE Customers
(
   CustomerID int IDENTITY PRIMARY KEY,
   FirstName nvarchar(255) NOT NULL,
   LastName nvarchar(255) NOT NULL,
   Email varchar(255) NOT NULL
);

INSERT INTO Customers VALUES ('John', 'Smith', 'John.Smith@somewhere.com');
INSERT INTO Customers VALUES ('Jane', 'Jones', 'JaneJ@somewhereelse.com');
GO

CREATE USER William FOR LOGIN William;
GO

USE MailingList
CREATE TABLE EmailAddresses
(
   ContactID int IDENTITY PRIMARY KEY,
   Email varchar(255) NOT NULL
);

INSERT INTO EmailAddresses VALUES('gprice@somedomain.com');
INSERT INTO EmailAddresses VALUES('fredb@anotherdomain.com');
GO

CREATE VIEW vGetAllContactEmails
AS
SELECT Email FROM EmailAddresses
UNION
SELECT Email FROM CustomerData.dbo.Customers;
GO

CREATE USER William FOR LOGIN William;
GRANT SELECT ON vGetAllContactEmails TO William;
GO

SETUSER 'William'
SELECT * FROM vGetAllContactEmails
SETUSER

SETUSER 'William'
SELECT Email FROM EmailAddresses
UNION
SELECT Email FROM CustomerData.dbo.Customers;
SETUSER
