using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class VictorySystem : Aspect
    {
        public bool IsGameOver()
        {
            var match = Container.GetMatch();
            foreach(Combatant combatant in match.players)
            {
                if (PartyDied(combatant))
                    return true;
            }
            return false;
        }

        bool PartyDied(Combatant combatant)
        {
            foreach(var card in combatant.party)
            {
                var destructable = card as IDestructable;
                if (destructable.HitPoints > 0)
                    return false;
            }
            return true;
        }
    }
}
