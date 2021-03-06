﻿using Assets.Scripts.Common.AspectContainer;
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
    public class DiscardCardsAction : GameAction, IAbilityLoader
    {
        public List<Card> cards;

        public DiscardCardsAction() { }

        public DiscardCardsAction(Card card) => cards = new List<Card> { card };

        public DiscardCardsAction(List<Card> cards) => this.cards = cards;

        public void Load(IContainer game, Ability ability)
        {
            var targetSelector = ability.GetAspect<ITargetSelector>();
            var targets = targetSelector.SelectTargets(game);
            cards = new List<Card>();
            foreach (Card card in targets)
            {
                cards.Add(card);
            }
        }
    }
}
