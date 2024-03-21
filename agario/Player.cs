using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace agario
{
    internal class Player : GameObject
    {
        public Color Color { get { return circleBrush.Color; } set { circleBrush.Color = value; borderPen.Color = Color.FromArgb(value.R/2, value.G/2, value.B/2); } }
        private static Player player;
        public static Player MPlayer { get { return player; } }
        public float Size {get; set; }
        public Player(string name) {
            this.Name = name;
            player = this;
            Size = 2.5f;
        }
        private Pen borderPen = new Pen(Color.Gray, 5);
        private SolidBrush circleBrush = new SolidBrush(Color.White);
        public override void Draw(Graphics g, float camerax, float cameray, Size size, float scale=1.0f)
        {
            int x = GetScreenPosition(camerax, cameray, size, scale).X;
            int y = GetScreenPosition(camerax, cameray, size, scale).Y;
            int r = (int)(Size*scale);

            // Rysowanie koła
            g.FillEllipse(circleBrush, x - r, y - r, 2 * r, 2 * r);
            // Rysowanie obramowania koła
            g.DrawEllipse(borderPen, x - r, y - r, 2 * r, 2 * r);
        }
        public override void Start()
        {

        }
        public override void Update(float time)
        {
            if(Player.MPlayer == this)
            {
                foreach(GameObject go in GameObject.GameObjects)
                {
                    if(go.GetType() == typeof(ExpPoint))
                    {
                        if((go.Position-Position).Magnitude < Size) {
                            go.Destroy();
                            Size=(float)Math.Sqrt(Size*Size+1);
                        }
                        
                    }
                    else if (go.GetType() == typeof(Player))
                    {
                        Player pl = (Player)go;
                        if ((go.Position - Position).Magnitude < Size&&Size>pl.Size)
                        {
                            go.Destroy();
                            Size = (float)Math.Sqrt(Size * Size + pl.Size+pl.Size);
                        }
                    }

                }
            }
        }
    }
}
