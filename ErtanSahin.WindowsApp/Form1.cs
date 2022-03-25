using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
namespace ErtanSahin.WindowsApp
{
    public partial class Form1 : Form
    {
        readonly HttpClientHandler _clientHandler = new HttpClientHandler();
        ListView listView = new ListView();
        IConfiguration _config;
        string api_path = "";
        public Form1()
        {
            _config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", true, true)
          .Build();

            api_path = _config["api-path"];


            _clientHandler.ServerCertificateCustomValidationCallback = (csender, cert, chain, sslPolicyErrors) => true;
            InitializeComponent();
            BuildForm();

            CallReservations();

            var timer1 = new Timer { Interval = 10000, Enabled = true };

            timer1.Tick += Timer1_Tick;
        }
        private void BuildForm()
        {

            listView.Dock = DockStyle.Fill;
            listView.FullRowSelect = true;
            listView.View = View.Details;
            listView.Columns.Add("RoomNumber", "RoomNumber", 150);
            listView.Columns.Add("OwnerName", "OwnerName", 150);
            listView.Columns.Add("ReservationStart", "ReservationStart", 150);
            listView.Columns.Add("ReservationEnd", "ReservationEnd", 150);

            this.Controls.Add(listView);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            CallReservations();

        }

        private void CallReservations()
        {
            Text = "Last Update Time " + DateTime.Now;


            listView.Items.Clear();

            var client = new HttpClient(_clientHandler);
            client.DefaultRequestHeaders.Add("api-key", "ACECA62D-F4A9-4973-91E7-140043816A72");
            var response = client.GetAsync(new Uri($"{api_path}api/reservation/filter")).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var responseModel = JsonConvert.DeserializeObject<List<ReservationModel>>(content);

                foreach (var item in responseModel)
                {
                    var l = new ListViewItem { Text = item.RoomNumber };
                    l.SubItems.Add(item.OwnerName);
                    l.SubItems.Add(item.RezervationStart.ToString(new CultureInfo("tr")));
                    l.SubItems.Add(item.RezervationEnd.ToString(new CultureInfo("tr")));

                    if (DateTime.Now >= item.RezervationStart && item.RezervationEnd > DateTime.Now)
                    {
                        l.BackColor = Color.Green;
                        l.ForeColor = Color.Black;
                    }

                    listView.Items.Add(l);
                }
            }
            else
            {
                MessageBox.Show(content);
            }
        }
    }
}
