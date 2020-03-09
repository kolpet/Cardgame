using Assets.Scripts.Common;
using Assets.Scripts.Common.Animation;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class BurnCardsView : MonoBehaviour
    {
        public Material burnMaterial;
        public float burnTime = 1f;

        private void OnEnable()
        {
            this.AddObserver(OnPrepareBurnCards, Global.PrepareNotification<BurnCardsAction>());
        }

        private void OnDisable()
        {
            this.AddObserver(OnPrepareBurnCards, Global.PrepareNotification<BurnCardsAction>());
        }

        private void OnPrepareBurnCards(object sender, object args)
        {
            var action = args as BurnCardsAction;
            action.Perform.viewer = BurnCardsViewer;
        }

        private IEnumerator BurnCardsViewer(IContainer game, GameAction action)
        {
            yield return true; // perform the action logic so that we know what cards have been drawn
            var burnAction = action as BurnCardsAction;
            var boardView = GetComponent<BoardView>();
            var playerView = boardView.combatantViews[burnAction.cards[0].ownerIndex] as PlayerView;

            foreach(var card in burnAction.cards)
            {
                var result = playerView.hand.GetMatch(card);
                if (result != null &&
                   card.zone == Zones.Hand)
                {
                    var view = result.GetComponent<CardView>();
                    var burnAnimation = BurnCard(view);

                    while (burnAnimation.MoveNext()) { yield return null; }
                    playerView.hand.Dismiss(view);

                    foreach (var graphic in view.GetComponentsInChildren<Graphic>())
                        graphic.material = graphic.defaultMaterial;
                }
            }
        }

        private IEnumerator BurnCard(CardView view)
        {
            var materials = new List<Material>();
            var graphics = view.GetComponentsInChildren<Graphic>();
            foreach (var graphic in graphics)
            {
                var material = new Material(burnMaterial);
                graphic.material = material;
                material.mainTexture = graphic.mainTexture;
                materials.Add(material);
            }

            float burnProgress = 0f;
            while(burnProgress < burnTime)
            {
                burnProgress += burnTime * Time.deltaTime;
                foreach(var material in materials)
                    material.SetFloat("_SliceAmount", burnProgress);
                yield return null;
            }
        }
    }
}
