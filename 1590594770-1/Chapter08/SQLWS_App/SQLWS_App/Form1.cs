#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Reflection;


#endregion

namespace SQLWS_App
{
	partial class Form1: Form
	{
		public Form1()
		{
			InitializeComponent();
		}

        private void button1_Click(object sender, EventArgs e)
        {
            //add a reference to our SQL WS
            ws.SQLWS_endpoint SQLWS = new ws.SQLWS_endpoint();
            
            //Set our default credentials to our Windows one
            SQLWS.Credentials = CredentialCache.DefaultCredentials;
            
            //Call the sproc through the WS
            System.Data.DataSet dsReturnValue = (System.Data.DataSet)SQLWS.SQLWS("Calling stored proc").GetValue(0);
            
            //Get the reader associated with our Dataset
            System.Data.DataTableReader drSQL = dsReturnValue.CreateDataReader();
            
            //Get the result
            string strResult = "";
            while (drSQL.Read())
            {
                strResult = drSQL[0].ToString();
            }

            //Display the results
            MessageBox.Show("Return value from SQL call: " + strResult);

            ws.SqlParameter[] sqlparams = new ws.SqlParameter[0];
            
            //Send a batch command to SQL
            System.Data.DataSet dsReturnValue1 = (System.Data.DataSet)SQLWS.sqlbatch("SELECT * FROM sys.http_endpoints", ref sqlparams).GetValue(0);

            //Get the reader associated with our Dataset
            System.Data.DataTableReader drSQL1 = dsReturnValue1.CreateDataReader();

            //Get the result
            string strResult1 = "";
            while (drSQL1.Read())
            {
                strResult1 = drSQL1[0].ToString();
            }

            //Display the results
            MessageBox.Show("Return value from SQL call: " + strResult1);

        }

        private void button2_Click(object sender, EventArgs e)
        {
        
        }    
	}
}