using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enums
{
    [Flags]
    public enum DamageType
    {
        Physical = 1 << 0,
        Magical = 1 << 1,
        Poison = 1 << 2,
        True = 1 << 3,

        Direct = Physical | Magical,
        Lethal = Direct | True,
        Any = Lethal | Poison
    }
}
