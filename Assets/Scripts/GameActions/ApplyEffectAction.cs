using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Models.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameActions
{
    public class ApplyEffectAction : GameAction, IAbilityLoader
    {
        public List<Card> targets;
        public Effect effect;

        public void Load(IContainer game, Ability ability)
        {
            var targetSelector = ability.GetAspect<ITargetSelector>();
            targets = targetSelector.SelectTargets(game);

            var data = (Dictionary<string, object>)ability.userInfo;
            effect = new Effect();
            effect.ActionName = (string)data["action"];
            effect.userInfo = data["info"];
            effect.TriggerName = (string)data["trigger"];
        }
    }
}
