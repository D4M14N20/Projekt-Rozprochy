using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioGame
{
    internal static class NewClient
    {
        private static TcpClient client;
        private static NetworkStream stream;
        private static StreamWriter writer;
        private static StreamReader reader;
        public static bool Connected { get; private set; } = false;
        public static int Ping { get; private set; } = 0;

        //struktury do ciaglej sychronizacji
        public static Dictionary<string, PlayerState> PlayerStates { get; private set; } = new Dictionary<string, PlayerState>();
        public static List<Vector2> ExpPoins { get; private set; } = new List<Vector2>();
        public static Queue<ServerEvent> ReceivedEvents { get; private set; } = new Queue<ServerEvent>();



        ///kolejki do zdarzen serwerowych;
        private static Queue<Vector2> eated = new Queue<Vector2>();
        private static Queue<string> killed = new Queue<string>();
        private static Queue<ServerEvent> eventsToSend = new Queue<ServerEvent>();
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
        public static Task Sync()
        {
            Task task = null;
            SetStart();
            while (client.Connected)
            {
                Ping = GetPing();
                Get();
                GetEvents();
                Eat();
                Set();
                Kill();
                AddEvent();
                if (task == null || task.IsCompleted)
                {
                    Take();
                    task = Task.Delay(100);
                }
                if (IsKilled())
                {
                    client.Close();
                    Application.Exit();
                }
                //Thread.Sleep(1);
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
        private static void Get()
        {
            Write("get");
            string msg = Read();
            string[] strs = msg.Split(';');
            lock (PlayerStates)
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
            lock (eated)
            {
                while (eated.Count > 0)
                {
                    Write("eat");
                    string front = eated.Dequeue().ToString2();
                    Write(front);
                }
            }
        }
        private static void Kill()
        {
            lock (killed)
            {
                while (killed.Count > 0)
                {
                    Write("kill");
                    string front = killed.Dequeue();
                    Write(front);
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
        private static void AddEvent()
        {            
            while (eventsToSend.Count > 0)
            {
                Write("AddEvent");
                lock (eventsToSend)
                {
                    string front = eventsToSend.Dequeue().ToJson();
                    Write(front);
                }
            }
        }
        private static void GetEvents()
        {
            Write("GetEvents");
            string ewent=Read();
            while (ewent!="end")
            {
                lock (ReceivedEvents)
                    ReceivedEvents.Enqueue(new ServerEvent(ewent));
                ewent = Read();
            }
        }














        public static void Eat(Vector2 position)
        {
            if (!Connected)
                return;
            Task.Run(() =>
            {
                lock (eated)
                    eated.Enqueue(position);
            });
            //return Task.CompletedTask;
        }
        public static void Kill(string name)
        {
            if (!Connected)
                return;
            Task.Run(() =>
            {
                lock (killed)
                    killed.Enqueue(name);
            });
            //return Task.CompletedTask;
        }
        public static void AddEvent(ServerEvent ewent)
        {
            if (!Connected)
                return;
            if (ewent.sender != Player.MPlayer.PlayerName)
                return;
            Task.Run(() =>
            {
                lock (eventsToSend)
                    eventsToSend.Enqueue(ewent);
            });
            //return Task.CompletedTask;
        }
    }
}
