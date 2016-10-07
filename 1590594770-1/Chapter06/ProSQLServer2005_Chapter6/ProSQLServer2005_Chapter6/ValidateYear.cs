using System;
using System.Data;
using System.Data.Sql;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;

public partial class Triggers
{
    // Enter existing table or view for the target and uncomment the attribute line
    [Microsoft.SqlServer.Server.SqlTrigger(
        Name = "ValidateYear",
        Target = "HumanResources.Department",
        Event = "FOR INSERT")]
    public static void ValidateYear()
    {
        SqlConnection conn =
            new SqlConnection("context connection=true");

        //Define the query
        string sql =
            "SELECT COUNT(*) " +
            "FROM INSERTED " +
            "WHERE YEAR(ModifiedDate) <> 2005";

        SqlCommand comm =
            new SqlCommand(sql, conn);

        //Open the connection
        conn.Open();

        //Get the number of bad rows
        int numBadRows = (int)comm.ExecuteScalar();

        if (numBadRows > 0)
        {
            //Get the SqlPipe
            SqlPipe pipe = SqlContext.Pipe;

            //Rollback and raise an error
            comm.CommandText =
                "RAISERROR('Modified Date must fall in 2005', 11, 1)";

            //Send the error
            try
            {
                pipe.ExecuteAndSend(comm);
            }
            catch
            {
                //do nothing
            }

            System.Transactions.Transaction.Current.Rollback();
        }

        //Close the connection
        conn.Close();
    }
}
