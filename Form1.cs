using CefSharp.WinForms;
using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrzegladarkaProjekt
{
    public partial class Form1 : Form
    {
        private DataTable dataTable;
        private int currentId;
        public Form1()
        {
            InitializeComponent();
        }

        ChromiumWebBrowser web;

        private void Form1_Load_1(object sender, EventArgs e)
        {
            CefSettings settings = new CefSettings(); //ustawienia dla biblioteki CefSharp
            //Inicjalizacja
            Cef.Initialize(settings);
            textBox1.Text = "https://www.interia.pl";
            web = new ChromiumWebBrowser(textBox1.Text);
            web.Parent = tabControl1.SelectedTab; //wyswietlenie nazwy w zakladce
            //this.panel1.Controls.Add(web);
            web.Dock = DockStyle.Fill; //wypelnia calą dostpena
            web.AddressChanged += web_AddressChanged;
            web.TitleChanged += web_TitleChanged;
            

            // Tworzenie tabeli
            dataTable = new DataTable();  
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Godzina", typeof(DateTime));
            dataTable.Columns.Add("LinkDoStrony", typeof(string));

            AddDate();
        }

        //zmiana adresu
        private void web_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
                {
                textBox1.Text = e.Address;
                    AddDate();
            }));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser web = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (web != null)
            {
                String url = textBox1.Text;
                if (Uri.TryCreate(url, UriKind.Absolute, out Uri result) && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
                {
                    web.Load(textBox1.Text);
                }
                else
                {
                    web.Load("https://www.google.com/search?q=" + url.Replace(" ", "+"));
                }
                /*
                char kropka = '.';              //kropka
                int licz = url.Count(c => c == kropka); //sprawdzanie czy są dwie kropki
                if (licz == 2)
                {
                    web.Load(textBox1.Text);
                }
                else 
                {
                    web.Load("https://www.google.com/search?q=" + url.Replace(" ", "+"));
                }
                */
                AddDate();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser web = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (web != null)
                web.Refresh(); //odswiez
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser web = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (web != null)
            { 
            if (web.CanGoForward)
                web.Forward();
            }
            AddDate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser web = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (web != null)
            {
                if (web.CanGoBack)
                    web.Back();
            }
            AddDate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TabPage tab = new TabPage();
            tab.Text = "Nowy";
            tabControl1.Controls.Add(tab);
            tabControl1.SelectTab(tabControl1.TabCount - 1);
            ChromiumWebBrowser web = new ChromiumWebBrowser("https://www.interia.pl");
            web.Parent = tab;
            web.Dock = DockStyle.Fill;
            textBox1.Text = "https://www.interia.pl";
            web.AddressChanged += web_AddressChanged;
            web.TitleChanged += web_TitleChanged;

            
        }

        private void web_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                tabControl1.SelectedTab.Text = e.Title;
            }));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Wyświetlanie danych za pomocą MessageBox
            string message = "ID\tData / Godzina\tLink do Strony\n";

            foreach (DataRow row in dataTable.Rows)
            {
                int id = (int)row["ID"];
                DateTime pobranaGodzina = (DateTime)row["Godzina"];
                string linkDoStrony = (string)row["LinkDoStrony"];

                message += $"{id}\t{pobranaGodzina}\t{linkDoStrony}\n";
            }

            MessageBox.Show(message, "Historia Przeglądania", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataTable.Clear();
        }

        private void AddDate()
        {
            DateTime currentDateTime = DateTime.Now;
            dataTable.Rows.Add(currentId, currentDateTime, textBox1.Text);
            currentId++;
        }
    }
}
