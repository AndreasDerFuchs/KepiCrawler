using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KepiCrawler
{
   public class DictionaryWithDefault<TKey, TValue> : Dictionary<TKey, TValue>
   {
      TValue _default;
      public TValue DefaultValue
      {
         get { return _default; }
         set { _default = value; }
      }
      public DictionaryWithDefault() : base() { }
      public DictionaryWithDefault(TValue defaultValue)
         : base()
      {
         _default = defaultValue;
      }
      public new TValue this[TKey key]
      {
         get
         {
            TValue t;
            return base.TryGetValue(key, out t) ? t : _default;
         }
         set { base[key] = value; }
      }
   }

   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      UInt64 loop_cnt = 0;
      string log_path, stored_s = "nix";
      enum MyState { INIT, STUNDENPLAN, KLASSEN, WOCHE, UPDATE };
      MyState my_state = MyState.INIT;
      string m_caret = "supercalifragilisticexpialidocious";
      DictionaryWithDefault<string, string> dictionary = new DictionaryWithDefault<string, string>("please-check-command-line-ags");

      Timer mytimer;
      double stored_interval = 0;
      const double dt_fast = 500;
      const double dt_threshold = 1000;
      const double dt_slow = dt_threshold + 500;
      const double dt_12min = 1000 * 60 * 12;
      string id_string = DateTime.Now.ToString("G");

      public MainWindow()
      {
         try
         {
            string[] args = Environment.GetCommandLineArgs();
            for (int index = 1; index < (args.Length - 1); index += 2)
            {
               dictionary.Add(args[index], args[index + 1]);
            }

            // see http://www.wpf-tutorial.com/misc-controls/the-webbrowser-control/
            InitializeComponent();

            string title = dictionary["Title"];
            this.MyWindow.Title = title;
            SetSizeAndPosition();
            SetAndDeleteLogDir();
            webBrowser.Navigated += (a, b) => { HideScriptErrors(webBrowser, true); };
            webBrowser.Navigated += (a, b) => { SpeedUpTimer(); };
            webBrowser.Navigate("https://tipo.webuntis.com/WebUntis");

            mytimer = new Timer(dt_fast); // calls set mytimer.Interval;
            mytimer.Elapsed += mytimer_Elapsed;
            mytimer.Start();

            MyWindow.Closing += MyWindow_Closing;
         }
         catch (IOException ex)
         {
            // Log it
            Console.WriteLine("Exception: {0}", ex.Message);
            throw;
         }
         catch (Exception ex)
         {
            // Log it
            Console.WriteLine("Exception: {0}", ex.Message);
            throw new Exception("My MainWindow", ex);
         }
         finally
         {
            // Normal clean goes here (like closing open files).
         }
      }

      public void SpeedUpTimer()
      {
         double dt = mytimer.Interval;

         if (dt < dt_threshold) // mytimer.Interval;
         {
            if (dt > 0.1)
               stored_interval = dt;
            mytimer.Stop();
            mytimer.AutoReset = false;
            mytimer.Interval = 0.1;
            mytimer.Start();
         }
      }

      public void HideScriptErrors(WebBrowser wb, bool hide)
      {
         var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
         if (fiComWebBrowser == null) return;
         var objComWebBrowser = fiComWebBrowser.GetValue(wb);
         if (objComWebBrowser == null)
         {
            wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
            return;
         }
         objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
      }

      // Delegates to enable async calls for setting controls properties
      private delegate void PushButtonCallback(System.Windows.Controls.Button control, RoutedEventArgs e);

      void mytimer_Elapsed(object sender, ElapsedEventArgs e)
      {
         if (stored_interval > 0.1)
            mytimer.Interval = stored_interval;
         mytimer.AutoReset = true;


         //System.Console.WriteLine(">>>>>>>>>>>>>>>>>> Timer start >>>>>>>>>>>>>>>>>>>");
         if (this.MyButtonUpdate.Dispatcher.CheckAccess()) // instead of InvokeRequired
            Button_Click(null, null);
         else
         {
            PushButtonCallback d = new PushButtonCallback(Button_Click);
            this.MyButtonUpdate.Dispatcher.Invoke(d, new object[] { this.MyButtonUpdate, null });
         }
         //System.Console.WriteLine("<<<<<<<<<<<<<<<<<< Timer end <<<<<<<<<<<<<<<<<<<<<");
      }
      public void SetSizeAndPosition()
      {
         string pos = dictionary["-geometry"]; // e.g. -geometry 720x692+20+50
         if (pos != dictionary.DefaultValue)
         {
            int char_x_pos = pos.IndexOf('x');
            int char_plus_pos = pos.IndexOf('+');
            int char_minus_pos = pos.IndexOf('-');
            if ((char_minus_pos >= 0) && ((char_minus_pos < char_plus_pos) || (char_plus_pos < 0)))
               char_plus_pos = char_minus_pos;
            int height_str_end = char_plus_pos > 0 ? (char_plus_pos - 1) : (pos.Length - 1);
            if (char_x_pos >= 0)
            {
               if (char_x_pos > 0)
               {
                  string width_str = pos.Substring(0, char_x_pos);
                  this.MyWindow.Width = Convert.ToDouble(width_str);
               }
               if (height_str_end > char_x_pos)
               {
                  string height_str = pos.Substring(char_x_pos + 1, height_str_end - char_x_pos);
                  this.MyWindow.Height = Convert.ToDouble(height_str);
               }
            }
            if (char_plus_pos >= 0)
            {
               int char_plus2_pos = pos.IndexOf('+', char_plus_pos + 1);
               int char_minus2_pos = pos.IndexOf('-', char_plus_pos + 1);
               if ((char_minus2_pos >= 0) && ((char_minus2_pos < char_plus2_pos) || (char_plus2_pos < 0)))
                  char_plus2_pos = char_minus2_pos;
               int x_str_end = char_plus2_pos > 0 ? (char_plus2_pos - 1) : (pos.Length - 1);
               if (x_str_end > char_plus_pos)
               {
                  string x_str = pos.Substring(char_plus_pos, 1 + x_str_end - char_plus_pos);
                  this.MyWindow.Left = Convert.ToDouble(x_str);
               }
               int y_str_end = (char_plus2_pos < 0) ? char_plus2_pos : (pos.Length - 1);
               if (y_str_end > char_plus2_pos)
               {
                  string y_str = pos.Substring(char_plus2_pos, 1 + y_str_end - char_plus2_pos);
                  this.MyWindow.Top = Convert.ToDouble(y_str);
               }
            }
         }
      }

      private void SetAndDeleteLogDir()
      {
         string user = dictionary["User"];
         log_path = String.Format("{0}\\Log-{1}", Directory.GetCurrentDirectory(), user);
         System.IO.Directory.CreateDirectory(log_path);

         System.IO.DirectoryInfo di = new DirectoryInfo(log_path);

         foreach (FileInfo file in di.GetFiles())
         {
            try
            {
               if (file.Name[0] == 'l')
                  file.Delete();
            }
            catch (Exception e)
            {
               System.Console.WriteLine("Could not delete file {0}: {1}", file.Name, e.Message);
            }
         }
         // foreach (DirectoryInfo dir in di.GetDirectories())
         // {
         //    dir.Delete(true);
         // }
         CreateIdFile();
      }
      void CreateIdFile()
      {
         string fname = String.Format("{0}\\KepiCrawl2-ID.txt", log_path);
         TextWriter tw = new StreamWriter(fname); tw.WriteLine(id_string); tw.Close();
      }
      void CheckIdFile()
      {
         string fname = String.Format("{0}\\KepiCrawl2-ID.txt", log_path);
         string line;
         try
         {
            TextReader tr = new StreamReader(fname);
            line = tr.ReadLine();
            tr.Close();
         }
         catch
         {
            line = "file does not exist";
         }
         if (!line.Equals(id_string))
         {
            id_string = "closing";
            Application.Current.Shutdown();
         }
      }
      void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         if (id_string != "closing")
         {
            MyWindow.WindowState = WindowState.Minimized;
            e.Cancel = true;
         }
      }

      private void Button_Click(object sender, RoutedEventArgs e)
      {
         CheckIdFile();
         System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("de-DE");
         mshtml.HTMLDocument doc = this.webBrowser.Document as mshtml.HTMLDocument;
         int m = 0;
         string user = dictionary["User"];    // The Command line arguments may be e.g.
         string password = dictionary["Pw"];  // Schule "kepi tuebingen" User 5b Pw Schnabel Title 5er, or
         string schule = dictionary["Schule"];// Schule "kepi tuebingen" User 8d Pw Lechner Title 5er
         string d = dictionary.DefaultValue;
         if (user == d || password == d || schule == d)
         {
            mytimer.Interval = dt_slow;
            /* Beschreibung der Kommandozeilenparameter Schule, User, Pw und Title: */
            Console.WriteLine("Syntax: {0} Schule <schulname> User <username> Pw <password> Title <egal-was>", System.AppDomain.CurrentDomain.FriendlyName);
            Console.WriteLine("e.g.    {0} Schule \"kepi tuebingen\" User 5b Pw Schnabel Title 5er", System.AppDomain.CurrentDomain.FriendlyName);
            if (loop_cnt >= 3)
            {
               System.Environment.Exit(1);
            }
         }


         loop_cnt++;

         if (doc == null)
         {
            if (loop_cnt < 10)
               Console.WriteLine("doc == null");
            return;
         }

         mshtml.HTMLInputElement el_school = doc.getElementById("school") as mshtml.HTMLInputElement;
         if (el_school != null) { ++m; el_school.setAttribute("value", schule); }

         mshtml.HTMLInputElement el_schooln = doc.getElementById("loginWidget.idschoolname") as mshtml.HTMLInputElement;
         if (el_schooln != null) { ++m; el_schooln.setAttribute("value", schule); }

         mshtml.HTMLInputElement el_usernam = doc.getElementById("loginWidget.idusername") as mshtml.HTMLInputElement;
         if (el_usernam != null) { ++m; el_usernam.setAttribute("value", user); }

         mshtml.HTMLInputElement el_passwrd = doc.getElementById("loginWidget.idpassword") as mshtml.HTMLInputElement;
         if (el_passwrd != null) { ++m; el_passwrd.setAttribute("value", password); }

         // HtmlElementCollection 
         mshtml.IHTMLElementCollection elc = doc.getElementsByTagName("input");
         foreach (var x in elc)
         {
            mshtml.HTMLInputElement xx = x as mshtml.HTMLInputElement;
            if ((xx != null) && xx.getAttribute("type").Equals("submit"))
            {
               xx.select();
               xx.click();
            }
         }
         // return

         // mshtml.IHTMLDocument2 doc2 = webBrowser.Document as mshtml.IHTMLDocument2;
         mshtml.HTMLDocument doc2 = doc;
         mshtml.IHTMLElementCollection elc2 = doc2.all;
         bool check = false;
         string fname = null;
         int i = 0;
         MyState my_next_state = my_state;
         foreach (var x in elc2)
         {
            i = i + 1;
            mshtml.HTMLInputElement xx1 = x as mshtml.HTMLInputElement;
            mshtml.HTMLHtmlElement xx2 = x as mshtml.HTMLHtmlElement;
            mshtml.HTMLLinkElement xx3 = x as mshtml.HTMLLinkElement;
            mshtml.HTMLBody xx4 = x as mshtml.HTMLBody;
            mshtml.HTMLFormElement xx5 = x as mshtml.HTMLFormElement;
            mshtml.HTMLScriptElement xx6 = x as mshtml.HTMLScriptElement;
            mshtml.HTMLImg xx7 = x as mshtml.HTMLImg;
            mshtml.HTMLAnchorElement xx8 = x as mshtml.HTMLAnchorElement;
            mshtml.HTMLButtonElement xx9 = x as mshtml.HTMLButtonElement;
            mshtml.HTMLTableRow x10 = x as mshtml.HTMLTableRow;
            mshtml.HTMLTable x11 = x as mshtml.HTMLTable;
            mshtml.HTMLStyleElement x12 = x as mshtml.HTMLStyleElement;
            mshtml.HTMLDivElement x13 = x as mshtml.HTMLDivElement;
            mshtml.HTMLSpanElement x14 = x as mshtml.HTMLSpanElement;
            mshtml.HTMLTableCell x15 = x as mshtml.HTMLTableCell;

            string s0 = x.GetType().ToString();
            string s = s0;
            int t = 0;

            if (xx2 != null) { t = 2; s = xx2.outerHTML; }
            if (xx4 != null) { t = 4; s = xx4.outerHTML; }
            if (x13 != null) { t = 13; s = x13.outerHTML; }
            if (x14 != null) { t = 14; s = x14.outerHTML; }
            if (x15 != null) { t = 15; s = x15.outerHTML; }
            if (xx1 != null) { t = 1; s = xx1.outerHTML; }
            if (xx3 != null) { t = 3; s = xx3.outerHTML; }
            if (xx5 != null) { t = 5; s = xx5.outerHTML; }
            if (xx6 != null) { t = 6; s = xx6.outerHTML; }
            if (xx7 != null) { t = 7; s = xx7.outerHTML; }
            if (xx8 != null) { t = 8; s = xx8.outerHTML; }
            if (xx9 != null) { t = 9; s = xx9.outerHTML; }
            if (x10 != null) { t = 10; s = x10.outerHTML; }
            if (x11 != null) { t = 11; s = x11.outerHTML; }
            if (x12 != null) { t = 12; s = x12.outerHTML; }

            if (s0.Equals("mshtml.HTMLSpanElementClass") && s.Contains("Stundenplan") && my_state == MyState.INIT)
            {
               my_next_state = MyState.STUNDENPLAN;
               int n = s.Length;
               if (n > 900)
                  s = s.Substring(0, 899);
               System.Console.WriteLine("AAA******l={6},m={5},i={4}: xx{3} = {0} = {2} chars***********", s0, s, n, t, i, m, loop_cnt);
               x14.click();
               mytimer.Interval = dt_fast;
            }
            else if (s0.Equals("mshtml.HTMLTableCellClass") && s.Contains("Klassen") && my_state == MyState.STUNDENPLAN)
            {
               my_next_state = MyState.KLASSEN;
               System.Console.WriteLine("BBB******l={6},m={5},i={4}: xx{3} = {0} = {2} chars***********", s0, s, 0, t, i, m, loop_cnt);
               x15.click();
            }
            else if (s0.Equals("mshtml.HTMLInputElementClass") && s.Contains("timetablePageToolbar_dateWeekSelect") && my_state == MyState.KLASSEN)
            {
               if (my_next_state != MyState.UPDATE)
                  my_next_state = MyState.WOCHE;
               if (loop_cnt < 1000)
                  System.Console.WriteLine("CCC******l={6},m={5},i={4}: xx{3} = {0} = {2} chars*********** at {7}, txt={8}", s0, s, 0, t, i, m, loop_cnt, DateTime.Now.ToString("G", ci), xx1.value);
               // Creates a CultureInfo for German in Germany.
               this.MyUpdateText.Text = String.Format("Update {0}: {1}", Convert.ToDouble(loop_cnt), DateTime.Now.ToString("G", ci));
               mytimer.Interval = dt_slow;
               DateTime monday = DateTime.ParseExact(xx1.value, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
               DateTime now = DateTime.Today;

               if (now > monday.AddDays(4)) // e.g. monday is this week, and now is Saturday: Saturday > ((Monday+4) == Friday)
                  m_caret = "fa fa-caret-right";
               else if (now.AddDays(2) < monday) // e.g. monday is next week, and now is Friday: ((Friday+2)==Sunday) < Monday
                  m_caret = "fa fa-caret-left";
               else
               {
                  m_caret = "supercalifragilisticexpialidocious";
                  my_next_state = MyState.UPDATE;
               }
            }
            else if (s0.Equals("mshtml.HTMLSpanElementClass") && s.Contains("fa fa-caret-") && (my_state == MyState.WOCHE))
            {
               my_next_state = MyState.KLASSEN;
               if (s.Contains(m_caret))
               {
                  x14.click();
                  System.Console.WriteLine("DDD******l={6},m={5},i={4}: xx{3} = {0} = {2} chars*********** clicked at {7}", s0, s, 0, t, i, m, loop_cnt, DateTime.Now.ToString("G", ci));
               }
               else if (loop_cnt < 1000)
                  System.Console.WriteLine("DDD******l={6},m={5},i={4}: xx{3} = {0} = {2} chars***********", s0, s, 0, t, i, m, loop_cnt);
            }
            else if (check)
            {
               int n = s.Length;
               if (n > 900)
                  s = s.Substring(0, 899);
               System.Console.WriteLine("******l={6},m={5},i={4}: xx{3} = {0} = {2} chars***********\n{1}", s0, s, n, t, i, m, loop_cnt);
            }
            if (s0.Equals("mshtml.HTMLHtmlElementClass") && (my_state == MyState.UPDATE))
            {
               my_next_state = MyState.INIT;
               mytimer.Interval = dt_12min; // 12 Minute Wait
               string s_full_text = s;
               // Remove all text like this in s:
               // <div class="nowMarker" style="left: 144px; top: 339px; width: 144px;" data-reactid=".0.0.0.8.0.0.3">
               s = RemoveBlock(s, '<', "nowMarker", '>');
               s = RemoveBlock(s, '<', "timetableGridColumn", '>', false);
               s = RemoveBlock(s, '<', "timetableGridRow", '>', false);
               s = RemoveBlock(s, '<', "grupetWidgetTimetableEntryContent", '>', false);

// Replace text like: "currentDate":"2016-11-30"
// With a text like:  "currentDate":"yyyy-mm-dd",
               s = RemoveBlock(s, '"', "currentDate\":\"", '"', true, "\n\"currentDate\":\"yyyy-mm-dd\"\n");

// Replace text like: span class="date">Mi. 30.11.2016
// with a text like:  span class="date">DD. dd.mm.yyyy
               s = RemoveBlock(s, '<', "span class=\"date\">", '>');

// Replace text like: "selectedDay">Mi  30
// with a text like:  "selectedDay">DD  dd
               s = RemoveBlock(s, '<', "\"selectedDay\">", '>');

// Replace text like: "selectedMonthYear">November 2016
// with a text like:  "selectedMonthYear">MMM yyyy
               s = RemoveBlock(s, '<', "\"selectedMonthYear\">", '>');

               string s_reduced = s;
               s = RemoveBlock(s, '<', "script", '>', true, "");
               s = RemoveBlock(s, '<', "data-reactid=", '>');
               s = RemoveBlock(s, '<', "dijitBorderContainerNoGutter", '>');
               s = RemoveBlock(s, '<', "id=\"Timetable_toolbar\"", '>');
               s = RemoveBlock(s, '<', "id=\"grupet_widget_LabelPanel_", '>');
               s = RemoveBlock(s, '<', "id=\"dijit_layout__LayoutWidget_", '>');
               s = RemoveBlock(s, '<', "grupet_widget_MainMenu_dropDownPopup", '>');
               s = RemoveBlock(s, '<', "dijitPopupMenuItem", '>');
               s = RemoveBlock(s, '<', "grupet_widget_MainMenu", '>');
               s = RemoveBlock(s, '<', "/tbody></table></div", '>');
               if (stored_s != s)
               {
                  TextWriter tw;
                  stored_s = s;
                  fname = String.Format("{0}\\l{1}-m{2}-i{3}-t{4}-{5}.html", log_path, loop_cnt, m, i, t, s0);
                  tw = new StreamWriter(fname); tw.WriteLine(s_reduced); tw.Close();
                  fname = String.Format("{0}\\s{1}-m{2}-i{3}-t{4}-{5}.txt", log_path, loop_cnt, m, i, t, s0);
                  tw = new StreamWriter(fname); tw.WriteLine(s); tw.Close();
                  fname = String.Format("{0}\\s_full_text.html", log_path);
                  tw = new StreamWriter(fname); tw.WriteLine(s_full_text); tw.Close();
                  System.Console.WriteLine("   *************** Changes seen at {0} *******************", DateTime.Now.ToString("G", ci));
                  {
                     if (MyWindow.WindowState == WindowState.Minimized)
                        MyWindow.WindowState = WindowState.Normal;
                     MyWindow.Topmost = true;
                     MyWindow.Activate();
                     MyWindow.Topmost = false;
                  }
               }
            }
         }
         if ((loop_cnt < 1000) && (fname != null))
            System.Console.WriteLine("   ############################ Last file written to {0} #####################", fname);
         my_state = my_next_state;
      }
      private string RemoveBlock(string s, char start_char, string tag, char end_char, bool do_remove = true, string replacement = "\n")
      {
         int i = s.IndexOf(tag);

         if (i < 0)
            return s;

         string s1 = s.Substring(0, i);
         string s2 = s.Substring(i);
         s2 = s2.Substring(tag.Length); // remove the tag in case it contains the end_char
         int len = s1.LastIndexOf(start_char);
         if (len < 0)
            return s;
         s1 = s1.Substring(0, len);
         s2 = s2.Substring(s2.IndexOf(end_char) + 1);
         if (do_remove)
         {
            s1 = RemoveBlock(s1, start_char, tag, end_char, true, replacement);
            s2 = RemoveBlock(s2, start_char, tag, end_char, true, replacement);
            return (s1 + replacement + s2);
         }
         else
         {
            int j = s1.Length, k = s2.Length, l = s.Length;
            int len2 = l - j - k;
            s1 = RemoveBlock(s1, start_char, tag, end_char, do_remove);
            s2 = RemoveBlock(s2, start_char, tag, end_char, do_remove);
            if (len2 < 0)
               return s;
            tag = s.Substring(j, len2);
            return (s1 + "\n" + tag + "\n" + s2);
         }
      }
   }
}
