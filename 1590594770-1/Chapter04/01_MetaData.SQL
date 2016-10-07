--Find all columns for all tables

SELECT
    t.name AS theTable,
    c.name AS theColumn,
    ty.name AS theType
FROM sys.tables t
JOIN sys.columns c ON t.object_id = c.object_id
JOIN sys.types ty ON c.system_type_id = ty.system_type_id



--Find all tables without clustered indexes


SELECT
    OBJECT_NAME(object_id) AS theTable
FROM sys.indexes
WHERE
    type_desc = 'HEAP'


