using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameActions
{
    public class FatigueAction : GameAction
    {
        public FatigueAction(Player player)
        {
            this.Player = player;
        }
    }
}
