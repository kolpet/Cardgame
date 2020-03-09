using Assets.Scripts.Common;
using Assets.Scripts.Common.Animation;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Common.Pooling;
using Assets.Scripts.GameActions;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class HandView : MonoBehaviour
    {
        public List<Transform> cards = new List<Transform>();
        public Transform activeHandle;
        public Transform inactiveHandle;
        public float cardDistance;

        [Header("Timing")]

        public float cardDrawSpeed = 1f;
        public float cardDrawWait = 1f;
        public float cardLayoutSpeed = 0.25f;
        public float cardBurnSpeed = 0.5f;

        void OnEnable()
        {
            this.AddObserver(OnValidatePlayCard, Global.ValidateNotification<PlayCardAction>());
        }

        void OnDisable()
        {
            this.RemoveObserver(OnValidatePlayCard, Global.ValidateNotification<PlayCardAction>());
        }

        public IEnumerator AddCard(Transform card, bool showPreview, bool overDraw)
        {
            if (showPreview)
            {
                var preview = ShowPreview(card);
                while (preview.MoveNext())
                    yield return null;

                var tweener = card.Wait(cardDrawWait);
                while (tweener != null)
                    yield return null;
            }

            if (overDraw)
            {
                var discard = OverdrawCard(card);
                while (discard.MoveNext())
                    yield return null;
            }
            else
            {
                cards.Add(card);
                var layout = LayoutCards();
                while (layout.MoveNext())
                    yield return null;
            }
        }

        public IEnumerator ShowPreview(Transform card)
        {
            Tweener tweener = null;
            card.RotateTo(activeHandle.rotation);
            tweener = card.MoveTo(activeHandle.position, cardDrawSpeed, EasingEquations.EaseOutBack);
            var cardView = card.GetComponent<CardView>();
            while(tweener != null)
            {
                if (!cardView.isFaceUp)
                {
                    var toCard = (Camera.main.transform.position - card.position).normalized;
                    if (Vector3.Dot(card.forward, toCard) > 0)
                        cardView.Flip(true);
                }
                yield return null;
            }
            cardView.Flip(true);
        }

        public IEnumerator LayoutCards(bool animated = true)
        {
            var width = cardDistance * (cards.Count - 1);
            var xPos = -(width / 2f);
            var duration = animated ? cardLayoutSpeed : 0;

            Tweener tweener = null;
            for(int i = 0; i < cards.Count; i++)
            {
                var canvas = cards[i].GetComponentInChildren<Canvas>();
                canvas.sortingOrder = i;

                var position = inactiveHandle.position + new Vector3(xPos, 0, 0);
                cards[i].RotateTo(inactiveHandle.rotation, duration);
                tweener = cards[i].MoveTo(position, duration);
                xPos += cardDistance;
            }

            while (tweener != null)
                yield return null;
        }

        public CardView GetView(Card card)
        {
            foreach(Transform t in cards)
            {
                var cardView = t.GetComponent<CardView>();
                if (cardView.card == card)
                    return cardView;
            }
            return null;
        }

        public void Dismiss(CardView card)
        {
            cards.Remove(card.transform);

            card.gameObject.SetActive(false);
            card.transform.localScale = Vector3.one;

            var poolable = card.GetComponent<Poolable>();
            var pooler = GetComponentInParent<BoardView>().cardPooler;
            pooler.Enqueue(poolable);
        }

        IEnumerator OverdrawCard(Transform card)
        {
            Tweener tweener = card.ScaleTo(Vector3.zero, cardBurnSpeed, EasingEquations.EaseInBack);
            while(tweener != null) { yield return null; }

            Dismiss(card.GetComponent<CardView>());
        }

        private void OnValidatePlayCard(object sender, object args)
        {
            var action = sender as PlayCardAction;
            if (GetComponentInParent<PlayerView>().player.index == action.card.ownerIndex)
            {
                action.Perform.viewer = PlayCardViewer;
                action.Cancel.viewer = CancelPlayCardViewer;
            }
        }

        IEnumerator PlayCardViewer(IContainer game, GameAction action)
        {
            var playAction = action as PlayCardAction;
            CardView cardView = GetView(playAction.card);
            if (cardView == null)
                yield break;

            cards.Remove(cardView.transform);
            StartCoroutine(LayoutCards(true));
            var discard = OverdrawCard(cardView.transform);
            while(discard.MoveNext()) { yield return null; }
        }

        IEnumerator CancelPlayCardViewer(IContainer game, GameAction action)
        {
            var layout = LayoutCards(true);
            while(layout.MoveNext()) { yield return null; }
        }

        public GameObject GetMatch(Card card)
        {
            for (int i = cards.Count - 1; i >= 0; --i)
            {
                var view = cards[i].GetComponent<CardView>();
                if (view.card == card)
                    return view.gameObject;
            }
            return null;
        }
    }
}
