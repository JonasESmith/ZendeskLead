namespace Task_Form
{
  partial class EmailMain
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
      this.tabController = new System.Windows.Forms.TabControl();
      this.email_Listing = new System.Windows.Forms.TabPage();
      this.label1 = new System.Windows.Forms.Label();
      this.script_runs = new System.Windows.Forms.Label();
      this.nextUpdateLabel = new System.Windows.Forms.Label();
      this.timer_Label = new System.Windows.Forms.Label();
      this.Sales_Reps = new System.Windows.Forms.TabPage();
      this.label2 = new System.Windows.Forms.Label();
      this.num_Emails_Sent = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.tabController.SuspendLayout();
      this.email_Listing.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabController
      // 
      this.tabController.Controls.Add(this.email_Listing);
      this.tabController.Controls.Add(this.Sales_Reps);
      this.tabController.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabController.Location = new System.Drawing.Point(0, 0);
      this.tabController.Name = "tabController";
      this.tabController.SelectedIndex = 0;
      this.tabController.Size = new System.Drawing.Size(648, 456);
      this.tabController.TabIndex = 0;
      // 
      // email_Listing
      // 
      this.email_Listing.Controls.Add(this.panel1);
      this.email_Listing.Location = new System.Drawing.Point(4, 22);
      this.email_Listing.Name = "email_Listing";
      this.email_Listing.Padding = new System.Windows.Forms.Padding(3);
      this.email_Listing.Size = new System.Drawing.Size(640, 430);
      this.email_Listing.TabIndex = 0;
      this.email_Listing.Text = "Mail List";
      this.email_Listing.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.BackColor = System.Drawing.Color.Transparent;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(2, 38);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(170, 24);
      this.label1.TabIndex = 3;
      this.label1.Text = "Num of script Calls ";
      // 
      // script_runs
      // 
      this.script_runs.AutoSize = true;
      this.script_runs.BackColor = System.Drawing.Color.Transparent;
      this.script_runs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.script_runs.Location = new System.Drawing.Point(255, 38);
      this.script_runs.Name = "script_runs";
      this.script_runs.Size = new System.Drawing.Size(50, 24);
      this.script_runs.TabIndex = 2;
      this.script_runs.Text = "0000";
      // 
      // nextUpdateLabel
      // 
      this.nextUpdateLabel.AutoSize = true;
      this.nextUpdateLabel.BackColor = System.Drawing.Color.Transparent;
      this.nextUpdateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.nextUpdateLabel.Location = new System.Drawing.Point(2, 8);
      this.nextUpdateLabel.Name = "nextUpdateLabel";
      this.nextUpdateLabel.Size = new System.Drawing.Size(201, 24);
      this.nextUpdateLabel.TabIndex = 1;
      this.nextUpdateLabel.Text = "Time until next update ";
      // 
      // timer_Label
      // 
      this.timer_Label.AutoSize = true;
      this.timer_Label.BackColor = System.Drawing.Color.Transparent;
      this.timer_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.timer_Label.Location = new System.Drawing.Point(255, 8);
      this.timer_Label.Name = "timer_Label";
      this.timer_Label.Size = new System.Drawing.Size(30, 24);
      this.timer_Label.TabIndex = 0;
      this.timer_Label.Text = "00";
      // 
      // Sales_Reps
      // 
      this.Sales_Reps.Location = new System.Drawing.Point(4, 22);
      this.Sales_Reps.Name = "Sales_Reps";
      this.Sales_Reps.Padding = new System.Windows.Forms.Padding(3);
      this.Sales_Reps.Size = new System.Drawing.Size(640, 430);
      this.Sales_Reps.TabIndex = 1;
      this.Sales_Reps.Text = "Sales Reps";
      this.Sales_Reps.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.BackColor = System.Drawing.Color.Transparent;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(3, 68);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(175, 24);
      this.label2.TabIndex = 5;
      this.label2.Text = "Num of emails sent ";
      // 
      // num_Emails_Sent
      // 
      this.num_Emails_Sent.AutoSize = true;
      this.num_Emails_Sent.BackColor = System.Drawing.Color.Transparent;
      this.num_Emails_Sent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.num_Emails_Sent.Location = new System.Drawing.Point(255, 68);
      this.num_Emails_Sent.Name = "num_Emails_Sent";
      this.num_Emails_Sent.Size = new System.Drawing.Size(40, 24);
      this.num_Emails_Sent.TabIndex = 4;
      this.num_Emails_Sent.Text = "000";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.BackColor = System.Drawing.Color.Transparent;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(3, 99);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(207, 24);
      this.label4.TabIndex = 7;
      this.label4.Text = "Total Time running (m) ";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.BackColor = System.Drawing.Color.Transparent;
      this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label5.Location = new System.Drawing.Point(255, 99);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(40, 24);
      this.label5.TabIndex = 6;
      this.label5.Text = "000";
      // 
      // panel1
      // 
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panel1.Controls.Add(this.label2);
      this.panel1.Controls.Add(this.label4);
      this.panel1.Controls.Add(this.timer_Label);
      this.panel1.Controls.Add(this.label5);
      this.panel1.Controls.Add(this.nextUpdateLabel);
      this.panel1.Controls.Add(this.script_runs);
      this.panel1.Controls.Add(this.num_Emails_Sent);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Location = new System.Drawing.Point(8, 6);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(319, 139);
      this.panel1.TabIndex = 8;
      // 
      // EmailMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(648, 456);
      this.Controls.Add(this.tabController);
      this.Name = "EmailMain";
      this.Text = "Sales-Rep Script";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EmailMain_FormClosing);
      this.tabController.ResumeLayout(false);
      this.email_Listing.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabController;
    private System.Windows.Forms.TabPage email_Listing;
    private System.Windows.Forms.TabPage Sales_Reps;
    private System.Windows.Forms.Label timer_Label;
    private System.Windows.Forms.Label nextUpdateLabel;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label script_runs;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label num_Emails_Sent;
  }
}

