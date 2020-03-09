using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameActions
{
    public class DrawCardsAction : GameAction, IAbilityLoader
    {
        public int amount;
        public List<Card> cards;

        public DrawCardsAction()
        {

        }

        public DrawCardsAction(Player player, int amount)
        {
            Player = player;
            this.amount = amount;
        }

        public void Load(IContainer game, Ability ability)
        {
            Player = game.GetMatch().players[ability.Card.ownerIndex];
            amount = Convert.ToInt32(ability.userInfo);
        }
    }
}
