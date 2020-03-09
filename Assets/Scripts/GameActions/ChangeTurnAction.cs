using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameActions
{
    public class ChangeTurnAction : GameAction
    {
        public int targetPlayerIndex;

        public ChangeTurnAction(int targetPlayerIndex) => this.targetPlayerIndex = targetPlayerIndex;
    }
}
