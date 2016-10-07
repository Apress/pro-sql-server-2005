#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Specialized;


#endregion

namespace SMO_Sample_CSharp
{
	partial class Form1: Form
	{
        Server srvSQLServer = new Server();
        //You could also use dynamic arrays here
        //with ArrayList
        Database[] arrDBs = new Database[100];
        Table[] arrTables = new Table[1000];

        public Form1()
		{
			InitializeComponent();
		}

        private void btnConnect_Click(object sender, EventArgs e)
        {
            listDatabases.Items.Clear();
            listTables.Items.Clear();
            txtSQLScript.Clear();
            ClearArray();
            listDatabases.DisplayMember = "Name";


            int i = 0;
            foreach(Database tmpdb in srvSQLServer.Databases){
                if (tmpdb.IsSystemObject != true){
                    listDatabases.Items.Add(tmpdb.ToString());
                    arrDBs[i] = tmpdb;
                    i++;
                }
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listDatabases.Items.Clear();
            listTables.Items.Clear();
            txtSQLScript.Clear();
            ClearArray();
        }

        private void ClearArray()
        {
            for (int i = 0; i < arrDBs.Length; i++)
            {
                arrDBs[i] = null;
            }
            for (int i = 0; i < arrTables.Length; i++)
            {
                arrTables[i] = null;
            }
        }

        private void listDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            listTables.Items.Clear();
            txtSQLScript.Clear();
            listTables.DisplayMember = "ToString()";

            Database tmpdb = new Database();
            tmpdb = arrDBs[listDatabases.SelectedIndex];

            int i = 0;
            foreach (Table tmptable in tmpdb.Tables){
                if (tmptable.IsSystemObject != true){
                    listTables.Items.Add(tmptable.ToString());
                    arrTables[i] = tmptable;
                    i++;
                }
            }
        }

        private void listTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            StringCollection sc = new StringCollection();
            
            //Get the table's script
            sc = arrTables[listTables.SelectedIndex].Script();
            
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < sc.Count; i++)
            {
                sb.AppendLine(sc[i]);
            }

            txtSQLScript.Text = sb.ToString();

        }
    }
} 