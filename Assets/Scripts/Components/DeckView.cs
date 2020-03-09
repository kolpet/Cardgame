using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameStates;
using Assets.Scripts.Models;
using Assets.Scripts.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class DeckView : MonoBehaviour
    {
        public Transform topCard;
        public Text deckText;

        void OnEnable()
        {
            this.AddObserver(OnDeckShuffledNotification, PlayerSystem.DeckShuffledNotification);
            this.AddObserver(OnDeckChangedNotification, PlayerSystem.DeckChangedNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnDeckShuffledNotification, PlayerSystem.DeckShuffledNotification);
            this.RemoveObserver(OnDeckChangedNotification, PlayerSystem.DeckChangedNotification);
        }

        private void OnDeckShuffledNotification(object sender, object args)
        {
            Refresh(args);
        }

        private void OnDeckChangedNotification(object sender, object args)
        {
            Refresh(args);
        }

        private void Refresh(object args)
        {
            var player = args as Player;
            if (player == null)
                return;

            deckText.text = player.deck.Count.ToString();
        }
    }
}