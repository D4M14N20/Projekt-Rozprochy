using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace agario
{
    public struct Vector2
    {
        public float x; 
        public float y;
        public float Magnitude { get { return (float) Math.Sqrt(x*x+y*y); } }
        public Vector2(float x, float y)
        {
            this.x = x; 
            this.y = y; 
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator+(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x+b.x, a.y+b.y);
        }
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public Point toPoint(float scale=1.0f)
        {
            return new Point((int)(this.x*scale), (int)(this.y*-scale));
        }
        public override string ToString()
        {
            return "{"+Math.Round(x, 0)+","+ Math.Round(y, 0) + "}";
        }
    }
}
