using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameActions
{
    public class NextTurnAction : GameAction
    {
        public int targetPlayerIndex;

        public NextTurnAction(int targetPlayerIndex) => this.targetPlayerIndex = targetPlayerIndex;
    }
}
