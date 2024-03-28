using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioGame
{
    public partial class Form1 : Form
    {
        public int clickCount = 0;
        private string name="";
        private  string wiadomosc="";
        private string ip="";
        public string Konsola{get { return wiadomosc; } set{wiadomosc=value; console.Text = wiadomosc; } }
        public Form1()
        {
            InitializeComponent();
            // Ustawienie AutoScaleMode na Dpi
        }

        private void button1_Click(object sender, EventArgs e) {
            if (NewClient.Connected||NewClient.Connect(name).Result)
            {
                Form2 form = new Form2(new StartingInfo(name, colorDialog1.Color));
                form.Show();
                this.Hide();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            colorDialog1.Color = GameMethods.RandomColor(new Random((int)DateTime.Now.Ticks));
            panel1.BackColor = colorDialog1.Color;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            name = textBox1.Text;
        }

        private void ipBox_TextChanged(object sender, EventArgs e)
        {
            ip = ipBox.Text;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void connectButton_Click(object sender, EventArgs e)
        {
            //Client.Connect(ipBox.Text, new Player(name
            NewClient.Connect(name, ip);
        }

        private void console_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            panel1.BackColor = colorDialog1.Color;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //NewClient.Disconnect();
        }
    }
}
