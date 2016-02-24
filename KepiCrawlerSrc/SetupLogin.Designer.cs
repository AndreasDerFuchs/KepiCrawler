namespace MyKepiCrawler
{
   partial class SetupLogin
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupLogin));
         this.label_Username = new System.Windows.Forms.Label();
         this.textBox_Username = new System.Windows.Forms.TextBox();
         this.textBox_Passwort = new System.Windows.Forms.TextBox();
         this.label_Passwort = new System.Windows.Forms.Label();
         this.label_Interval = new System.Windows.Forms.Label();
         this.comboBox_UpdateInterval = new System.Windows.Forms.ComboBox();
         this.richTextBox1 = new System.Windows.Forms.RichTextBox();
         this.SuspendLayout();
         // 
         // label_Username
         // 
         this.label_Username.AutoSize = true;
         this.label_Username.Location = new System.Drawing.Point(13, 13);
         this.label_Username.Name = "label_Username";
         this.label_Username.Size = new System.Drawing.Size(55, 13);
         this.label_Username.TabIndex = 0;
         this.label_Username.Text = "Username";
         // 
         // textBox_Username
         // 
         this.textBox_Username.Location = new System.Drawing.Point(74, 10);
         this.textBox_Username.Name = "textBox_Username";
         this.textBox_Username.Size = new System.Drawing.Size(198, 20);
         this.textBox_Username.TabIndex = 1;
         this.textBox_Username.Validated += new System.EventHandler(this.textBox_Username_Entered);
         // 
         // textBox_Passwort
         // 
         this.textBox_Passwort.Location = new System.Drawing.Point(74, 36);
         this.textBox_Passwort.Name = "textBox_Passwort";
         this.textBox_Passwort.Size = new System.Drawing.Size(198, 20);
         this.textBox_Passwort.TabIndex = 3;
         this.textBox_Passwort.Validated += new System.EventHandler(this.textBox_Passwort_Entered);
         // 
         // label_Passwort
         // 
         this.label_Passwort.AutoSize = true;
         this.label_Passwort.Location = new System.Drawing.Point(13, 39);
         this.label_Passwort.Name = "label_Passwort";
         this.label_Passwort.Size = new System.Drawing.Size(50, 13);
         this.label_Passwort.TabIndex = 2;
         this.label_Passwort.Text = "Passwort";
         // 
         // label_Interval
         // 
         this.label_Interval.AutoSize = true;
         this.label_Interval.Location = new System.Drawing.Point(13, 65);
         this.label_Interval.Name = "label_Interval";
         this.label_Interval.Size = new System.Drawing.Size(80, 13);
         this.label_Interval.TabIndex = 4;
         this.label_Interval.Text = "Update-Interval";
         // 
         // comboBox_UpdateInterval
         // 
         this.comboBox_UpdateInterval.FormattingEnabled = true;
         this.comboBox_UpdateInterval.Items.AddRange(new object[] {
            "15 Minuten",
            "30 Minuten",
            "60 Minuten",
            "2 Stunden",
            "6 Stunden",
            "12 Stunden",
            "24 Stunden"});
         this.comboBox_UpdateInterval.Location = new System.Drawing.Point(99, 62);
         this.comboBox_UpdateInterval.Name = "comboBox_UpdateInterval";
         this.comboBox_UpdateInterval.Size = new System.Drawing.Size(173, 21);
         this.comboBox_UpdateInterval.TabIndex = 6;
         this.comboBox_UpdateInterval.SelectedIndexChanged += new System.EventHandler(this.comboBox_UpdateInterval_SelectedIndexChanged);
         this.comboBox_UpdateInterval.SelectionChangeCommitted += new System.EventHandler(this.comboBox_UpdateInterval_SelectionChangeCommitted);
         this.comboBox_UpdateInterval.TextUpdate += new System.EventHandler(this.comboBox_UpdateInterval_TextUpdate);
         // 
         // richTextBox1
         // 
         this.richTextBox1.Enabled = false;
         this.richTextBox1.Location = new System.Drawing.Point(16, 91);
         this.richTextBox1.Name = "richTextBox1";
         this.richTextBox1.ReadOnly = true;
         this.richTextBox1.Size = new System.Drawing.Size(256, 188);
         this.richTextBox1.TabIndex = 7;
         this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
         // 
         // SetupLogin
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(284, 291);
         this.Controls.Add(this.richTextBox1);
         this.Controls.Add(this.comboBox_UpdateInterval);
         this.Controls.Add(this.label_Interval);
         this.Controls.Add(this.textBox_Passwort);
         this.Controls.Add(this.label_Passwort);
         this.Controls.Add(this.textBox_Username);
         this.Controls.Add(this.label_Username);
         this.Name = "SetupLogin";
         this.Text = "SetupLogin";
         this.Load += new System.EventHandler(this.SetupLogin_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label_Username;
      private System.Windows.Forms.TextBox textBox_Username;
      private System.Windows.Forms.TextBox textBox_Passwort;
      private System.Windows.Forms.Label label_Passwort;
      private System.Windows.Forms.Label label_Interval;
      private System.Windows.Forms.ComboBox comboBox_UpdateInterval;
      private System.Windows.Forms.RichTextBox richTextBox1;

   }
}