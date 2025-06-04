
namespace C969
{
    partial class RecordsForm
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
            this.customerDataGrid = new System.Windows.Forms.DataGridView();
            this.addCustomerButton = new System.Windows.Forms.Button();
            this.updateCustomerButton = new System.Windows.Forms.Button();
            this.deleteCustomerButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.appointmentDataGrid = new System.Windows.Forms.DataGridView();
            this.appointmentAddButton = new System.Windows.Forms.Button();
            this.appointmentUpdateButton = new System.Windows.Forms.Button();
            this.appointmentDeleteButton = new System.Windows.Forms.Button();
            this.viewLogsButton = new System.Windows.Forms.Button();
            this.viewDateButton = new System.Windows.Forms.Button();
            this.viewAppointmentDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.showAllButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.customerDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // customerDataGrid
            // 
            this.customerDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customerDataGrid.Location = new System.Drawing.Point(12, 12);
            this.customerDataGrid.Name = "customerDataGrid";
            this.customerDataGrid.Size = new System.Drawing.Size(763, 675);
            this.customerDataGrid.TabIndex = 0;
            // 
            // addCustomerButton
            // 
            this.addCustomerButton.Location = new System.Drawing.Point(9, 751);
            this.addCustomerButton.Name = "addCustomerButton";
            this.addCustomerButton.Size = new System.Drawing.Size(75, 23);
            this.addCustomerButton.TabIndex = 1;
            this.addCustomerButton.Text = "Add";
            this.addCustomerButton.UseVisualStyleBackColor = true;
            this.addCustomerButton.Click += new System.EventHandler(this.addCustomerButton_Click);
            // 
            // updateCustomerButton
            // 
            this.updateCustomerButton.Location = new System.Drawing.Point(90, 751);
            this.updateCustomerButton.Name = "updateCustomerButton";
            this.updateCustomerButton.Size = new System.Drawing.Size(75, 23);
            this.updateCustomerButton.TabIndex = 2;
            this.updateCustomerButton.Text = "Update";
            this.updateCustomerButton.UseVisualStyleBackColor = true;
            this.updateCustomerButton.Click += new System.EventHandler(this.updateCustomerButton_Click);
            // 
            // deleteCustomerButton
            // 
            this.deleteCustomerButton.Location = new System.Drawing.Point(171, 751);
            this.deleteCustomerButton.Name = "deleteCustomerButton";
            this.deleteCustomerButton.Size = new System.Drawing.Size(75, 23);
            this.deleteCustomerButton.TabIndex = 3;
            this.deleteCustomerButton.Text = "Delete";
            this.deleteCustomerButton.UseVisualStyleBackColor = true;
            this.deleteCustomerButton.Click += new System.EventHandler(this.deleteCustomerButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(1468, 754);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 4;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 723);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Customer Section";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(778, 723);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Appointment Section";
            // 
            // appointmentDataGrid
            // 
            this.appointmentDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.appointmentDataGrid.Location = new System.Drawing.Point(781, 12);
            this.appointmentDataGrid.Name = "appointmentDataGrid";
            this.appointmentDataGrid.Size = new System.Drawing.Size(762, 675);
            this.appointmentDataGrid.TabIndex = 7;
            // 
            // appointmentAddButton
            // 
            this.appointmentAddButton.Location = new System.Drawing.Point(781, 751);
            this.appointmentAddButton.Name = "appointmentAddButton";
            this.appointmentAddButton.Size = new System.Drawing.Size(75, 23);
            this.appointmentAddButton.TabIndex = 8;
            this.appointmentAddButton.Text = "Add";
            this.appointmentAddButton.UseVisualStyleBackColor = true;
            this.appointmentAddButton.Click += new System.EventHandler(this.appointmentAddButton_Click);
            // 
            // appointmentUpdateButton
            // 
            this.appointmentUpdateButton.Location = new System.Drawing.Point(862, 751);
            this.appointmentUpdateButton.Name = "appointmentUpdateButton";
            this.appointmentUpdateButton.Size = new System.Drawing.Size(75, 23);
            this.appointmentUpdateButton.TabIndex = 9;
            this.appointmentUpdateButton.Text = "Update";
            this.appointmentUpdateButton.UseVisualStyleBackColor = true;
            this.appointmentUpdateButton.Click += new System.EventHandler(this.appointmentUpdateButton_Click);
            // 
            // appointmentDeleteButton
            // 
            this.appointmentDeleteButton.Location = new System.Drawing.Point(943, 751);
            this.appointmentDeleteButton.Name = "appointmentDeleteButton";
            this.appointmentDeleteButton.Size = new System.Drawing.Size(75, 23);
            this.appointmentDeleteButton.TabIndex = 10;
            this.appointmentDeleteButton.Text = "Delete";
            this.appointmentDeleteButton.UseVisualStyleBackColor = true;
            this.appointmentDeleteButton.Click += new System.EventHandler(this.appointmentDeleteButton_Click);
            // 
            // viewLogsButton
            // 
            this.viewLogsButton.Location = new System.Drawing.Point(400, 754);
            this.viewLogsButton.Name = "viewLogsButton";
            this.viewLogsButton.Size = new System.Drawing.Size(75, 23);
            this.viewLogsButton.TabIndex = 11;
            this.viewLogsButton.Text = "View Logs";
            this.viewLogsButton.UseVisualStyleBackColor = true;
            this.viewLogsButton.Click += new System.EventHandler(this.viewLogsButton_Click);
            // 
            // viewDateButton
            // 
            this.viewDateButton.Location = new System.Drawing.Point(1201, 751);
            this.viewDateButton.Name = "viewDateButton";
            this.viewDateButton.Size = new System.Drawing.Size(75, 23);
            this.viewDateButton.TabIndex = 12;
            this.viewDateButton.Text = "Search";
            this.viewDateButton.UseVisualStyleBackColor = true;
            this.viewDateButton.Click += new System.EventHandler(this.viewDateButton_Click);
            // 
            // viewAppointmentDate
            // 
            this.viewAppointmentDate.Location = new System.Drawing.Point(1201, 723);
            this.viewAppointmentDate.Name = "viewAppointmentDate";
            this.viewAppointmentDate.Size = new System.Drawing.Size(200, 20);
            this.viewAppointmentDate.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1198, 701);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "View appointments by date";
            // 
            // showAllButton
            // 
            this.showAllButton.Location = new System.Drawing.Point(1326, 751);
            this.showAllButton.Name = "showAllButton";
            this.showAllButton.Size = new System.Drawing.Size(75, 23);
            this.showAllButton.TabIndex = 15;
            this.showAllButton.Text = "Show All";
            this.showAllButton.UseVisualStyleBackColor = true;
            this.showAllButton.Click += new System.EventHandler(this.showAllButton_Click);
            // 
            // RecordsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1555, 789);
            this.Controls.Add(this.showAllButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.viewAppointmentDate);
            this.Controls.Add(this.viewDateButton);
            this.Controls.Add(this.viewLogsButton);
            this.Controls.Add(this.appointmentDeleteButton);
            this.Controls.Add(this.appointmentUpdateButton);
            this.Controls.Add(this.appointmentAddButton);
            this.Controls.Add(this.appointmentDataGrid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.deleteCustomerButton);
            this.Controls.Add(this.updateCustomerButton);
            this.Controls.Add(this.addCustomerButton);
            this.Controls.Add(this.customerDataGrid);
            this.Name = "RecordsForm";
            this.Text = "Customer Records";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EndConnectionOnClose);
            ((System.ComponentModel.ISupportInitialize)(this.customerDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView customerDataGrid;
        private System.Windows.Forms.Button addCustomerButton;
        private System.Windows.Forms.Button updateCustomerButton;
        private System.Windows.Forms.Button deleteCustomerButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView appointmentDataGrid;
        private System.Windows.Forms.Button appointmentAddButton;
        private System.Windows.Forms.Button appointmentUpdateButton;
        private System.Windows.Forms.Button appointmentDeleteButton;
        private System.Windows.Forms.Button viewLogsButton;
        private System.Windows.Forms.Button viewDateButton;
        private System.Windows.Forms.DateTimePicker viewAppointmentDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button showAllButton;
    }
}