using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Systems
{
    public class PassiveCardSystem : Aspect, IObserve
    {
        public List<Card> passives = new List<Card>();

        public void Awake()
        {
            //this.AddObserver(OnPrepareNextTurn, Global.PrepareNotification<NextTurnAction>());
            //this.AddObserver(OnPerformNextTurn, Global.PerformNotification<NextTurnAction>());
            this.AddObserver(OnDeckChanged, PlayerSystem.DeckChangedNotification);
        }


        public void Destroy()
        {
            //this.RemoveObserver(OnPrepareNextTurn, Global.PrepareNotification<NextTurnAction>());
            //this.RemoveObserver(OnPerformNextTurn, Global.PerformNotification<NextTurnAction>());
            this.RemoveObserver(OnDeckChanged, PlayerSystem.DeckChangedNotification);
        }

        private void OnDeckChanged(object sender, object args)
        {
            passives.Clear();

            //var action = args as NextTurnAction;
            var player = Container.GetMatch().CurrentPlayer as Player;
            if (player == null)
                return;
            foreach (var card in player.hand)
                if (card.Type == CardType.Passive)
                    passives.Add(card);

            foreach (var card in player.deck)
                if (card.Type == CardType.Passive)
                    passives.Add(card);

            foreach (var card in player.graveyard)
                if (card.Type == CardType.Passive)
                    passives.Add(card);
        }

        private void OnPrepareNextTurn(object sender, object args)
        {
            
        }

        private void OnPerformNextTurn(object sender, object args)
        {
            foreach (var card in passives)
            {
                if (card == null)
                    return;

                var abilities = card.GetAspects<Ability>();
                if (abilities != null)
                {
                    foreach (var ability in abilities)
                    {
                        var reaction = new AbilityAction(ability);
                        reaction.Priority = 5;
                        Container.AddReaction(reaction);
                    }
                }
            }
        }
    }
}
