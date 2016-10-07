using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction(FillRowMethodName = "GetNextString",
        TableDefinition = "StringCol NVARCHAR(MAX)")]
    public static IEnumerable GetTableFromStringArray(StringArray strings)
    {
        string csv = strings.ToString();
        string[] arr = csv.Split(',');
        return arr;
    }

    public static void GetNextString(object row, out string theString)
    {
        theString = (string)row;
    }
};
