using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace agario
{
    public partial class Form2 : Form
    {
        public Label PlayerLabel{ get { return labelf2; } }
        private Game game;
        private float Zoom { get; set; }
        private float Camerax { get; set; }
        private float Cameray { get; set; }
        public Form2(StartingInfo startingInfo)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            game = new Game(this, startingInfo);
            Zoom = 20;
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

        private void Form2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            c = char.ToUpper(c);
            if (c == 'P'&&Zoom<100.0f)
                Zoom *= 1.1f;
            if (c == 'O'&&Zoom>2.0f)
                Zoom /= 1.1f;
        }
        void drawGrid(Graphics g, float length)
        {
            Pen pen = new Pen(Color.DarkOliveGreen, 1);
            int x =(Game.GetScreenPosition(Camerax, Cameray, Size, 0, 0, Zoom).X)%(int)(length*Zoom);
            while (x < Size.Width)
            {
                Point start = new Point(x, 0);
                Point end = new Point(x, Size.Height);
                g.DrawLine(pen, start, end);
                x += (int)(length*Zoom);
            }
            int y = (Game.GetScreenPosition(Camerax, Cameray, Size, 0, 0, Zoom).Y) % (int)(length * Zoom);
            while (y < Size.Height)
            {
                Point start = new Point(0, y);
                Point end = new Point(Size.Width, y);
                g.DrawLine(pen, start, end);
                y += (int)(length * Zoom);
            }
        }
        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Camerax = Player.MPlayer.Position.x;
            Cameray = Player.MPlayer.Position.y;

           

            drawGrid(e.Graphics, 2.0f );
            Point loc = Player.MPlayer.GetScreenPosition(Camerax, Cameray, Size, Zoom);
            PlayerLabel.Location = new Point(loc.X - PlayerLabel.Size.Width / 2, loc.Y - (int)(Player.MPlayer.Size*Zoom+30));
            //this.BackgroundImage.
            foreach (GameObject go in GameObject.GameObjects)
            {
                go.Draw(e.Graphics, Camerax, Cameray, Size, Zoom);
            }
            positionLabel.Text= "Pos: "+Player.MPlayer.Position.ToString();
            fpsLabel.Text = "Fps: "+game.Fps.ToString();
        }

        private void positionLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
