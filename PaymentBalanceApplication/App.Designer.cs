namespace PaymentBalanceApplication
{
    partial class App
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.gpOptions = new System.Windows.Forms.GroupBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAddCashback = new System.Windows.Forms.Button();
            this.btnAddPayment = new System.Windows.Forms.Button();
            this.btnAddBalance = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintTxt = new System.Windows.Forms.Button();
            this.gpOptions.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpOptions
            // 
            this.gpOptions.Controls.Add(this.btnEdit);
            this.gpOptions.Controls.Add(this.btnClear);
            this.gpOptions.Controls.Add(this.btnAddCashback);
            this.gpOptions.Controls.Add(this.btnAddPayment);
            this.gpOptions.Controls.Add(this.btnAddBalance);
            this.gpOptions.Location = new System.Drawing.Point(12, 12);
            this.gpOptions.Name = "gpOptions";
            this.gpOptions.Size = new System.Drawing.Size(372, 106);
            this.gpOptions.TabIndex = 1;
            this.gpOptions.TabStop = false;
            this.gpOptions.Text = "Transaction Options";
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(186, 61);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(173, 23);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnClear
            // 
            this.btnClear.Enabled = false;
            this.btnClear.Location = new System.Drawing.Point(6, 61);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(173, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear All";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAddCashback
            // 
            this.btnAddCashback.Location = new System.Drawing.Point(256, 19);
            this.btnAddCashback.Name = "btnAddCashback";
            this.btnAddCashback.Size = new System.Drawing.Size(103, 23);
            this.btnAddCashback.TabIndex = 2;
            this.btnAddCashback.Text = "Add a Cashback";
            this.btnAddCashback.UseVisualStyleBackColor = true;
            this.btnAddCashback.Click += new System.EventHandler(this.btnAddCashback_Click);
            // 
            // btnAddPayment
            // 
            this.btnAddPayment.Location = new System.Drawing.Point(131, 19);
            this.btnAddPayment.Name = "btnAddPayment";
            this.btnAddPayment.Size = new System.Drawing.Size(103, 23);
            this.btnAddPayment.TabIndex = 1;
            this.btnAddPayment.Text = "Add a Payment";
            this.btnAddPayment.UseVisualStyleBackColor = true;
            this.btnAddPayment.Click += new System.EventHandler(this.btnAddPayment_Click);
            // 
            // btnAddBalance
            // 
            this.btnAddBalance.Location = new System.Drawing.Point(6, 19);
            this.btnAddBalance.Name = "btnAddBalance";
            this.btnAddBalance.Size = new System.Drawing.Size(103, 23);
            this.btnAddBalance.TabIndex = 0;
            this.btnAddBalance.Text = "Add a Balance";
            this.btnAddBalance.UseVisualStyleBackColor = true;
            this.btnAddBalance.Click += new System.EventHandler(this.btnAddBalance_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gpOptions);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(399, 124);
            this.panel1.TabIndex = 2;
            // 
            // txtSummary
            // 
            this.txtSummary.BackColor = System.Drawing.Color.White;
            this.txtSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSummary.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSummary.Location = new System.Drawing.Point(0, 124);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSummary.Size = new System.Drawing.Size(399, 591);
            this.txtSummary.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnImport);
            this.panel2.Controls.Add(this.btnExport);
            this.panel2.Controls.Add(this.btnPrintTxt);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 667);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(399, 48);
            this.panel2.TabIndex = 4;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(98, 12);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(81, 23);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(11, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(81, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPrintTxt
            // 
            this.btnPrintTxt.Enabled = false;
            this.btnPrintTxt.Location = new System.Drawing.Point(267, 12);
            this.btnPrintTxt.Name = "btnPrintTxt";
            this.btnPrintTxt.Size = new System.Drawing.Size(103, 23);
            this.btnPrintTxt.TabIndex = 0;
            this.btnPrintTxt.Text = "Save Text";
            this.btnPrintTxt.UseVisualStyleBackColor = true;
            this.btnPrintTxt.Click += new System.EventHandler(this.btnPrintTxt_Click);
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 715);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.txtSummary);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(415, 39);
            this.Name = "App";
            this.Text = "Balance-Payment App";
            this.gpOptions.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gpOptions;
        private System.Windows.Forms.Button btnAddCashback;
        private System.Windows.Forms.Button btnAddPayment;
        private System.Windows.Forms.Button btnAddBalance;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtSummary;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnPrintTxt;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
    }
}

