using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Mark
    {
        public Alliance alliance;
        public Zones zones;

        public Mark(Alliance alliance, Zones zones)
        {
            this.alliance = alliance;
            this.zones = zones;
        }

        public Mark(Dictionary<string, object> data)
        {
            alliance = (Alliance)Enum.Parse(typeof(Alliance), (string)data["alliance"]);
            zones = (Zones)Enum.Parse(typeof(Zones), (string)data["zone"]);
        }
    }
}
