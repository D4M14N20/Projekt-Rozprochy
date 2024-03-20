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
        private Color color;
        public Color Color { get { return color; } set { color = value;  circleBrush.Color = value; borderPen.Color = Color.FromArgb(value.R/2, value.G/2, value.B/2); } }
        private static Player player;
        public static Player MPlayer { get { return player; } }
        public float Size {get; set; }
        public Player(string name) {
            this.Name = name;
            player = this;
            Size = 2.5f;
        }
        private Pen borderPen = new Pen(Color.DarkRed, 5);
        private SolidBrush circleBrush = new SolidBrush(Color.Red);
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
    }
}
