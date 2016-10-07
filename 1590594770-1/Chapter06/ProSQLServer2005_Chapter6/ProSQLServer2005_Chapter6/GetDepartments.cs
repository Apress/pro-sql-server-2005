using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Collections;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction(
        DataAccess = DataAccessKind.Read,
        FillRowMethodName = "GetNextDepartment",
        TableDefinition = "Name NVARCHAR(50), GroupName NVARCHAR(50)")]
    public static IEnumerable GetDepartments()
    {
        using (SqlConnection conn =
            new SqlConnection("context connection=true;"))
        {
            string sql =
                "SELECT Name, GroupName FROM HumanResources.Department";
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataAdapter adaptor = new SqlDataAdapter(comm);
            DataSet dSet = new DataSet();
            adaptor.Fill(dSet);
            return (dSet.Tables[0].Rows);
        }
    }

    public static void GetNextDepartment(object row,
        out string name,
        out string groupName)
    {
        DataRow theRow = (DataRow)row;
        name = (string)theRow["Name"];
        groupName = (string)theRow["GroupName"];
    }
};
