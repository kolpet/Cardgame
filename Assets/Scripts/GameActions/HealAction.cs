using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameActions
{
    public class HealAction : GameAction, IAbilityLoader
    {
        public List<IDestructable> targets;
        public int amount;

        public HealAction()
        {

        }

        public HealAction(IDestructable target, int amount)
        {
            targets = new List<IDestructable>(1) { target };
            this.amount = amount;
        }

        public HealAction(List<IDestructable> targets, int amount)
        {
            this.targets = targets;
            this.amount = amount;
        }

        public void Load(IContainer game, Ability ability)
        {
            var targetSelector = ability.GetAspect<ITargetSelector>();
            var cards = targetSelector.SelectTargets(game);
            targets = new List<IDestructable>();
            foreach (Card card in cards)
            {
                var destructable = card as IDestructable;
                if (destructable != null)
                    targets.Add(destructable);
            }

            amount = Convert.ToInt32(ability.userInfo);
        }
    }
}
