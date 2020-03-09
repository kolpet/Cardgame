using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enums
{
    [Flags]
    public enum Alliance
    {
        None = 0,
        Self = 1 << 0,
        Enemy = 1 << 1,
        Ally = (1 << 2) | Self,
        Any = Ally | Enemy
    }

    public static class AllianceExtensions
    {
        public static bool Contains(this Alliance source, Alliance target)
        {
            return (source & target) == target;
        }
    }
}
