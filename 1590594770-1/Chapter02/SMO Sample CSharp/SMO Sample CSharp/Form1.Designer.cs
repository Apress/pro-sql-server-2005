namespace SMO_Sample_CSharp
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.listDatabases = new System.Windows.Forms.ListBox();
            this.listTables = new System.Windows.Forms.ListBox();
            this.txtSQLScript = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
// 
// btnConnect
// 
            this.btnConnect.Location = new System.Drawing.Point(27, 13);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "&Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
// 
// btnClear
// 
            this.btnClear.Location = new System.Drawing.Point(140, 13);
            this.btnClear.Name = "btnClear";
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "C&lear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
// 
// listDatabases
// 
            this.listDatabases.FormattingEnabled = true;
            this.listDatabases.Location = new System.Drawing.Point(26, 71);
            this.listDatabases.Name = "listDatabases";
            this.listDatabases.Size = new System.Drawing.Size(332, 264);
            this.listDatabases.TabIndex = 2;
            this.listDatabases.SelectedIndexChanged += new System.EventHandler(this.listDatabases_SelectedIndexChanged);
// 
// listTables
// 
            this.listTables.FormattingEnabled = true;
            this.listTables.Location = new System.Drawing.Point(373, 69);
            this.listTables.Name = "listTables";
            this.listTables.Size = new System.Drawing.Size(298, 264);
            this.listTables.TabIndex = 3;
            this.listTables.SelectedIndexChanged += new System.EventHandler(this.listTables_SelectedIndexChanged);
// 
// txtSQLScript
// 
            this.txtSQLScript.AutoSize = false;
            this.txtSQLScript.Location = new System.Drawing.Point(29, 350);
            this.txtSQLScript.Multiline = true;
            this.txtSQLScript.Name = "txtSQLScript";
            this.txtSQLScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSQLScript.Size = new System.Drawing.Size(640, 151);
            this.txtSQLScript.TabIndex = 4;
// 
// Form1
// 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(700, 513);
            this.Controls.Add(this.txtSQLScript);
            this.Controls.Add(this.listTables);
            this.Controls.Add(this.listDatabases);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "SMO Sample Application";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ListBox listDatabases;
        private System.Windows.Forms.ListBox listTables;
        private System.Windows.Forms.TextBox txtSQLScript;
    }
}

