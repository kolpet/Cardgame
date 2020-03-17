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
    public class DiscardCardsView : MonoBehaviour
    {
        public Material smokeMaterial;
        public float smokeTime = 1f;

        private void OnEnable()
        {
            this.AddObserver(OnPrepareDiscardCards, Global.PrepareNotification<DiscardCardsAction>());
        }

        private void OnDisable()
        {
            this.AddObserver(OnPrepareDiscardCards, Global.PrepareNotification<DiscardCardsAction>());
        }

        private void OnPrepareDiscardCards(object sender, object args)
        {
            var action = args as DiscardCardsAction;
            action.Perform.viewer = DiscardCardsViewer;
        }

        private IEnumerator DiscardCardsViewer(IContainer game, GameAction action)
        {
            yield return true; // perform the action logic so that we know what cards have been drawn
            var burnAction = action as DiscardCardsAction;
            if (burnAction.cards.Count == 0)
                yield break;
            var boardView = GetComponent<BoardView>();
            var playerView = boardView.combatantViews[burnAction.cards[0].ownerIndex] as PlayerView;

            foreach(var card in burnAction.cards)
            {
                var result = playerView.hand.GetMatch(card);
                if (result != null)
                {
                    var view = result.GetComponent<CardView>();
                    var burnAnimation = DiscardCard(view);

                    while (burnAnimation.MoveNext()) { yield return null; }
                    playerView.hand.Dismiss(view);

                    foreach (var graphic in view.GetComponentsInChildren<Graphic>())
                        graphic.material = graphic.defaultMaterial;
                }
            }
        }

        private IEnumerator DiscardCard(CardView view)
        {
            var materials = new List<Material>();
            var graphics = view.GetComponentsInChildren<Graphic>();
            foreach (var graphic in graphics)
            {
                var material = new Material(smokeMaterial);
                graphic.material = material;
                material.mainTexture = graphic.mainTexture;
                materials.Add(material);
            }

            float burnProgress = 0f;
            while(burnProgress < smokeTime)
            {
                burnProgress += smokeTime * Time.deltaTime;
                foreach(var material in materials)
                    material.SetFloat("_SliceAmount", burnProgress);
                yield return null;
            }
        }
    }
}
