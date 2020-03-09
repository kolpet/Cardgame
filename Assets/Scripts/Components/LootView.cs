using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.AssetManagement;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Common.Pooling;
using Assets.Scripts.Common.Pooling.Poolers;
using Assets.Scripts.GameStates;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components
{
    public class LootView : MonoBehaviour
    {
        public GameObject cardPrefab;
        public List<CardView> cards;

        LootSystem lootSystem;

        public void ShowLoot(LootState state)
        {
            var boardView = GetComponentInParent<BoardView>();
            lootSystem = state.Container.GetAspect<LootSystem>();

            for(int i = 0; i < lootSystem.loot.Count; i++)
            {
                var cardObj = boardView.cardPooler.Dequeue().gameObject;
                var cardView = cardObj.GetComponent<CardView>();
                cardView.card = lootSystem.loot[i];
                cardView.Flip(true);
                cardView.transform.ResetParent(transform);
                cardView.transform.position = transform.position + Vector3.right * ((i * 2f) - 2f);
                cardView.transform.rotation = transform.rotation;
                cardView.transform.localScale = transform.localScale;
                cardView.gameObject.SetActive(true);
                cardObj.LoadAssets();

                cards.Add(cardView);
            }
        }

        public void HideLoot(LootState state)
        {
            var pooler = GetComponentInParent<BoardView>().cardPooler;

            for (int i = cards.Count - 1; i >= 0; i--)
            {
                var card = cards[i];
                cards.Remove(card);

                card.gameObject.SetActive(false);
                card.transform.localScale = Vector3.one;

                var poolable = card.GetComponent<Poolable>();
                pooler.Enqueue(poolable);
            }
        }

        public void ChooseLoot(PointerEventData eventData)
        {
            var hover = eventData.pointerCurrentRaycast.gameObject;

            var view = (hover != null) ? hover.GetComponentInParent<CardView>() : null;
            if (cards.Contains(view))
            {
                lootSystem.Choose(view.card);
            }
        }
    }
}
