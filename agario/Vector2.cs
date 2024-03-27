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
        public static Vector2 Zero { get { return new Vector2(0, 0); } }
        public double x; 
        public double y;
        public double Magnitude { get { return (double) Math.Sqrt(x*x+y*y); } }
        public double Angle { get { return Math.Atan2(y, x); } }
        public Vector2(double x, double y)
        {
            this.x = x; 
            this.y = y; 
        }
        public Vector2(Vector2 v)
        {
            x = v.x;
            y = v.y;
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
        public Vector2 Rotated(double angle)
        {
            double mag = Magnitude;
            double ang = Angle;
            return FromAngle(ang + angle, mag);
        }
        public static Vector2 FromAngle(double angle, double magnitude=1.0)
        {
            return new Vector2(Math.Cos(angle), Math.Sin(angle))*magnitude;
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.x, -a.y);
        }
        public static Vector2 operator+(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x+b.x, a.y+b.y);
        }
        public static Vector2 operator *(Vector2 a, double b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator *(double b, Vector2 a)
        {
            return a*b;
        }
        public static Vector2 operator /(Vector2 a, double b)
        {
            return new Vector2(a.x / b, a.y / b);
        }
        public Point toPoint(double scale=1.0f)
        {
            return new Point((int)Math.Round(this.x*scale), (int)Math.Round(this.y*-scale));
        }
        public Vector2 ToUnitVector()
        {
            return new Vector2(this) / this.Magnitude;
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
