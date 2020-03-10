using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.AssetManagement;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Models;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class DrawCardsView : MonoBehaviour
    {
        private void OnEnable()
        {
            this.AddObserver(OnPrepareDrawCards, Global.PrepareNotification<DrawCardsAction>());
            this.AddObserver(OnPrepareDrawCards, Global.PrepareNotification<OverdrawAction>());
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnPrepareDrawCards, Global.PrepareNotification<DrawCardsAction>());
            this.RemoveObserver(OnPrepareDrawCards, Global.PrepareNotification<OverdrawAction>());
        }

        private void OnPrepareDrawCards(object sender, object args)
        {
            var action = args as DrawCardsAction;
            action.Perform.viewer = DrawCardsViewer;
        }

        private IEnumerator DrawCardsViewer(IContainer game, GameAction action)
        {
            yield return true; // perform the action logic so that we know what cards have been drawn
            var drawAction = action as DrawCardsAction;
            var boardView = GetComponent<BoardView>();
            var playerView = boardView.combatantViews[drawAction.Player.index] as PlayerView;

            for(int i = 0; i < drawAction.cards.Count; i++)
            {
                //int deckSize = action.player[Zones.Deck].Count + drawAction.cards.Count - (i + 1);
                //playerView.deck.ShowDeckSize((float)deckSize / (float)Player.maxDeck);

                var cardObj = boardView.cardPooler.Dequeue();
                var cardView = cardObj.GetComponent<CardView>();
                cardView.card = drawAction.cards[i];
                cardView.Flip(false);
                cardView.transform.ResetParent(playerView.hand.transform);
                cardView.transform.position = playerView.deck.topCard.position;
                cardView.transform.rotation = playerView.deck.topCard.rotation;
                cardView.transform.localScale = playerView.deck.topCard.localScale;
                cardView.gameObject.SetActive(true);
                cardObj.gameObject.LoadAssets();

                var showPreview = (action.Player as Player).mode == ControlModes.Local;
                var overDraw = action is OverdrawAction;
                var addCard = playerView.hand.AddCard(cardView.transform, showPreview, overDraw);
                while(addCard.MoveNext()) { yield return null; }
                yield return true;
            }
        }
    }
}
