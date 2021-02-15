using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enums
{
    [Flags]
    public enum AuraType
    {
        None = 0,
        Stat = 1 << 1,
        Reaction = 1 << 2,
        Any = 255
    }

    public static class AuraTypeExtensions
    {
        public static bool Contains(this CardType source, CardType target)
        {
            return (source & target) == target;
        }
    }
}
