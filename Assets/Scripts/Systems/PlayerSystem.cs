using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.Factory;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Systems
{
    public class PlayerSystem : Aspect, IObserve
    {
        public const string DeckShuffledNotification = "PlayerSystem.DeckShuffledNotification";
        public const string DeckChangedNotification = "PlayerSystem.DeckChangedNotification";
        public const string GraveyardChangedNotification = "PlayerSystem.GraveyardChangedNotification";

        public void Awake()
        {
            this.AddObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), Container);
            this.AddObserver(OnPerformNextTurn, Global.PerformNotification<NextTurnAction>(), Container);
            this.AddObserver(OnPerformDrawCards, Global.PerformNotification<DrawCardsAction>(), Container);
            this.AddObserver(OnPerformFatigue, Global.PerformNotification<FatigueAction>(), Container);
            this.AddObserver(OnPerformOverDraw, Global.PerformNotification<OverdrawAction>(), Container);
            this.AddObserver(OnPerformBurnCards, Global.PerformNotification<BurnCardsAction>(), Container);
            this.AddObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>(), Container);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), Container);
            this.RemoveObserver(OnPerformNextTurn, Global.PerformNotification<NextTurnAction>(), Container);
            this.RemoveObserver(OnPerformDrawCards, Global.PerformNotification<DrawCardsAction>(), Container);
            this.RemoveObserver(OnPerformFatigue, Global.PerformNotification<FatigueAction>(), Container);
            this.RemoveObserver(OnPerformOverDraw, Global.PerformNotification<OverdrawAction>(), Container);
            this.RemoveObserver(OnPerformBurnCards, Global.PerformNotification<BurnCardsAction>(), Container);
            this.RemoveObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>(), Container);
        }

        void OnPerformChangeTurn(object sender, object args)
        {
            /*
            var action = args as ChangeTurnAction;
            var match = Container.GetAspect<DataSystem>().match;
            if (match.players[action.targetPlayerIndex] is Player player)
                DrawCards(player, 1);*/
        }

        void OnPerformNextTurn(object sender, object args)
        {
            var action = args as NextTurnAction;
            var match = Container.GetAspect<DataSystem>().match;
            if (match.players[action.targetPlayerIndex] is Player player)
            {
                var missing = player.maxHand - player.hand.Count;
                DrawCards(player, missing);
            }
        }

        void OnPerformDrawCards(object sender, object args)
        {
            var action = args as DrawCardsAction;
            var player = action.Player as Player;
            int deckCount = player[Zones.Deck].Count;

            action.cards = new List<Card>();
            for (int i = 0; i < action.amount; i++)
            {
                if(player.hand.Count < player.maxHand) //Has space in hand
                {
                    if(player.deck.Count == 0) //Deck is empty
                    {
                        if(player.graveyard.Count == 0) //Graveyard empty, draw fatigue
                        {
                            var fatigueAction = new FatigueAction(player);
                            Container.AddReaction(fatigueAction);
                        }
                        else //Shuffle graveyard into deck
                        {
                            for (int j = player.graveyard.Count - 1; j >= 0; j--)
                                ChangeZone(player.graveyard[j], Zones.Deck);
                            this.PostNotification(DeckShuffledNotification, player);
                        }
                    }
                    if (player.deck.Count > 0) //Draw
                    {
                        var draw = player[Zones.Deck].Draw(1)[0];
                        action.cards.Add(draw);
                        ChangeZone(draw, Zones.Hand);
                        this.PostNotification(DeckChangedNotification, player);
                    }
                }
                else //Overdraw
                {
                    var overDrawAction = new OverdrawAction(player, 1);
                    Container.AddReaction(overDrawAction);
                }
            }
        }

        void OnPerformFatigue(object sender, object args)
        {
            var action = args as FatigueAction;
            var player = action.Player as Player;
            player.fatique++;

            var card = CardFactory.CreateFatigueCard(player.index, player.fatique);
            ChangeZone(card, Zones.Deck);

            var reaction = new DrawCardsAction(player, 1);
            Container.AddReaction(reaction);

            /*var damageTarget = action.Player.party[0] as IDestructable;
            var damageAction = new DamageAction(damageTarget, player.fatique);
            Container.AddReaction(damageAction);*/
        }

        void OnPerformOverDraw(object sender, object args)
        {
            var action = args as OverdrawAction;
            action.cards = action.Player[Zones.Deck].Draw(action.amount);
            foreach (Card card in action.cards)
                ChangeZone(card, Zones.Graveyard);

            this.PostNotification(GraveyardChangedNotification, action.Player);
        }

        void OnPerformBurnCards(object sender, object args)
        {
            var action = args as BurnCardsAction;
            foreach (Card card in action.cards)
            {
                var cardSystem = Container.GetAspect<CardSystem>();
                cardSystem.BurnCard(card);

                var player = Container.GetAspect<DataSystem>().match.players[card.ownerIndex];
                //this.PostNotification(GraveyardChangedNotification, player);
            }
        }

        void OnPerformPlayCard(object sender, object args)
        {
            var action = args as PlayCardAction;
            var player = Container.GetAspect<DataSystem>().match.players[action.card.ownerIndex];
            ChangeZone(action.card, Zones.Graveyard);
            this.PostNotification(GraveyardChangedNotification, player);
        }

        void DrawCards(Player player, int amount)
        {
            var action = new DrawCardsAction(player, amount);
            Container.AddReaction(action);
        }

        void ChangeZone(Card card, Zones zone, Player toPlayer = null)
        {
            var cardSystem = Container.GetAspect<CardSystem>();
            cardSystem.ChangeZone(card, zone, toPlayer);
        }
    }
}
