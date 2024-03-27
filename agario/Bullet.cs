using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agario
{
    internal class Bullet : GameObject
    {
        public Color Color { get; set; }
        private static Random random = new Random();
        private double distance = 0;
        public double Range { get; set; } = 48.0;
        public double Size { get; set; } = 1.5;
        public Player Owner { get; private set; } = null;
        private bool hitted=false;
        public Bullet(Vector2 position, Player owner=null)
        {
            Owner = owner;
            Layer = 2;
            Color = Game.RandomColor(random);
            Drag = 0.5;
            this.Position = position;
            Initialize();
        }
        private SolidBrush circleBrush = new SolidBrush(Color.White);
        private Pen borderPen = new Pen(Color.Gray, 5);
        public override void Draw(Graphics g, double camerax, double cameray, Size size, double scale = 1.0f)
        {
            int x = GetScreenPosition(camerax, cameray, size, scale).X;
            int y = GetScreenPosition(camerax, cameray, size, scale).Y;
            int r = (int)(Size * scale);

            // Rysowanie koła
            circleBrush.Color = Color; 
            borderPen.Color = Color.FromArgb(Color.A, Color.R / 2, Color.G / 2, Color.B / 2);

            borderPen.Width = (float)(scale / 4.0);
            g.FillEllipse(circleBrush, x - r, y - r, 2 * r, 2 * r);
            g.DrawEllipse(borderPen, x - r, y - r, 2 * r, 2 * r);

        }
        public override void Go(double time)
        {
            double k =  Math.Min(Drag * (time / 1000.0), 1.0);
            Velocity -= Velocity*k;
            //Velocity *= Math.Max(1 - Drag * (time / 1000.0), 0);
            Position += Velocity * (time / 1000.0);
            distance += (Velocity * (time / 1000.0)).Magnitude;
            //if (distance > Range * 0.75) 
            //Color = Color.FromArgb((int)GameMethods.Lerp(255, 0, distance, Range*0.75, Range), Color);
            if (distance > Range)
                hitted = true;
                    //Destroy();
        }
        double timeD = 0;
        public override void Update(double time)
        {
            if (hitted){
                timeD += time*0.002;
                Color = Color.FromArgb((int)GameMethods.Lerp(255, 0, timeD), Color);
                Drag = 10;
                if (timeD > 1)
                    Destroy();
            }
            if(Owner == Player.MPlayer)
                foreach (GameObject go in GameObjects)
                {
                    if (go.GetType() == typeof(ExpPoint))
                    {
                        if ((go.Position - Position).Magnitude < 2.5)
                        {
                            Player.MPlayer.Size = (double)Math.Sqrt(Player.MPlayer.Size* Player.MPlayer.Size + 0.1);
                            go.Destroy();
                            Task.Run(() => { NewClient.Eat(go.Position); });
                            hitted = true;
                        }

                    }
                   /* else if (go.GetType() == typeof(Player) && this != go)
                    {
                        Player pl = (Player)go;
                        if (!pl.Phantom && (go.Position - Position).Magnitude < Size && Size > pl.Size)//*1.2f)
                        {
                            go.Destroy();
                            Task.Run(() => { NewClient.Kill(pl.Name); });
                            Size = (double)Math.Sqrt(Size * Size + pl.Size + pl.Size);
                        }
                    }*/
                }
        }
    }
}
