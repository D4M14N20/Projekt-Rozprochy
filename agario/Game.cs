using System;
using System.Collections;
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
        private double deltaTime = 0;
        private Form2 form;
        private StartingInfo startingInfo;
        private static Dictionary<Keys, bool> keysMap = new Dictionary<Keys, bool>();
        private static Dictionary<string, Player> otherPlayers = new Dictionary<string, Player>();
        private static Dictionary<Vector2, ExpPoint> expPoints = new Dictionary<Vector2, ExpPoint>();
        //private static Thread gameThread;
        //private List<Player> otherPlayers = new List<Player>();
        public Game(Form2 form, StartingInfo startingInfo) { 
            //if(otherPlayers!=null)
                //otherPlayers.Clear();
            //if(gameThread != null ) 
                //gameThread.Abort();
            //gameThread = new Thread(new ThreadStart(MainLoop));
            //gameThread.IsBackground = true;
            this.startingInfo = startingInfo;
            this.form = form;
            Player.MPlayer = new Player(startingInfo.playerName);


            //Random random = new Random();
            //for (int i = 1; i < 1000; i++)
            //{
            //    ExpPoint exp = new ExpPoint();
            //    exp.Position = new Vector2(random.Next(-200, 201), random.Next(-200, 201));
            //    exp.Layer = -1;
            //}
            //for (int i = 1; i < 250; i++)
            //{
            //    player = new Player("obj");
            //    player.Color = RandomColor(random);
            //    player.Position = new Vector2(random.Next(-200, 201), random.Next(-200, 201));
            //    player.Velocity = new Vector2(random.Next(-90, 91), random.Next(-90, 91));
            //    player.Drag = 1.2f;
            //    player.Size = i / 30 + 1;
            //}
            //player=Player.MPlayer


            if (NewClient.Connected)
                Task.Run(NewClient.Sync);
            Task.Run(MainLoop);
            //Thread.Sleep(10);
        }
        private Task MainLoop()
        {
            Start();
            foreach (GameObject go in GameObject.GameObjects)
                go.Start();
            while (true)
            {
                long startTime = DateTime.Now.Ticks;

                Update();
                foreach (GameObject go in GameObject.GameObjects)
                    go.Update(deltaTime);
                form.Invalidate();
                //Form.ActiveForm.Invoke(new Action(()=> { form.Invalidate(); }));
                


                long endTime = DateTime.Now.Ticks;
                deltaTime = (endTime - startTime)/(double)TimeSpan.TicksPerMillisecond;
            }
        }
        public static void setKey(Keys key, bool value)
        {
            keysMap[key] = value;
        }

        private void Start()
        {
            Player.MPlayer.Color= startingInfo.color;
        
        }
        private Task sync;
        private void Update()
        {
            double v0 = 12.0f;
            double vx = Player.MPlayer.Velocity.x;
            double vy = Player.MPlayer.Velocity.y;
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
            Player.MPlayer.Velocity = new Vector2(vx, vy);
            foreach (GameObject go in GameObject.GameObjects)
            {
                go.Go(deltaTime);
            }
            if(sync == null || sync.IsCompleted)
                sync = Task.Run(Sync);
        }
        private Task Sync()
        {
            if (NewClient.Connected)
            {
                lock (NewClient.NewClientLockObject)
                {
                    var playersToSpawn = NewClient.PlayerStates.Keys.Except(otherPlayers.Keys);
                    foreach (var kv in playersToSpawn)
                          otherPlayers[kv] = new Player(kv, NewClient.PlayerStates[kv]);

                    foreach (var kv in NewClient.PlayerStates)
                        otherPlayers[kv.Key].Set(kv.Value);
                    
                    var keysToRemove = otherPlayers.Keys.Except(NewClient.PlayerStates.Keys);
                    foreach (var key in keysToRemove)
                        otherPlayers[key].Phantom = true;
                }
                lock (NewClient.ExpPoins)
                {
                    var pointsToSpawn = NewClient.ExpPoins.Except(expPoints.Keys);
                    var pointsToRemove = expPoints.Keys.Except(NewClient.ExpPoins).ToArray();
                    foreach (var point in pointsToSpawn)
                        expPoints[point] = new ExpPoint(point);
                    foreach (var point in pointsToRemove)
                    {
                        expPoints[point].Destroy();
                        expPoints.Remove(point);
                    }
                }
            }
            return Task.CompletedTask;
        }
        private bool IsPressed(Keys key)
        {
            return keysMap.ContainsKey(key) && keysMap[key];
        }
        public static Point GetScreenPosition(double camerax, double cameray, Size size, double posx, double posy,double scale = 1.0f)
        {
            double x = (size.Width / 2.0 - scale * (camerax - posx));
            double y = (size.Height / 2.0 + scale * (cameray - posy));
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }
        public static Color RandomColor(Random random)
        {
            return Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
        }

        //private string pos = "";
        //private double time = 0;
        //private int fps = 0;
        //private long lastTime = 0;
        //private int frames = 0;
        //private double camerax;
        //private double cameray; 
        //void drawGrid(Graphics g, double length)
        //{
        //    Size size = form.Size;
        //    double Zoom = form.Zoom;
        //    Pen pen = new Pen(Color.FromArgb(50, 70, 80), 1);
        //    //Pen pen = new Pen(fpsLabel.BackColor, 1);
        //    int x = (Game.GetScreenPosition(camerax, cameray, size, 0, 0, Zoom).X) % (int)(length * Zoom);
        //    while (x < size.Width)
        //    {
        //        Point start = new Point(x, 0);
        //        Point end = new Point(x, size.Height);
        //        g.DrawLine(pen, start, end);
        //        x += (int)(length * Zoom);
        //    }
        //    int y = (Game.GetScreenPosition(camerax, cameray, size, 0, 0, Zoom).Y) % (int)(length * Zoom);
        //    while (y < size.Height)
        //    {
        //        Point start = new Point(0, y);
        //        Point end = new Point(size.Width, y);
        //        g.DrawLine(pen, start, end);
        //        y += (int)(length * Zoom);
        //    }
        //}
        //private void Paint()
        //{
        //    Size size = form.Size;
        //    double Zoom = form.Zoom;
        //    double Camerax = Player.MPlayer.Position.x;
        //    double Cameray = Player.MPlayer.Position.y;
        //    Graphics g = form.CreateGraphics();
        //    g.Clear(form.BackColor);

        //    Player player = Player.MPlayer;

        //    drawGrid(g, 2.0f);


        //    List<GameObject> gos = GameObject.GameObjects;
        //    gos.Sort(new GameObject.SizeComparer());
        //    foreach (GameObject go in gos)
        //    {
        //        Point z = go.GetScreenPosition(Camerax, Cameray, size, Zoom);
        //        if (z.X < -200 || z.X > size.Width + 200 || z.Y < -200 || z.Y > size.Height + 200)
        //            continue;
        //        go.Draw(g, Camerax, Cameray, size, Zoom);

        //    }
        //    if (pos != "Pos: " + player.Position.ToString())
        //    {
        //        pos = "Pos: " + player.Position.ToString();
        //        //positionLabel.Text = pos;
        //        //positionLabel.Refresh();
        //    }


        //    long timeNow = DateTime.Now.Ticks;
        //    double deltaTime = (timeNow - lastTime) / (double)TimeSpan.TicksPerMillisecond;
        //    lastTime = timeNow;


        //    time += deltaTime;
        //    frames++;
        //    if (time > 1000)
        //    {
        //        fps = (int)((frames * 1000) / time);
        //        time = 0;
        //        frames = 0;
        //        //fpsLabel.Text = "Fps: " + fps.ToString();

        //        //fpsLabel.Refresh();
        //    }
        //    g.Dispose();
        //}
    }
}
