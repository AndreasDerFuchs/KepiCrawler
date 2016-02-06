using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MyKepiCrawler
{

    public partial class KepiCrawler : Form
    {
        private string tmpFile = "C:\\Temp\\ask_kepi_plan_auszug.txt";
        private string subContent = "";
        private int cntDown = 0;

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public string RemoveBrackets(string s)
        {
            int i, j;
            for (; ; )
            {
                i = s.IndexOf('<');
                j = s.IndexOf('>');

                if ((i < 0) || (j <= i))
                    break;
                s = s.Remove(i) + "\t" + s.Remove(0, j + 1);
            }
            return s;
        }

        public KepiCrawler()
        {
            string path_dir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "KepiCrawler");
            string path_full = System.IO.Path.Combine(path_dir, Path.GetFileName(tmpFile));
            if (!Directory.Exists(path_dir))
            {
                Directory.CreateDirectory(path_dir); //Needs to be created
            }
            if (Directory.Exists(Path.GetDirectoryName(tmpFile)))
                File.WriteAllText(tmpFile, "see file: " + path_full);
            tmpFile = path_full;

            InitializeComponent();
            FormClosing += KepiCrawler_FormClosing;
            this.textBox2.Text = Properties.Settings.Default.MySearchString;
            this.textBox_eMail.Text = Properties.Settings.Default.MySendEmail;
            this.Resize += frmMain_Resize;
            button1_Click(this, null);
        }

        private static int WM_QUERYENDSESSION = 0x11;
        private static bool systemShutdown = false;
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_QUERYENDSESSION)
            {
                // MessageBox.Show("KepiCrawler: this is a logoff, shutdown, or reboot");
                systemShutdown = true;
                Application.Exit();
            }

            // If this is WM_QUERYENDSESSION, the closing event should be
            // raised in the base WndProc.
            base.WndProc(ref m);

        } //WndProc 


        private void KepiCrawler_FormClosing(object sender, FormClosingEventArgs e)
        {
#if FALSE
            var window = MessageBox.Show("Willst Du den Kepi Crawler wirklich beenden?\nBei 'Ja' läuft nichts mehr,\nbei 'Nein' wird er nur minimiert.", "Kepi Crawler beenden?",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2);
            if (window == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else if (window == DialogResult.No)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
            else
                e.Cancel = false;
#else
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
            if (m_DoExit || systemShutdown)
                e.Cancel = false;
#endif
        }
        

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.notifyIcon1.Visible = true;
                this.notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
#if FALSE
            else if (FormWindowState.Normal == this.WindowState)
            {
                this.notifyIcon1.Visible = false;
            }
#endif
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "http://212.71.198.9/vp_home/VP_Web.php";
            string postData = "user=kepler&passwort=1571-1630&submit=Anmelden";
            string pageContent = "";

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            myHttpWebRequest.Method = "POST";

            byte[] data = Encoding.ASCII.GetBytes(postData);

            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = data.Length;

            try
            {
                using (Stream requestStream = myHttpWebRequest.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();
                }
            }
            catch (System.Net.WebException ex)
            {
                this.richTextBox1.Text = "GetRequestStream failed: " + ex.Message;
                Thread.Sleep(1000);
                return;
            }

            try
            {
                using (HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse())
                using (Stream responseStream = myHttpWebResponse.GetResponseStream())
                using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default))
                {
                    pageContent = myStreamReader.ReadToEnd();
                    string searchString = this.textBox2.Text;

                    subContent = "";
                    using (Stream s = GenerateStreamFromString(pageContent))
                    using (StreamReader myStreamReader2 = new StreamReader(s, Encoding.Default))
                    {
                        string derTag = "";

                        // Define a case-insensitive regular expression for repeated words.
                        // for usage see http://msdn.microsoft.com/en-us/library/az24scfc.aspx
                        Regex rxInsensitive = new Regex(searchString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                        while (!myStreamReader2.EndOfStream)
                        {
                            string ln = myStreamReader2.ReadLine();
                            if (ln.Contains("mon_title"))
                                derTag = RemoveBrackets(ln) + '\n';
                            // Find matches.
                            MatchCollection matches = rxInsensitive.Matches(ln);
                            if (matches.Count > 0)
                            {
                                ln = ln.Replace("&nbsp;", " "); // non-breaking space
                                subContent += derTag + RemoveBrackets(ln) + '\n';
                                derTag = "";
                            }
                        }
                        myStreamReader2.Close();
                    }

                    myStreamReader.Close();
                    responseStream.Close();
                    myHttpWebResponse.Close();
                }
            }
            catch (System.Net.ProtocolViolationException ex)
            {
                this.richTextBox1.Text = "GetResponse failed: " + ex.Message;
                Thread.Sleep(1000);
                return;
            }

            this.webBrowser1.DocumentText = pageContent;
            this.richTextBox1.Text = subContent;

            string oldSubContent = "";
            try
            {
                oldSubContent = File.ReadAllText(tmpFile);
            }
            catch (FileNotFoundException ex)
            {
                oldSubContent = ex.Message;
            }

            if (oldSubContent != subContent)
            {
                cntDown = 60;
                this.timer_CntDwn.Enabled = true;
            }

            label_last_update.Text = System.DateTime.Now.ToString();
            this.notifyIcon1.Text = "Kepi Crawler, last update at " + label_last_update.Text;

            //return pageContent;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.Delete(tmpFile);
            this.timer_5min.Stop();
            this.timer_5min.Interval = 120000; // = 2 minutes
            this.timer_5min.Start();
            this.timer_CntDwn.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void timer_CntDwn_Tick(object sender, EventArgs e)
        {
            if ((cntDown < 55) && (subContent.Length == 0))
            {
                this.richTextBox1.Text = String.Format("No data to send at {0} (cancelled)", DateTime.Now.ToString());
                this.timer_CntDwn.Enabled = false;
                File.WriteAllText(tmpFile, subContent); // avoid a re-trigger in 5 minutes.
            }
            else if (cntDown > 0)
            {
                this.richTextBox1.Text = String.Format("Sending in {0} seconds:\n{1}", cntDown , subContent);
                cntDown--;
                switch (cntDown)
                {
                    case 59:
                    case 54:
                    case 44:
                        this.Show();
                        this.WindowState = FormWindowState.Normal;
                        this.TopMost = true;
                        this.CenterToScreen();
                        this.TopMost = false;
                        break;
                }
            }
            else
            {
                this.richTextBox1.Text = String.Format("Sent at {0}:\n{1}", DateTime.Now.ToString(), subContent);
                string emailTo = this.textBox_eMail.Text;
                string originalValue = MyKepiCrawler.Properties.Settings.Default.Properties["MySendEmail"].DefaultValue as string;
                if (emailTo == originalValue)
                    emailTo = "";
                try
                {
                    File.WriteAllText(tmpFile, subContent);

                    if (emailTo.Length > 3)
                    {
                        string bodyEmail = subContent;

                        Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
                        Microsoft.Office.Interop.Outlook.MailItem eMail = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);

                        eMail.Subject = "Kepi Update V1";
                        eMail.To = emailTo;
                        eMail.Body = bodyEmail;
                        ((Microsoft.Office.Interop.Outlook._MailItem)eMail).Send();
                    }
                }
                catch (Exception ex)
                {
                    string errMsg = String.Format("Error sending 'Kepi Update' Email: {0} at {1}:\n{2}", ex.Message, DateTime.Now.ToString(), subContent);
                    this.richTextBox1.Text = errMsg;
                    File.WriteAllText(tmpFile, errMsg);
                }
                finally
                {
                    this.timer_CntDwn.Enabled = false;
                }
            }
            Properties.Settings.Default.MySearchString = this.textBox2.Text;
            Properties.Settings.Default.MySendEmail = this.textBox_eMail.Text;
            Properties.Settings.Default.Save();
        }
    }
}
