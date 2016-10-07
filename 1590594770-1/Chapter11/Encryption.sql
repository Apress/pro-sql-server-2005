DECLARE @creditCardNum varchar(20),
   @passPhrase varchar(255),
   @cipherText varchar(1000),
   @clearText varchar(1000);
SET @creditCardNum = '34324521093872';
SET @passPhrase = 'fe34&*$PO2hs';
SET @cipherText = EncryptByPassPhrase(@passPhrase, @creditCardNum);
PRINT 'Cipher text = ' + @cipherText;

SET @clearText = DecryptByPassPhrase(@passPhrase, @cipherText);
PRINT 'Clear text = ' + @clearText;
GO

CREATE SYMMETRIC KEY CreditCardKey
WITH ALGORITHM = TRIPLE_DES
ENCRYPTION BY PASSWORD = 'fe34&*$PO2hs';

DECLARE @keyGuid UNIQUEIDENTIFIER,
   @creditCardNum varchar(20),
   @password varchar(255),
   @cipherText varchar(1000),
   @clearText varchar(1000);
SET @keyGuid = Key_GUID('CreditCardKey');
SET @creditCardNum = '12345678';

OPEN SYMMETRIC KEY CreditCardKey
   DECRYPTION BY PASSWORD = 'fe34&*$PO2hs';
SET @cipherText = EncryptByKey(@keyGuid, @creditCardNum);
PRINT 'Cipher text = ' + @cipherText;
SET @clearText = DecryptByKey(@cipherText);
PRINT 'Clear text = ' + @clearText;
CLOSE SYMMETRIC KEY CreditCardKey;
GO

CREATE ASYMMETRIC KEY PpeKey
WITH ALGORITHM = RSA_512
ENCRYPTION BY PASSWORD = 'fe34&*$PO2hs';

DECLARE @keyId int,
   @creditCardNum varchar(20),
   @password varchar(255),
   @cipherText varchar(1000),
   @clearText varchar(1000);
SET @keyId = AsymKey_ID('PpeKey');
SET @creditCardNum = '12345678';

SET @cipherText = EncryptByAsymKey(@keyId, @creditCardNum);
PRINT 'Cipher text = ' + @cipherText;
SET @clearText = DecryptByAsymKey(@keyId, @cipherText,
   CAST('fe34&*$PO2hs' AS nvarchar));
PRINT 'Clear text = ' + @clearText;
GO

CREATE CERTIFICATE TestCertificate
ENCRYPTION BY PASSWORD = 'fe34&*$PO2hs'
WITH START_DATE = '04/04/2005',
     EXPIRY_DATE = '04/04/2006',
     SUBJECT = 'certserver.apress.com';

DECLARE @certId int,
   @creditCardNum varchar(20),
   @password varchar(255),
   @cipherText varchar(1000),
   @clearText varchar(1000);
SET @certId = Cert_ID('TestCertificate');
SET @creditCardNum = '12345678';

SET @cipherText = EncryptByCert(@certId, @creditCardNum);
PRINT 'Cipher text = ' + @cipherText;
SET @clearText = DecryptByCert(@certId, @cipherText,
   CAST('fe34&*$PO2hs' AS nvarchar));
PRINT 'Clear text = ' + @clearText;
GO