using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioGame
{
    internal class ExpPoint:GameObject
    {
        public Color Color { get { return circleBrush.Color; } set { circleBrush.Color = value; } }
        private static Random random = new Random();
        public ExpPoint()
        {
            Color = GameMethods.RandomColor(random);
            Initialize();
        }
        public ExpPoint(Vector2 position)
        {
            Random random = new Random((int)(position.x*position.y));
            Color = GameMethods.RandomColor(random);
            Position = position;
            Layer = -3;
            Initialize();
        }
        SolidBrush circleBrush = new SolidBrush(Color.White);
        public override void Draw(Graphics g, double camerax, double cameray, Size size, double scale = 1.0f)
        {
            float x = (float)GetScreenPosition(camerax, cameray, size, scale).x;
            float y = (float)GetScreenPosition(camerax, cameray, size, scale).y;
            float r = (float)(1 * scale);

            // Rysowanie koła
            g.FillEllipse(circleBrush, x - r, y - r, 2 * r, 2 * r);
        }
        public override void Start()
        {

        }
        public override void Update(double time)
        {

        }
    }
}
