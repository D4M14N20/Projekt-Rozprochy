using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace agario
{
    internal class Game
    {
        private Player player;
        private float deltaTime = 0;
        private Form2 form;
        private Color color;
        private static Dictionary<Keys, bool> keysMap = new Dictionary<Keys, bool>();
        public int Fps {  get; private set; }
        public Game(Form2 form, StartingInfo startingInfo) { 
            Thread gameThread = new Thread(new ThreadStart(MainLoop));
            gameThread.IsBackground = true;
            gameThread.Start();
            this.color = startingInfo.color;
            this.form = form;



            Random random = new Random();
            for (int i = 1; i < 250; i++)
            {
                player = new Player("obj");
                player.Color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                player.Position = new Vector2(random.Next(-200, 201), random.Next(-200, 201));
                player.Velocity = new Vector2(random.Next(-90, 91), random.Next(-90, 91));
                player.Drag = 1.2f;
                player.Size = i / 30 + 1;
            }
            player = new Player(startingInfo.playerName);
            Start();
        }
        private void MainLoop()
        {
            float frames = 0;
            float time = 0;
            while (true)
            {
                long startTime = DateTime.Now.Ticks;
                frames++;
                time+=deltaTime;
                if(time >= 100) {
                    Fps = (int)Math.Round(frames / (time / 1000.0f));
                    time = 0;
                    frames = 0;
                }

                Update();
                form.Refresh();

                long endTime = DateTime.Now.Ticks;
                deltaTime = (endTime - startTime)/(float)TimeSpan.TicksPerMillisecond;
            }
        }
        public static void setKey(Keys key, bool value)
        {
            keysMap[key] = value;
        }

        private void Start()
        {
            player.Color= color;
            player.Drag = 1.2f;
            form.PlayerLabel.Text = player.Name;
        
        }
        private void Update()
        {
            float v0 = 12.0f;
            float vx = player.Velocity.x;
            float vy = player.Velocity.y;
            if (IsPressed(Keys.W))
            {
                vy = v0;
            }
            if (IsPressed(Keys.S))
            {
                vy = -v0;
            }
            if (IsPressed(Keys.A))
            {
                vx = -v0;
            }
            if (IsPressed(Keys.D))
            {
                vx = v0;
            }
            player.Velocity = new Vector2(vx, vy);
            foreach(GameObject go in GameObject.GameObjects)
            {
                go.go(deltaTime);
            }
        }
        private bool IsPressed(Keys key)
        {
            return keysMap.ContainsKey(key) && keysMap[key];
        }
        public static Point GetScreenPosition(float camerax, float cameray, Size size, float posx, float posy,float scale = 1.0f)
        {
            int x = size.Width / 2 - (int)(scale * (camerax - posx));
            int y = size.Height / 2 + (int)(scale * (cameray - posy));
            return new Point(x, y);
        }
    }
}
