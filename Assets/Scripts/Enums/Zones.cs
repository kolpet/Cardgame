using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enums
{
    [Flags]
    public enum Zones
    {
        None = 0,
        Party = 1 << 0,
        Deck = 1 << 1,
        Hand = 1 << 2,
        Graveyard = 1 << 3,
        Active = Party | Hand
    }
    public static class ZonesExtensions
    {
        public static bool Contains(this Zones source, Zones target)
        {
            return (source & target) == target;
        }
    }
}
