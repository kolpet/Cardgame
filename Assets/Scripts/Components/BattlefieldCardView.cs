using Assets.Scripts.Common;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameActions;
using Assets.Scripts.GameStates;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public abstract class BattlefieldCardView : MonoBehaviour
    {
        public Image avatar;
		public Text health;
		public Sprite inactive;
		public Sprite active;
		protected bool isActive;

        public abstract Card card { get; set; }

		void OnEnable()
		{
			this.AddObserver(OnPlayerIdleEnter, PlayerIdleState.EnterNotification);
			this.AddObserver(OnPlayerIdleExit, PlayerIdleState.ExitNotification);
		}

		void OnDisable()
		{
			this.RemoveObserver(OnPlayerIdleEnter, PlayerIdleState.EnterNotification);
			this.RemoveObserver(OnPlayerIdleExit, PlayerIdleState.ExitNotification);
		}

		void OnPlayerIdleEnter(object sender, object args)
		{
			var container = (sender as PlayerIdleState).Container;
			isActive = false;//container.GetAspect<AttackSystem>().validAttackers.Contains(card);
			Refresh();
		}

		void OnPlayerIdleExit(object sender, object args)
		{
			isActive = false;
		}

		public abstract void Refresh();
	}
}
