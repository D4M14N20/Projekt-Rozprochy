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
        private Form2 form;
        private StartingInfo startingInfo;
        private static Dictionary<Keys, bool> keysMap = new Dictionary<Keys, bool>();
        private static Dictionary<string, Player> otherPlayers = new Dictionary<string, Player>();
        private static Dictionary<Vector2, ExpPoint> expPoints = new Dictionary<Vector2, ExpPoint>();
        public static Vector2 Mouse { get; private set; } = new Vector2(0, 0);
        public static double FPS { get; private set; } = 0;
        public static double DeltaTime { get; private set; } = 0;
        //private static Thread gameThread;
        //private List<Player> otherPlayers = new List<Player>();
        public Game(Form2 form, StartingInfo startingInfo) { 
            this.startingInfo = startingInfo;
            this.form = form;
            Player.MPlayer = new Player(startingInfo.playerName);
            if (NewClient.Connected)
                Task.Run(NewClient.Sync);
            Task.Run(MainLoop);
        }
        private async Task MainLoop()
        {
            double frames = 0;
            double time = 0;
            Start();
            foreach (GameObject go in GameObject.GameObjects)
                go.Start();
            while (true)
            {
                long startTime = DateTime.Now.Ticks;
                //Render();
                await Task.Run(() =>
                {
                    form.Invalidate();
                    form.Invoke(new Action(() => form.Update()));
                    return Task.CompletedTask;
                });

                Update();
                var gos = GameObject.GameObjects;
                foreach (GameObject go in gos)
                    go.Go(DeltaTime);
                foreach (GameObject go in gos)
                    go.Update(DeltaTime);


                long endTime = DateTime.Now.Ticks;
                DeltaTime = (endTime - startTime) / (double)TimeSpan.TicksPerMillisecond;
                frames++;
                time += DeltaTime;
                if(time >= 1000)
                {
                    FPS = Math.Round(frames*(1000 / time));
                    time = 0;
                    frames = 0;
                }
            }
        }
        public static void SetMouse(Vector2 pos)
        {
            Mouse = pos;
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
                //lock (NewClient.GameObjects) { 
                    
                //}
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
        public static bool IsPressed(Keys key)
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

        private Bitmap drawingBitmap;
        private double Camerax { get; set; }
        private double Cameray { get; set; }
        private void Render()
        {
            drawingBitmap = new Bitmap(form.Size.Width, form.Size.Height);
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                double scale = form.Zoom;
                Size size = form.Size;
                double deltaTime = Game.DeltaTime;
                Camerax = GameMethods.Lerp(Camerax, Player.MPlayer.Position.x, 0.005 * deltaTime);
                Cameray = GameMethods.Lerp(Cameray, Player.MPlayer.Position.y, 0.005 * deltaTime);
                Player player = Player.MPlayer;

                //drawGrid2(e.Graphics, 4.0f);
                foreach (GameObject go in GameObject.GameObjects.OrderBy((GameObject go) => { return go; }, new GameObject.SizeComparer()))
                {
                    if (go == null)
                        continue;
                    Point z = go.GetScreenPosition(Camerax, Cameray, size, scale);
                    double r = scale;
                    if (go.GetType() == typeof(Player))
                        r = ((Player)go).Size * scale;
                    if (z.X < -r || z.X > size.Width + r || z.Y < -r || z.Y > size.Height + r)
                        continue;
                    go.Draw(g, Camerax, Cameray, size, scale);

                }
            }

            // Aktualizacja PictureBox z wątku głównego
            //pictureBox.BeginInvoke(new Action(() =>
            //{
                form.BackgroundImage = drawingBitmap;
            //}));
            //Thread.Sleep(100);

        }
    }
    public static class GameMethods
    {
        public static Vector2 ToGamePoint(this Point point, Size screenSize, Vector2 cameraPosition, double scale)
        {
            return new Vector2(cameraPosition.x + (point.X-screenSize.Width/2.0)/scale, cameraPosition.y+(-point.Y + screenSize.Height / 2.0) / scale);
        }
        public static double Lerp(double a, double b, double t, double ts=0, double te=1)
        {
            t = (t - ts)/(te-ts);
            t = Clamp01(t);
            return a + (b - a) * t;
        }
        private static double Clamp01(double value)
        {
            return (value < 0f) ? 0 : (value > 1) ? 1 : value;
        }
    }
}
