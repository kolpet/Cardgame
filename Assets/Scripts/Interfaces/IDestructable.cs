using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface IDestructable
    {
        int HitPoints { get; set; }

        int MaxHitPoints { get; set; }

        int TemporaryHitPoints { get; set; }
    }
}
