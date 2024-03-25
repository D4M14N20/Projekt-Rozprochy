using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace agario
{
    public partial class Form2 : Form
    {
        private Game game;
        private double targetZoom=15;
        public double Zoom { get; private set; }
        private double Camerax { get; set; }
        private double Cameray { get; set; }
        public Form2(StartingInfo startingInfo)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            game = new Game(this, startingInfo);
            Zoom = 15;
            Camerax = 0;
            Cameray = 0;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            Game.setKey(e.KeyCode, true);
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            Game.setKey(e.KeyCode, false);
        }

        void drawGrid(Graphics g, double length)
        {
            Pen pen = new Pen(fpsLabel.BackColor, 1);
            //pen.Width = Zoom / 15.0f;
            double x = (Game.GetScreenPosition(Camerax, Cameray, Size, 0, 0, Zoom).X)%(length*Zoom);
            while (x < Size.Width)
            {
                Point start = new Point((int)x, 0);
                Point end = new Point((int)x, Size.Height);
                g.DrawLine(pen, start, end);
                x += (length*Zoom);
            }
            double y = (Game.GetScreenPosition(Camerax, Cameray, Size, 0, 0, Zoom).Y) % (length * Zoom);
            while (y < Size.Height)
            {
                Point start = new Point(0, (int)y);
                Point end = new Point(Size.Width, (int)y);
                g.DrawLine(pen, start, end);
                y += (length * Zoom);
            }
        }
        private string pos = "";
        private double time = 0;
        private int fps = 0;
        private long lastTime = 0;
        private int frames = 0;
        private double deltaTime = 0;
        public static double Lerp(double a, double b, double t)
        {
            t = Clamp01(t);
            return a + (b - a) * t;
        }

        private static double Clamp01(double value)
        {
            return (value < 0f) ? 0 : (value > 1) ? 1 : value;
        }
        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Camerax = Lerp(Camerax, Player.MPlayer.Position.x, 0.005*deltaTime);
            //Camerax = Player.MPlayer.Position.x;//, 0.005*deltaTime);
            //Cameray = Player.MPlayer.Position.y;//, 0.005*deltaTime);
            Cameray = Lerp(Cameray, Player.MPlayer.Position.y, 0.005*deltaTime);
            Zoom = Lerp(Zoom, targetZoom, 0.005f * deltaTime);
            //Zoom = targetZoom;

            Player player = Player.MPlayer;

            

            GameObject[] gos = GameObject.GameObjects;
            Array.Sort<GameObject>(gos, new GameObject.SizeComparer());
            //List<GameObject> gos = GameObject.GameObjects;
            //gos.Sort(new GameObject.SizeComparer());
            drawGrid(e.Graphics, 4.0f );
            foreach (GameObject go in gos)
            {
                if(go==null)
                    continue; 
                Point z = go.GetScreenPosition(Camerax, Cameray, Size, Zoom);
                if (z.X < -200 || z.X > Size.Width + 200 || z.Y < -200 || z.Y > Size.Height + 200)
                    continue;
                go.Draw(e.Graphics, Camerax, Cameray, Size, Zoom);
        
            }
            if(pos!= "Pos: " + player.Position.ToString()){
                pos = "Pos: " + player.Position.ToString();
                positionLabel.Text = pos;
                positionLabel.Refresh();
            }


            long timeNow = DateTime.Now.Ticks;
            deltaTime = (timeNow - lastTime) / (double)TimeSpan.TicksPerMillisecond;
            //if (deltaTime < 3)
            //{
            //    Thread.Sleep(3 - (int)deltaTime);
            //    timeNow = DateTime.Now.Ticks;
            //    deltaTime = (timeNow - lastTime) / (double)TimeSpan.TicksPerMillisecond;
            //}
            lastTime = timeNow;
            
            
            time += deltaTime;
            frames++;
            if (time > 1000)
            {
                fps = (int)((frames*1000)/time);
                time = 0;
                frames = 0;
                fpsLabel.Text = "Fps: " + fps.ToString() + " Ping: " + NewClient.Ping;
                
                fpsLabel.Refresh();
            }
        }

        private void positionLabel_Click(object sender, EventArgs e)
        {

        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
           
        }
        private void Form2_MW(object sender, MouseEventArgs e)
        {
            int delta = e.Delta/120;
            for(int i = 0; i < delta; i++)
                if (targetZoom < 100.0f)
                    targetZoom *= 1.1f;
            for(int i = delta; i < 0; i++)
                if ( targetZoom > 2.0f)
                    targetZoom /= 1.1f;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void pingLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
