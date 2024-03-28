using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Drawing;

namespace agario
{
    internal struct PlayerState
    {
        public double posX, posY, vX, vY;
        public Color color;
        //List<string> events;
        //List<List<object>> eventsArgs=new List<List<object>>();
        public PlayerState(double posX, double posY, double vX, double vY, Color color)
        {
            //events = new List<string>();
            //eventsArgs = new List<List<object>>();
            this.posX = posX;
            this.posY = posY;
            this.vX = vX;
            this.vY = vY;
            this.color = color; 
        }
        public PlayerState(string Json)
        {
            try
            {
                this = JsonConvert.DeserializeObject<PlayerState>(Json);
            }
            catch 
            {
                this = new PlayerState();
            }
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public override string ToString()
        {
            return ToJson();
        }
        public static PlayerState GetPlayerState(Player player)
        {
            return new PlayerState(player.Position.x, player.Position.y, player.Velocity.x, player.Velocity.y, player.Color);
        }
        public static void Set(this Player player, PlayerState ps)
        {
            player.Position = new Vector2(ps.posX, ps.posY);
            player.Velocity = new Vector2(ps.vX, ps.vY);
            player.Color = ps.color;
        }
    }
}
