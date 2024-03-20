using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agario
{
    internal class ExpPoint:GameObject
    {
        public Color Color { get { return circleBrush.Color; } set { circleBrush.Color = value; } }
        private static Random random = new Random();
        public ExpPoint()
        {
            Color = Game.RandomColor(random);
        }
        SolidBrush circleBrush = new SolidBrush(Color.White);
        public override void Draw(Graphics g, float camerax, float cameray, Size size, float scale = 1.0f)
        {
            int x = GetScreenPosition(camerax, cameray, size, scale).X;
            int y = GetScreenPosition(camerax, cameray, size, scale).Y;
            int r = (int)(1 * scale);

            // Rysowanie koła
            g.FillEllipse(circleBrush, x - r, y - r, 2 * r, 2 * r);
        }
        public override void Start()
        {

        }
        public override void Update(float time)
        {

        }
    }
}
