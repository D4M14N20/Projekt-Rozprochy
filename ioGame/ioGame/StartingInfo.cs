using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioGame
{
    public struct StartingInfo
    {
        public StartingInfo(string name, Color color){
            this.color = color;
            this.playerName = name;
        }
        public string playerName;
        public Color color;
    }
}
