Instructions for running the Simple Service Broker Example
==========================================================

To run this example, you will need to enable and configure Database Mail in (A)
and create a profile that the example can use to send e-mails. Please see
Chapter 13 for instructions on how to do that.

If the profile you want to use isn't called 'Default Profile', you will
need to change the calls to sp_send_dbmail in the usp_ProcessHolidayRequest
stored procedure to use your profile.




Instructions for running the Distributed Service Broker Example
===============================================================

To run this example, you need to create the objects in the correct order
in the correct instance of SQL Server 2005. To indicate which instance of
SQL Server to run a SQL script or perform another action on, we use the
following abbreviations:

(P) - The instance of SQL Server 2005 that hosts the Projects database
(A) - The instance of SQL Server 2005 that hosts the AdventureWorks database


The scripts create certificates and save them to file in a folder called
C:\Apress\ProSqlServer\Chapter12, so you will need to ensure that this folder
exists on both machines, or you will need to change the code to save the
certificates to another location.


To run the example, please follow these instructions:

(1)  Enable and configure Database Mail in (A), if you haven't already done so.
(2)  Run the Projects_endpoint.sql script (P).
(3)  Copy the generated certificate file projEndpointCert.cer from the (P) machine
     to a directory of the same name on the (A) machine.
(4)  Run the AW_endpoint.sql script (A).
(5)  Copy the generated certificate file awEndpointCert.cer from the (A) machine
     to the directory of the same name on the (P) machine.
(6)  Run the Projects_service.sql script (P).
(7)  Copy the generated certificate file projUserCert.cer from the (P) machine
     to the directory of the same name on the (A) machine.
(8)  Change the address in the CREATE ROUTE statement in the AW_services.sql script
     so that it points to the (P) machine in your setup and change the name of the
     login for the CREATE USER projUser statement to an existing login on your system.
     If you haven't created a database master key for AdventureWorks, uncomment the
     CREATE MASTER KEY statement.
(9)  Run the AW_services.sql script on the (A) machine.
(10) Copy the generated certificate file awUserCert.cer from the (A) machine
     to the directory of the same name on the (P) machine.
(11) Change the name of the login in the CREATE USER awUser statemet to an existing login
     and run the Projects_security.sql script on the (P) machine.
(12) Run the following command in the AdventureWorks database to make a vacation request.
     Change the parameters as appropriate, making sure that you pass in a valid e-mail address
     where you can receive e-mail:

     EXEC usp_RequestVacation 140, 'someone@somewhere.com', 8, '2005-08-01T09:00:00+00:00'

(13) Wait for a few seconds for the services to respond, and then execute the following command:

     EXEC usp_ReadResponseMessages;

     You should receive an e-mail after a short period.