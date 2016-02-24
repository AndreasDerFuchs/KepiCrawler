namespace MyKepiCrawler
{
    partial class KepiCrawler
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private bool m_DoExit = false;

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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KepiCrawler));
         this.button1 = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.textBox2 = new System.Windows.Forms.TextBox();
         this.richTextBox1 = new System.Windows.Forms.RichTextBox();
         this.buttonReset = new System.Windows.Forms.Button();
         this.timer_5min = new System.Windows.Forms.Timer(this.components);
         this.label_last_update = new System.Windows.Forms.Label();
         this.webBrowser1 = new System.Windows.Forms.WebBrowser();
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.textBox_eMail = new System.Windows.Forms.TextBox();
         this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
         this.NotifyIconMenu = new System.Windows.Forms.ContextMenu();
         this.menuItem1 = new System.Windows.Forms.MenuItem();
         this.menuItem2 = new System.Windows.Forms.MenuItem();
         this.timer_CntDwn = new System.Windows.Forms.Timer(this.components);
         this.button_Setup = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         this.SuspendLayout();
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(13, 13);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(75, 23);
         this.button1.TabIndex = 0;
         this.button1.Text = "Search Now";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.buttonSearchNow_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(91, 18);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(42, 13);
         this.label1.TabIndex = 2;
         this.label1.Text = "RegEx:";
         // 
         // textBox2
         // 
         this.textBox2.Location = new System.Drawing.Point(127, 15);
         this.textBox2.Name = "textBox2";
         this.textBox2.Size = new System.Drawing.Size(135, 20);
         this.textBox2.TabIndex = 3;
         this.textBox2.Text = "default string: see settings";
         // 
         // richTextBox1
         // 
         this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.richTextBox1.Location = new System.Drawing.Point(0, 0);
         this.richTextBox1.Name = "richTextBox1";
         this.richTextBox1.Size = new System.Drawing.Size(936, 102);
         this.richTextBox1.TabIndex = 4;
         this.richTextBox1.Text = "";
         // 
         // buttonReset
         // 
         this.buttonReset.Location = new System.Drawing.Point(350, 12);
         this.buttonReset.Name = "buttonReset";
         this.buttonReset.Size = new System.Drawing.Size(96, 23);
         this.buttonReset.TabIndex = 6;
         this.buttonReset.Text = "Cancel / Reset";
         this.buttonReset.UseVisualStyleBackColor = true;
         this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
         // 
         // timer_5min
         // 
         this.timer_5min.Enabled = true;
         this.timer_5min.Interval = 300000;
         this.timer_5min.Tick += new System.EventHandler(this.timer1_Tick);
         // 
         // label_last_update
         // 
         this.label_last_update.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label_last_update.AutoSize = true;
         this.label_last_update.Location = new System.Drawing.Point(804, 18);
         this.label_last_update.Name = "label_last_update";
         this.label_last_update.Size = new System.Drawing.Size(62, 13);
         this.label_last_update.TabIndex = 7;
         this.label_last_update.Text = "last_update";
         // 
         // webBrowser1
         // 
         this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.webBrowser1.Location = new System.Drawing.Point(0, 0);
         this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
         this.webBrowser1.Name = "webBrowser1";
         this.webBrowser1.Size = new System.Drawing.Size(936, 326);
         this.webBrowser1.TabIndex = 5;
         // 
         // splitContainer1
         // 
         this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.splitContainer1.Location = new System.Drawing.Point(12, 42);
         this.splitContainer1.Name = "splitContainer1";
         this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.richTextBox1);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
         this.splitContainer1.Size = new System.Drawing.Size(936, 432);
         this.splitContainer1.SplitterDistance = 102;
         this.splitContainer1.TabIndex = 8;
         // 
         // textBox_eMail
         // 
         this.textBox_eMail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.textBox_eMail.Location = new System.Drawing.Point(452, 15);
         this.textBox_eMail.Name = "textBox_eMail";
         this.textBox_eMail.Size = new System.Drawing.Size(346, 20);
         this.textBox_eMail.TabIndex = 9;
         this.textBox_eMail.Text = "default email addresses: see settings";
         // 
         // notifyIcon1
         // 
         this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
         this.notifyIcon1.BalloonTipText = "Kepi Crawler läuft noch ...";
         this.notifyIcon1.ContextMenu = this.NotifyIconMenu;
         this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
         this.notifyIcon1.Text = "Kepi Crawler";
         this.notifyIcon1.Visible = true;
         this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick_1);
         this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
         // 
         // NotifyIconMenu
         // 
         this.NotifyIconMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
         // 
         // menuItem1
         // 
         this.menuItem1.Index = 0;
         this.menuItem1.Text = "Exit";
         this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click_1);
         // 
         // menuItem2
         // 
         this.menuItem2.Index = 1;
         this.menuItem2.Text = "Setup";
         this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
         // 
         // timer_CntDwn
         // 
         this.timer_CntDwn.Interval = 1000;
         this.timer_CntDwn.Tick += new System.EventHandler(this.timer_CntDwn_Tick);
         // 
         // button_Setup
         // 
         this.button_Setup.Location = new System.Drawing.Point(268, 13);
         this.button_Setup.Name = "button_Setup";
         this.button_Setup.Size = new System.Drawing.Size(76, 23);
         this.button_Setup.TabIndex = 10;
         this.button_Setup.Text = "Setup";
         this.button_Setup.UseVisualStyleBackColor = true;
         this.button_Setup.Click += new System.EventHandler(this.button_Setup_Click);
         // 
         // KepiCrawler
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(960, 486);
         this.Controls.Add(this.button_Setup);
         this.Controls.Add(this.splitContainer1);
         this.Controls.Add(this.textBox_eMail);
         this.Controls.Add(this.label_last_update);
         this.Controls.Add(this.buttonReset);
         this.Controls.Add(this.textBox2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.button1);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "KepiCrawler";
         this.Text = "Kepi Crawler";
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
         this.splitContainer1.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Timer timer_5min;
        private System.Windows.Forms.Label label_last_update;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBox_eMail;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer timer_CntDwn;
        private System.Windows.Forms.ContextMenu NotifyIconMenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.Button button_Setup;
        private System.Windows.Forms.MenuItem menuItem2;
    }
}

