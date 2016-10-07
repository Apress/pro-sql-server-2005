using System;
using System.Data;
using System.Data.Sql;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Data.SqlClient;


public partial class StoredProcedures
{
    [SqlProcedure]
    public static void GetSalesPerTerritoryByMonth(SqlDateTime StartDate,
                                                    SqlDateTime EndDate)
    {
        //Get a SqlCommand object
        SqlCommand command = new SqlCommand();

        //Use the context connection
        command.Connection = new SqlConnection("Context connection=true");
        command.Connection.Open();

        //Define the T-SQL to execute
        string sql =
            "SELECT DISTINCT " +
                "CONVERT(CHAR(7), h.OrderDate, 120) AS YYYY_MM " +
            "FROM Sales.SalesOrderHeader h " +
            "WHERE h.OrderDate BETWEEN @StartDate AND @EndDate " +
            "ORDER BY YYYY_MM";
        command.CommandText = sql.ToString();

        //Assign the StartDate and EndDate parameters
        SqlParameter param =
            command.Parameters.Add("@StartDate", SqlDbType.DateTime);
        param.Value = StartDate;
        param = command.Parameters.Add("@EndDate", SqlDbType.DateTime);
        param.Value = EndDate;

        //Get the data
        SqlDataReader reader = command.ExecuteReader();

        //Get a StringBuilder object
        System.Text.StringBuilder yearsMonths = new System.Text.StringBuilder();

        //Loop through each row in the reader, adding the value to the StringBuilder
        while (reader.Read())
        {
            yearsMonths.Append("[" + (string)reader["YYYY_MM"] + "], ");
        }

        //Close the reader
        reader.Close();

        if (yearsMonths.Length > 0)
        {
            //Remove the final comma in the list
            yearsMonths.Remove(yearsMonths.Length - 2, 1);
        }
        else
        {
            command.CommandText =
                "RAISERROR('No data present for the input date range.', 16, 1)";
            try
            {
                SqlContext.Pipe.ExecuteAndSend(command);
            }
            catch
            {
                return;
            }
        }

        //Define the cross-tab query
        sql =
            "SELECT TerritoryId, " +
                    yearsMonths.ToString() +
            "FROM " +
            "(" +
                "SELECT " +
                    "TerritoryId, " +
                    "CONVERT(CHAR(7), h.OrderDate, 120) AS YYYY_MM, " +
                    "d.LineTotal " +
                "FROM Sales.SalesOrderHeader h " +
                "JOIN Sales.SalesOrderDetail d " +
                    "ON h.SalesOrderID = d.SalesOrderID " +
                "WHERE h.OrderDate BETWEEN @StartDate AND @EndDate " +
            ") p " +
            "PIVOT " +
            "( " +
                "SUM (LineTotal) " +
                "FOR YYYY_MM IN " +
                "( " +
                    yearsMonths.ToString() +
                ") " +
            ") AS pvt " +
            "ORDER BY TerritoryId";

        //Set the CommandText
        command.CommandText = sql.ToString();

        //Have the caller execute the cross-tab query
        SqlContext.Pipe.ExecuteAndSend(command);

        //Close the connection
        command.Connection.Close();
    }
};
