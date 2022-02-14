using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp; // Adding websocket
using Newtonsoft.Json.Linq; //Adding jsonj parser
using Newtonsoft.Json;
using System.Windows.Threading;

namespace ClientTest
{
    public partial class Form1 : Form
    {
        public class Vigorus
        {
            [JsonProperty("php")]
            public string peso { get; set; }
        }
        public class Data
        {
            [JsonProperty("vigorus")]
            public Vigorus vigorus { get; set; }
        }

        public class Price
        {
            [JsonProperty("code")]
            public string code { get; set; }

            [JsonProperty("data")]
            public Data data { get; set; }

            [JsonProperty("message")]
            public string message { get; set; }

            [JsonProperty("success")]
            public bool success{ get; set; }
        }

     

        WebSocket ws = new WebSocket(url: "ws://localhost:3300", onMessage: onMessage);
        string username = "guest";
        static dynamic json;
        static dynamic prevJson;
        public Form1()
        {
            InitializeComponent();
            ws.Connect();
            updateMessageBox();
        }

        private async void updateMessageBox()
        {
            while(true)
            {
                if (prevJson != json)
                {
                    try
                    {
                        Price castedJson = (Price)json;
                        addMessage((string)castedJson.data.vigorus.peso);
                    }
                    catch (Exception)
                    {

                    }
                }

                prevJson = json;
                await Task.Delay(100);
            }
        }
       
        private static Task onMessage(MessageEventArgs e)
        {
            json = JsonConvert.DeserializeObject<Price>(e.Text.ReadToEnd());
            return Task.FromResult(0);
        }

        private void addMessage(string peso)
        {
            
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                txtPeso.Text = (float.Parse(txtVis.Text) * float.Parse(peso)).ToString();
                lblVisPrice.Text = peso;
            });
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ws.Send("Hello");
           //ws.Send("{	\"MessageInfo\": {		\"Username\": \"" + username + "\",		\"Message\": \" " + txtVis.Text + "\"	}}");
            //ws.Send("{ \"MessageInfo\": { \"Username\": \"" + username + "\", \"Message\": \"" + textBox2.Text + "\" }}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
