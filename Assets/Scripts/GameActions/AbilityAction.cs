using Assets.Scripts.Models.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameActions
{
    public class AbilityAction : GameAction
    {
        public Ability ability;

        public AbilityAction(Ability ability) => this.ability = ability;
    }
}
