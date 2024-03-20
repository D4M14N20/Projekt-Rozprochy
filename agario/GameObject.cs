using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agario
{
    internal abstract class GameObject
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Drag {  get; set; }
        private static List<GameObject> gameObjects = new List<GameObject>();
        public static List<GameObject> GameObjects{ get { return gameObjects; } }
        public GameObject(string name="gameObject") { 
            this.Name = name;
            this.Position = new Vector2(0, 0);
            this.Velocity = new Vector2(0, 0);
            this.Drag = 0;
            gameObjects.Add(this);
        }
        public void go(float time)
        {
            Position += Velocity * (time/1000.0f);
            Velocity *= (1 - Drag * (time/1000.0f));
        }
        public abstract void Draw(Graphics g, float camerax, float cameray, Size size, float scale = 1.0f);
        public Point GetScreenPosition(float camerax, float cameray, Size size, float scale = 1.0f)
        {
            int x = size.Width / 2 - (int)(scale * (camerax - Position.x));
            int y = size.Height / 2 + (int)(scale * (cameray - Position.y));
            return new Point(x, y);
        }
    }
}
