using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Common.Pooling.Poolers;
using Assets.Scripts.GameStates;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class BoardView : MonoBehaviour
    {
        public List<CombatantView> combatantViews;
        public SetPooler cardPooler;

        private void OnEnable()
        {
            this.AddObserver(OnNewMatchState, NewMatchState.EnterNotification);
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnNewMatchState, NewMatchState.EnterNotification);
        }

        private void OnNewMatchState(object sender, object args)
        {
            var match = GetComponentInParent<GameViewSystem>().Container.GetMatch();
            for (int i = 0; i < match.players.Count; i++)
            {
                combatantViews[i].SetCombatant(match.players[i]);
            }
        }

        public GameObject GetMatch(Card card)
        {
            var playerView = combatantViews[card.ownerIndex];
            return playerView.GetMatch(card);
        }
    }
}
