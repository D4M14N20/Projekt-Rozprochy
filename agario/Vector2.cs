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
        public double x; 
        public double y;
        public double Magnitude { get { return (double) Math.Sqrt(x*x+y*y); } }
        public Vector2(double x, double y)
        {
            this.x = x; 
            this.y = y; 
        }
        public Vector2(string v)
        {
            v = v.Trim('{', '}');
            string[] e = v.Split(':');
            try
            {
                this.x = Convert.ToInt32(e[0])/100.0f;
                this.y = Convert.ToInt32(e[1])/100.0f;
            }
            catch 
            {
                x = 22.0f; y = 0.0f;
            }
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator+(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x+b.x, a.y+b.y);
        }
        public static Vector2 operator *(Vector2 a, double b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public Point toPoint(double scale=1.0f)
        {
            return new Point((int)Math.Round(this.x*scale), (int)Math.Round(this.y*-scale));
        }
        public override string ToString()
        {
            return "{"+Math.Round(x, 0)+":"+ Math.Round(y, 0) + "}";
        }
        public string ToString2()
        {
            return "{" + (int)Math.Round(x*100) + ":" + (int)Math.Round(y*100) + "}";
        }
    }
}
