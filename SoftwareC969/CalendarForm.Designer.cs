namespace SoftwareC969
{
    partial class CalendarForm
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
            this.monthCalendar = new System.Windows.Forms.MonthCalendar();
            this.dgvDailyAppointments = new System.Windows.Forms.DataGridView();
            this.selectedDateLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // monthCalendar
            // 
            this.monthCalendar.Location = new System.Drawing.Point(480, 179);
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 0;
            this.monthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar_DateChanged);
            // 
            // dgvDailyAppointments
            // 
            this.dgvDailyAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDailyAppointments.Location = new System.Drawing.Point(24, 47);
            this.dgvDailyAppointments.Name = "dgvDailyAppointments";
            this.dgvDailyAppointments.RowHeadersWidth = 62;
            this.dgvDailyAppointments.RowTemplate.Height = 28;
            this.dgvDailyAppointments.Size = new System.Drawing.Size(453, 385);
            this.dgvDailyAppointments.TabIndex = 1;
            // 
            // selectedDateLbl
            // 
            this.selectedDateLbl.AutoSize = true;
            this.selectedDateLbl.Location = new System.Drawing.Point(501, 150);
            this.selectedDateLbl.Name = "selectedDateLbl";
            this.selectedDateLbl.Size = new System.Drawing.Size(115, 20);
            this.selectedDateLbl.TabIndex = 2;
            this.selectedDateLbl.Text = "Selected Date:";
            // 
            // CalendarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.selectedDateLbl);
            this.Controls.Add(this.dgvDailyAppointments);
            this.Controls.Add(this.monthCalendar);
            this.Name = "CalendarForm";
            this.Text = "CalendarForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar monthCalendar;
        private System.Windows.Forms.DataGridView dgvDailyAppointments;
        private System.Windows.Forms.Label selectedDateLbl;
    }
}