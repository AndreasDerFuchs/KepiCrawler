using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyKepiCrawler
{
   public partial class SetupLogin : Form
   {
      public SetupLogin()
      {
         InitializeComponent();
      }

      private void SetupLogin_Load(object sender, EventArgs e)
      {
         this.textBox_Username.Text = Properties.Settings.Default.MyUserName;
         this.textBox_Passwort.Text = "*********"; // Properties.Settings.Default.MyPassword;
         if (Properties.Settings.Default.UpdateMinutes < 60)
            this.comboBox_UpdateInterval.Text = String.Format("{0} Minuten", Properties.Settings.Default.UpdateMinutes);
         else
            this.comboBox_UpdateInterval.Text = String.Format("{0} Stunden", Properties.Settings.Default.UpdateMinutes / 60);

      }

      private void textBox_Username_Entered(object sender, EventArgs e)
      {
         Properties.Settings.Default.MyUserName = this.textBox_Username.Text;
         Properties.Settings.Default.Save();
      }

      private void textBox_Passwort_Entered(object sender, EventArgs e)
      {
         String pw = this.textBox_Passwort.Text;
         if (!pw.Contains("*"))
         {
            Properties.Settings.Default.MyPassword = pw;
            this.textBox_Passwort.Text = "*********";
            Properties.Settings.Default.Save();
         }
      }

      private void comboBox_UpdateInterval_SelectionChangeCommitted(object sender, EventArgs e)
      {
         String subjectString = this.comboBox_UpdateInterval.Text;
         //System.Text.RegularExpressions.Regex
         String resultString = Regex.Match(subjectString, @"\d+").Value;
         // returns a string containing the first occurrence of a number in subjectString.
         int n = (resultString.Length > 0) ? Int32.Parse(resultString) : 0;

         if (n > 0)
         {
            if (subjectString.Contains("Stunde"))
               Properties.Settings.Default.UpdateMinutes = n * 60;
            else if (subjectString.Contains("Minute"))
               Properties.Settings.Default.UpdateMinutes = n;
            else
               this.comboBox_UpdateInterval.Text = "ungültig";
            Properties.Settings.Default.Save();
         }
      }

      private void comboBox_UpdateInterval_TextUpdate(object sender, EventArgs e)
      {
         comboBox_UpdateInterval_SelectionChangeCommitted(sender, e);
      }

      private void comboBox_UpdateInterval_SelectedIndexChanged(object sender, EventArgs e)
      {
         comboBox_UpdateInterval_SelectionChangeCommitted(sender, e);
      }
   }
}
