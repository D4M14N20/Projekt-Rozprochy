﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ioGame
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
            DoubleBuffered = true;
            game = new Game(this, startingInfo);
            Zoom = 15;
            Camerax = 0;
            Cameray = 0;
        }

        private void Form2_Load(object sender, EventArgs e)
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
            double x = (GameMethods.GetScreenPosition(Camerax, Cameray, Size, 0, 0, Zoom).x)%(length*Zoom);
            while (x < Size.Width)
            {
                Point start = new Point((int)x, 0);
                Point end = new Point((int)x, Size.Height);
                g.DrawLine(pen, start, end);
                x += (length*Zoom);
            }
            double y = (GameMethods.GetScreenPosition(Camerax, Cameray, Size, 0, 0, Zoom).y) % (length * Zoom);
            while (y < Size.Height)
            {
                Point start = new Point(0, (int)y);
                Point end = new Point(Size.Width, (int)y);
                g.DrawLine(pen, start, end);
                y += (length * Zoom);
            }
        }
        void drawGrid2(Graphics g, double length)
        {
            SolidBrush brush = new SolidBrush(fpsLabel.BackColor);
            double x = (GameMethods.GetScreenPosition(Camerax, Cameray, Size, 0, 0, Zoom).x) % (length * Zoom);
            while (x < Size.Width)
            {
                double y = (GameMethods.GetScreenPosition(Camerax, Cameray, Size, 0, 0, Zoom).y) % (length * Zoom);
                while (y < Size.Height)
                {
                    g.FillEllipse(brush, (int)x, (int)y, (int)(Zoom/2), (int)(Zoom/2));
                    y += (length * Zoom);
                }
                x += (length * Zoom);
            }
        }
        private string pos = "";

            
        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            double deltaTime = Game.DeltaTime;
            //Vector2 camera = new Vector2(Camerax, Cameray);
            //Vector2 deltaC = Player.MPlayer.Position - camera;
            //camera += deltaC * deltaTime/100024.0;

            Camerax = GameMethods.Lerp(Camerax, Player.MPlayer.Position.x, 0.005f * deltaTime);
            Cameray = GameMethods.Lerp(Cameray, Player.MPlayer.Position.y, 0.005f * deltaTime);
            //Point w = GameMethods.GetScreenPosition(Camerax, Cameray, Size, Camerax, Cameray, Zoom);
            //Vector2 wd = GameMethods.ToGamePoint(w, Size, new Vector2(Camerax, Cameray), Zoom);
            //Camerax = wd.x;
            //Cameray = wd.y;
            //Camerax = camera.x;
            //Cameray = camera.y;
            Game.SetCamera(new Vector2 (Camerax, Cameray));
            Zoom = GameMethods.Lerp(Zoom, targetZoom, 0.005f * deltaTime);
            Player player = Player.MPlayer;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextContrast = 0;

            drawGrid2(g, 4.0f);
            foreach (GameObject go in GameObject.GameObjects.OrderBy((GameObject go) => { return go; }, new GameObject.SizeComparer()))
            {
                if (go == null)
                    continue;
                Vector2 z = go.GetScreenPosition(Camerax, Cameray, Size, Zoom);
                double r = Zoom;
                if (go.GetType() == typeof(Player))
                    r = ((Player)go).Size * Zoom;
                if (z.x < -r || z.x > Size.Width + r || z.y < -r || z.y > Size.Height + r)
                    continue;
                go.Draw(g, Camerax, Cameray, Size, Zoom);

            }
            if (pos!= "Pos: " + player.Position.ToString()){
                pos = "Pos: " + player.Position.ToString();
                positionLabel.Text = pos;
            }
  
            fpsLabel.Text = "Fps: " + Game.FPS.ToString() + " Ping: " + NewClient.Ping;
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

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = new Point(e.X, e.Y);
            Game.SetMouse(pos.ToGamePoint(Size, new Vector2(0, 0), Zoom));
        }
    }
}
