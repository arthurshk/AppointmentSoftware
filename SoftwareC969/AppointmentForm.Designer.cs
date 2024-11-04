namespace SoftwareC969
{
    partial class AppointmentForm
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
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.customerComboBox = new System.Windows.Forms.Label();
            this.txtType = new System.Windows.Forms.TextBox();
            this.typeLbl = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.datetimestartLbl = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.datetimeendLbl = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.btnViewCalendar = new System.Windows.Forms.Button();
            this.cmbTimeZone = new System.Windows.Forms.ComboBox();
            this.btnViewReports2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.Location = new System.Drawing.Point(267, 31);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(121, 28);
            this.cmbCustomer.TabIndex = 0;
            // 
            // customerComboBox
            // 
            this.customerComboBox.AutoSize = true;
            this.customerComboBox.Location = new System.Drawing.Point(120, 34);
            this.customerComboBox.Name = "customerComboBox";
            this.customerComboBox.Size = new System.Drawing.Size(78, 20);
            this.customerComboBox.TabIndex = 1;
            this.customerComboBox.Text = "Customer";
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(267, 70);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(121, 26);
            this.txtType.TabIndex = 2;
            // 
            // typeLbl
            // 
            this.typeLbl.AutoSize = true;
            this.typeLbl.Location = new System.Drawing.Point(124, 70);
            this.typeLbl.Name = "typeLbl";
            this.typeLbl.Size = new System.Drawing.Size(43, 20);
            this.typeLbl.TabIndex = 3;
            this.typeLbl.Text = "Type";
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(267, 113);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(382, 26);
            this.dtpStart.TabIndex = 4;
            // 
            // datetimestartLbl
            // 
            this.datetimestartLbl.AutoSize = true;
            this.datetimestartLbl.Location = new System.Drawing.Point(124, 113);
            this.datetimestartLbl.Name = "datetimestartLbl";
            this.datetimestartLbl.Size = new System.Drawing.Size(118, 20);
            this.datetimestartLbl.TabIndex = 5;
            this.datetimestartLbl.Text = "Date/Time start";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(267, 154);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(382, 26);
            this.dtpEnd.TabIndex = 6;
            // 
            // datetimeendLbl
            // 
            this.datetimeendLbl.AutoSize = true;
            this.datetimeendLbl.Location = new System.Drawing.Point(124, 154);
            this.datetimeendLbl.Name = "datetimeendLbl";
            this.datetimeendLbl.Size = new System.Drawing.Size(113, 20);
            this.datetimeendLbl.TabIndex = 7;
            this.datetimeendLbl.Text = "Date/Time end";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(128, 195);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(130, 49);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(327, 195);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(134, 49);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(526, 195);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(123, 49);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dgvAppointments
            // 
            this.dgvAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppointments.Location = new System.Drawing.Point(124, 250);
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.RowHeadersWidth = 62;
            this.dgvAppointments.RowTemplate.Height = 28;
            this.dgvAppointments.Size = new System.Drawing.Size(525, 188);
            this.dgvAppointments.TabIndex = 11;
            this.dgvAppointments.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAppointments_CellContentClick);
            // 
            // btnViewCalendar
            // 
            this.btnViewCalendar.Location = new System.Drawing.Point(478, 31);
            this.btnViewCalendar.Name = "btnViewCalendar";
            this.btnViewCalendar.Size = new System.Drawing.Size(171, 65);
            this.btnViewCalendar.TabIndex = 12;
            this.btnViewCalendar.Text = "View Calendar";
            this.btnViewCalendar.UseVisualStyleBackColor = true;
            this.btnViewCalendar.Click += new System.EventHandler(this.btnViewCalendar_Click);
            // 
            // cmbTimeZone
            // 
            this.cmbTimeZone.FormattingEnabled = true;
            this.cmbTimeZone.Location = new System.Drawing.Point(667, 62);
            this.cmbTimeZone.Name = "cmbTimeZone";
            this.cmbTimeZone.Size = new System.Drawing.Size(121, 28);
            this.cmbTimeZone.TabIndex = 13;
            // 
            // btnViewReports2
            // 
            this.btnViewReports2.Location = new System.Drawing.Point(683, 356);
            this.btnViewReports2.Name = "btnViewReports2";
            this.btnViewReports2.Size = new System.Drawing.Size(75, 65);
            this.btnViewReports2.TabIndex = 14;
            this.btnViewReports2.Text = "View Reports";
            this.btnViewReports2.UseVisualStyleBackColor = true;
            this.btnViewReports2.Click += new System.EventHandler(this.btnViewReports2_Click);
            // 
            // AppointmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnViewReports2);
            this.Controls.Add(this.cmbTimeZone);
            this.Controls.Add(this.btnViewCalendar);
            this.Controls.Add(this.dgvAppointments);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.datetimeendLbl);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.datetimestartLbl);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.typeLbl);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.customerComboBox);
            this.Controls.Add(this.cmbCustomer);
            this.Name = "AppointmentForm";
            this.Text = "AppointmentForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Label customerComboBox;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Label typeLbl;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label datetimestartLbl;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label datetimeendLbl;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dgvAppointments;
        private System.Windows.Forms.Button btnViewCalendar;
        private System.Windows.Forms.ComboBox cmbTimeZone;
        private System.Windows.Forms.Button btnViewReports2;
    }
}