using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.SqlServer.Server;

namespace Apress.SqlServer2005.SecurityChapter
{
   public class SalesFetcher
   {
      public static void GetSalesForNames(SqlString filename)
      {
         try
         {
            PermissionSet perms = new PermissionSet(PermissionState.None);

            // Ensure that only correct file can be accessed through this method
            FileIOPermission ioPerm = new FileIOPermission(
                 FileIOPermissionAccess.Read, @"C:\names.txt");
            perms.AddPermission(ioPerm);

            // Permit access to SQL Server data
            SqlClientPermission sqlPerm = new SqlClientPermission(
                                                 PermissionState.None);
            sqlPerm.Add("context connection=true", "",
                        KeyRestrictionBehavior.AllowOnly);
            perms.AddPermission(sqlPerm);
            perms.PermitOnly();

            // Get the names from the text file as a string array
            string[] names = FileReader.ReadFile(filename.ToString());

            // Build SQL statement
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT emp.EmployeeID,
                               sp.SalesYTD + sp.SalesLastYear AS RecentSales
                        FROM Sales.SalesPerson sp
                           INNER JOIN HumanResources.Employee emp
                           ON emp.EmployeeID = sp.SalesPersonID
                        WHERE sp.SalesPersonID IN
                        (
                           SELECT emp.EmployeeID
                           FROM HumanResources.Employee emp
                              INNER JOIN Person.Contact c
                              ON c.ContactID = emp.ContactID
                           WHERE c.FirstName + ' ' + c.MiddleName + ' ' +
                                 c.LastName
                           IN (");

            // Concatenate array into single string for WHERE clause
            foreach (string name in names)
            {
               sb.Append("'");
               sb.Append(name);
               sb.Append("', ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append("))");

            // Execute the SQL statement and get back a SqlResultSet
            using (SqlConnection cn = new SqlConnection(
                                             "context connection=true"))
            {
               cn.Open();
               SqlCommand cmd = new SqlCommand(sb.ToString(), cn);
               SqlDataReader dr = cmd.ExecuteReader();

               // Send success message to SQL Server and return SqlDataReader
               SqlPipe pipe = SqlContext.Pipe;
               pipe.Send(dr);
               pipe.Send("Command(s) completed successfully.");
               cn.Close();
            }
         }
         catch (Exception e)
         {
            SqlPipe pipe = SqlContext.Pipe;
            pipe.Send(e.Message);
            pipe.Send(e.StackTrace);
            pipe.Send("Error executing assembly");
         }
      }
   }
}