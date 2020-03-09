using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameActions
{
    public class DamageAction : GameAction, IAbilityLoader
    {
        public List<IDestructable> targets;
        public DamageType type;
        public int amount;

        public DamageAction()
        {

        }

        public DamageAction(IDestructable target, int amount, DamageType type = DamageType.Physical)
        {
            targets = new List<IDestructable>(1) { target };
            this.type = type;
            this.amount = amount;
        }

        public DamageAction(List<IDestructable> targets, int amount, DamageType type = DamageType.Physical)
        {
            this.targets = targets;
            this.type = type;
            this.amount = amount;
        }

        public void Load(IContainer game, Ability ability)
        {
            var targetSelector = ability.GetAspect<ITargetSelector>();
            var cards = targetSelector.SelectTargets(game);
            targets = new List<IDestructable>();
            foreach(Card card in cards)
            {
                var destructable = card as IDestructable;
                if (destructable != null)
                    targets.Add(destructable);
            }

            var data = (Dictionary<string, object>)ability.userInfo;
            amount = Convert.ToInt32(data["amount"]);
            type = (DamageType)Enum.Parse(typeof(DamageType), (string)data["type"]);
        }
    }
}
