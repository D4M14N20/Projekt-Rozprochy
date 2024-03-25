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
        public double size;
        public bool isDead;
        public Color color;
        public PlayerState(double posX, double posY, double vX, double vY, double size, bool isDead, Color color)
        {
            this.posX = posX;
            this.posY = posY;
            this.vX = vX;
            this.vY = vY;
            this.size = size;
            this.isDead = isDead;
            this.color = color;
        }
        public PlayerState(String Json)
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
        public String ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public override string ToString()
        {
            return ToJson();
        }
        public static PlayerState GetPlayerState(Player player)
        {
            return new PlayerState(player.Position.x, player.Position.y, player.Velocity.x, player.Velocity.y, player.Size, false, player.Color);
        }
    }
}
