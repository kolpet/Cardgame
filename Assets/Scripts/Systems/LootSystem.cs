using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Common.StateMachine;
using Assets.Scripts.Factory;
using Assets.Scripts.GameStates;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class LootSystem : Aspect
    {
        const int lootAmount = 3;
        public List<Card> loot;

        public void Refresh()
        {
            var player = Container.GetMatch().CurrentPlayer as Player;
            var cardIds = CardFactory.Cards.Keys.ToList();
            cardIds.RemoveAll(card => card.Contains("Monster"));
            var drawIds = cardIds.Draw(lootAmount);

            loot = new List<Card>();
            foreach (var id in drawIds)
            {
                var card = CardFactory.CreateCard(id, player.index);
                loot.Add(card);
            }
        }

        public void Choose(Card card)
        {
            if (!loot.Contains(card))
                return;

            var player = Container.GetMatch().CurrentPlayer as Player;
            player.deck.Add(card);
            this.PostNotification(PlayerSystem.DeckChangedNotification);

            Container.ChangeState<NewMatchState>();
        }
    }
}
