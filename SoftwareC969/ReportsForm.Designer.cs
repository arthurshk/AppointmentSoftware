namespace SoftwareC969
{
    partial class ReportsForm
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
            this.dgvReport1 = new System.Windows.Forms.DataGridView();
            this.dgvReport2 = new System.Windows.Forms.DataGridView();
            this.dgvReport3 = new System.Windows.Forms.DataGridView();
            this.btnGenerateReports = new System.Windows.Forms.Button();
            this.numberAppointmentLbl = new System.Windows.Forms.Label();
            this.userScheduleLbl = new System.Windows.Forms.Label();
            this.appointmentPerCustomerLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport3)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvReport1
            // 
            this.dgvReport1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport1.Location = new System.Drawing.Point(12, 158);
            this.dgvReport1.Name = "dgvReport1";
            this.dgvReport1.RowHeadersWidth = 62;
            this.dgvReport1.RowTemplate.Height = 28;
            this.dgvReport1.Size = new System.Drawing.Size(251, 280);
            this.dgvReport1.TabIndex = 0;
            // 
            // dgvReport2
            // 
            this.dgvReport2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport2.Location = new System.Drawing.Point(269, 158);
            this.dgvReport2.Name = "dgvReport2";
            this.dgvReport2.RowHeadersWidth = 62;
            this.dgvReport2.RowTemplate.Height = 28;
            this.dgvReport2.Size = new System.Drawing.Size(262, 280);
            this.dgvReport2.TabIndex = 1;
            // 
            // dgvReport3
            // 
            this.dgvReport3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport3.Location = new System.Drawing.Point(537, 158);
            this.dgvReport3.Name = "dgvReport3";
            this.dgvReport3.RowHeadersWidth = 62;
            this.dgvReport3.RowTemplate.Height = 28;
            this.dgvReport3.Size = new System.Drawing.Size(260, 280);
            this.dgvReport3.TabIndex = 2;
            // 
            // btnGenerateReports
            // 
            this.btnGenerateReports.Location = new System.Drawing.Point(269, 12);
            this.btnGenerateReports.Name = "btnGenerateReports";
            this.btnGenerateReports.Size = new System.Drawing.Size(262, 79);
            this.btnGenerateReports.TabIndex = 3;
            this.btnGenerateReports.Text = "Generate Reports";
            this.btnGenerateReports.UseVisualStyleBackColor = true;
            this.btnGenerateReports.Click += new System.EventHandler(this.btnGenerateReports_Click);
            // 
            // numberAppointmentLbl
            // 
            this.numberAppointmentLbl.AutoSize = true;
            this.numberAppointmentLbl.Location = new System.Drawing.Point(-1, 124);
            this.numberAppointmentLbl.Name = "numberAppointmentLbl";
            this.numberAppointmentLbl.Size = new System.Drawing.Size(275, 20);
            this.numberAppointmentLbl.TabIndex = 4;
            this.numberAppointmentLbl.Text = "Number Appointment Types by Month";
            // 
            // userScheduleLbl
            // 
            this.userScheduleLbl.AutoSize = true;
            this.userScheduleLbl.Location = new System.Drawing.Point(319, 124);
            this.userScheduleLbl.Name = "userScheduleLbl";
            this.userScheduleLbl.Size = new System.Drawing.Size(173, 20);
            this.userScheduleLbl.TabIndex = 5;
            this.userScheduleLbl.Text = "Schedule for each user";
            // 
            // appointmentPerCustomerLbl
            // 
            this.appointmentPerCustomerLbl.AutoSize = true;
            this.appointmentPerCustomerLbl.Location = new System.Drawing.Point(550, 124);
            this.appointmentPerCustomerLbl.Name = "appointmentPerCustomerLbl";
            this.appointmentPerCustomerLbl.Size = new System.Drawing.Size(247, 20);
            this.appointmentPerCustomerLbl.TabIndex = 6;
            this.appointmentPerCustomerLbl.Text = "Total Appointments per Customer";
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.appointmentPerCustomerLbl);
            this.Controls.Add(this.userScheduleLbl);
            this.Controls.Add(this.numberAppointmentLbl);
            this.Controls.Add(this.btnGenerateReports);
            this.Controls.Add(this.dgvReport3);
            this.Controls.Add(this.dgvReport2);
            this.Controls.Add(this.dgvReport1);
            this.Name = "ReportsForm";
            this.Text = "ReportsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReport1;
        private System.Windows.Forms.DataGridView dgvReport2;
        private System.Windows.Forms.DataGridView dgvReport3;
        private System.Windows.Forms.Button btnGenerateReports;
        private System.Windows.Forms.Label numberAppointmentLbl;
        private System.Windows.Forms.Label userScheduleLbl;
        private System.Windows.Forms.Label appointmentPerCustomerLbl;
    }
}