using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Models.Cards;
using System.Collections;
using UnityEngine;


namespace Assets.Scripts.Components
{
	public class TestDealDamageView : MonoBehaviour
	{
		public TestProjectileView prefab;

		void OnEnable()
		{
			this.AddObserver(OnPrepareDamage, Global.PrepareNotification<DamageAction>());
		}

		void OnDisable()
		{
			this.RemoveObserver(OnPrepareDamage, Global.PrepareNotification<DamageAction>());
		}

		void OnPrepareDamage(object sender, object args)
		{
			var action = args as DamageAction;
			action.Perform.viewer = DamageViewer;
		}

		private IEnumerator DamageViewer(IContainer game, GameAction action)
		{
			var damageAction = action as DamageAction;
			var boardView = GetComponent<BoardView>();
			var originView = boardView.combatantViews[0].party.characters[0];

			for(int i = 0; i < damageAction.targets.Count; i++)
			{
				var targetView = boardView.GetMatch(damageAction.targets[i] as Card);

				var projectile = Instantiate(prefab);
				var fly = projectile.FlyToTarget(originView.transform, targetView.transform);
				while (fly.MoveNext())
				{
					yield return null;
				}
				Destroy(projectile);
			}
			yield return true;
		}
	}
}