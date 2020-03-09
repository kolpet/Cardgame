using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface IResistant
    {
        int Armor { get; set; }

        int MagicResist { get; set; }
    }
}
