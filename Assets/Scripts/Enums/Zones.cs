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
        Self = 1 << 0,
        Party = 1 << 1,
        Deck = 1 << 2,
        Hand = 1 << 3,
        Graveyard = 1 << 4,
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
