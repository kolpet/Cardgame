using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameStates;
using Assets.Scripts.Models;
using Assets.Scripts.Systems;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class GraveyardView : MonoBehaviour
    {
        public Text graveyardText;

        void OnEnable()
        {
            this.AddObserver(OnDeckShuffledNotification, PlayerSystem.DeckShuffledNotification);
            this.AddObserver(OnGraveyardChangedNotification, PlayerSystem.GraveyardChangedNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnDeckShuffledNotification, PlayerSystem.DeckShuffledNotification);
            this.RemoveObserver(OnGraveyardChangedNotification, PlayerSystem.GraveyardChangedNotification);
        }

        private void OnDeckShuffledNotification(object sender, object args)
        {
            Refresh(args);
        }

        private void OnGraveyardChangedNotification(object sender, object args)
        {
            Refresh(args);
        }

        private void Refresh(object args)
        {
            var player = args as Player;
            if (player == null)
                return;

            graveyardText.text = player.graveyard.Count.ToString();
        }
    }
}