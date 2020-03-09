using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components
{
    public class VictoryView : MonoBehaviour
    {
        public LootView lootView;

        private void OnEnable()
        {
            this.AddObserver(OnEnterLootState, LootState.EnterNotification);
            this.AddObserver(OnExitLootState, LootState.ExitNotification);
            this.AddObserver(OnClickedNotification, Clickable.ClickedNotification);
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnEnterLootState, LootState.EnterNotification);
            this.RemoveObserver(OnExitLootState, LootState.ExitNotification);
            this.RemoveObserver(OnClickedNotification, Clickable.ClickedNotification);
        }

        private void OnEnterLootState(object sender, object args)
        {
            var lootState = sender as LootState;
            lootView.ShowLoot(lootState);
        }

        private void OnExitLootState(object sender, object args)
        {
            var lootState = sender as LootState;
            lootView.HideLoot(lootState);
        }

        private void OnClickedNotification(object sender, object args)
        {
            var eventData = args as PointerEventData;
            lootView.ChooseLoot(eventData);
        }
    }
}
