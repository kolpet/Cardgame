using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enums
{
    [Flags]
    public enum CardType
    {
        None = 0,
        Action = 1 << 1,
        Consumable = 1 << 2,
        Equip = 1 << 3,
        Event = 1 << 4,
        Monster = 1 << 5,
        Passive = 1 << 6,

        Curse = Consumable | Event
    }

    public static class CardTypeExtensions
    {
        public static bool Contains(this CardType source, CardType target)
        {
            return (source & target) == target;
        }
    }
}
