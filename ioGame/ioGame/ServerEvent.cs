using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioGame
{
    internal enum GameEvents
    {
        SizeSet,
        BulletSpawn,
        BulletHitted
    }
    internal struct ServerEvent
    {
        public string sender;
        public GameEvents gameEvent;
        public object[] eventsArgs;
        public ServerEvent(string sender, GameEvents ewent, params object[] args)
        {
            this.sender = sender;
            this.gameEvent = ewent;
            this.eventsArgs = args;
        }
        private static JsonSerializerSettings settings = new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.Auto};
        public ServerEvent(string Json)
        {
            
            this = JsonConvert.DeserializeObject<ServerEvent>(Json, settings);
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
