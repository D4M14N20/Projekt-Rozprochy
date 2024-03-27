using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace agario
{
    internal static class NewClient
    {
        private static TcpClient client;
        private static NetworkStream stream;
        private static StreamWriter writer;
        private static StreamReader reader;
        public static bool Connected { get; private set; } = false;
        public static int Ping { get; private set; } = 0;
        public static Dictionary<string, PlayerState> PlayerStates { get; private set; } = new Dictionary<string, PlayerState>();
        public static List<Vector2> ExpPoins { get; private set; } = new List<Vector2>();
        //public static List<Gam> ExpPoins { get; private set; } = new List<Vector2>();
        private static Queue<Vector2> Eated { get; set; } = new Queue<Vector2>();
        private static Queue<string> Killed { get; set; } = new Queue<string>();
        //[DllImport("kernel32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool AllocConsole();
        public static Task<bool> Connect(string name, string ip = "127.0.0.1")
        {
            string serverIP = ip;
            int serverPort = 50080;
            client = new TcpClient();
            try
            {
                client.Connect(serverIP, serverPort);
            }
            catch (SocketException ex)
            {
                //AllocConsole();
                Console.WriteLine(ex);
                if (client.Connected)
                    client.Close();
                return Task.FromResult(false);
            }
            stream = client.GetStream();
            writer = new StreamWriter(stream, Encoding.ASCII);
            writer.AutoFlush = true;
            reader = new StreamReader(stream, Encoding.ASCII);


            Write("connect");
            Write(name);
            string msg = Read();
            if (msg == "accept")
            {
                Console.WriteLine("poloczono");
                ((Form1)Form1.ActiveForm).Konsola = ("poloczono");
                Connected = true;
                return Task.FromResult(true);
            }
            else
            {
                //AllocConsole();
                Console.WriteLine("blad nie poloczono");
                client.Close();
                return Task.FromResult(false);
            }
        }
        private static Task StartSync()
        {
            if (client.Connected)
                Take();
            return Task.CompletedTask;
        }
        public static Task Sync()
        {

            StartSync();
            Task task = null;
            SetStart();
            while (client.Connected)
            {
                Ping = GetPing();
                Get();
                if (task == null || task.IsCompleted)
                {
                    Take();
                    task = Task.Delay(100);
                }
                Eat();
                Kill();
                if (IsKilled())
                {
                    client.Close();
                    Application.Exit();
                }
                else
                    Set();
                Thread.Sleep(10);
            }
            client.Close();
            return Task.CompletedTask;
        }
        public static Task Disconnect()
        {
            if (client.Connected)
                client.Close();
            return Task.CompletedTask;
        }
        private static void Write(string msg)
        {
            writer.WriteLine(msg);
        }
        private static string Read()
        {
            return reader.ReadLine();
        }



        private static void Set()
        {
            Write("set");
            string msg = PlayerState.GetPlayerState(Player.MPlayer).ToJson();
            Write(msg);

            string str = Read();
            if (str == "ok")
                return;
        }
        private static void SetStart()
        {
            Write("setStart");
            string msg = PlayerState.GetPlayerState(Player.MPlayer).ToJson();
            Write(msg);
        }
        static public readonly object NewClientLockObject = new object();
        private static void Get()
        {
            Write("get");
            string msg = Read();
            string[] strs = msg.Split(';');
            lock (NewClientLockObject)
            {
                PlayerStates.Clear();
                foreach (string s in strs)
                {
                    string[] ss = s.Split('!');
                    if (ss.Length == 2)
                    {
                        PlayerState ps = new PlayerState(ss[1]);
                        PlayerStates.Add(ss[0], ps);
                    }
                }
            }
        }
        private static Task Take()
        {
            Write("take");
            string msg = Read();

            lock (ExpPoins)
            {
                ExpPoins.Clear();
                while (msg != "end")
                {
                    ExpPoins.Add(new Vector2(msg));
                    msg = Read();
                }
            }
            return Task.CompletedTask;
        }
        private static void Eat()
        {
            lock (Eated)
            {
                while (Eated.Count > 0)
                {
                    Write("eat");
                    string front = Eated.Dequeue().ToString2();
                    Write(front);
                }
            }
        }
        private static void Kill()
        {
            lock (Killed)
            {
                while (Killed.Count > 0)
                {
                    Write("kill");
                    lock (Killed)
                    {
                        string front = Killed.Dequeue();
                        Write(front);
                    }
                }
            }
        }
        private static bool IsKilled()
        {
            Write("isKilled");
            string answer = Read();
            if (answer == "yes")
                return true;
            else
                return false;
        }
        private static int GetPing()
        {
            int ping = 0;
            long startTime = DateTime.Now.Ticks;
            Write("Ping");
            Read();
            long endTime = DateTime.Now.Ticks;
            ping = (int)((endTime - startTime) / (double)TimeSpan.TicksPerMillisecond);
            return ping;
        }



        public static Task Eat(Vector2 position)
        {
            lock (Eated)
                Eated.Enqueue(position);
            return Task.CompletedTask;
        }
        public static Task Kill(string name)
        {
            lock (Killed)
                Killed.Enqueue(name);
            return Task.CompletedTask;
        }
    }
}
