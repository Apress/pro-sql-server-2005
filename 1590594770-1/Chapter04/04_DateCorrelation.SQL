--Enable date correlation optimization

ALTER DATABASE AdventureWorks
SET DATE_CORRELATION_OPTIMIZATION ON





--Determine whether date correlation optimization is turned on

SELECT
    Name,
    is_date_correlation_on
FROM sys.databases

