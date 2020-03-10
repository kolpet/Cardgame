using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class DestructableSystem : Aspect, IObserve
    {
		IEnumerator enumerator;

		public void Awake()
		{
			this.AddObserver(OnPrepareDamageAction, Global.PrepareNotification<DamageAction>(), Container);
			this.AddObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>(), Container);
			this.AddObserver(OnPrepareHealAction, Global.PrepareNotification<HealAction>(), Container);
			this.AddObserver(OnPerformHealAction, Global.PerformNotification<HealAction>(), Container);
		}

		public void Destroy()
		{
			this.RemoveObserver(OnPrepareDamageAction, Global.PrepareNotification<DamageAction>(), Container);
			this.RemoveObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>(), Container);
			this.RemoveObserver(OnPrepareHealAction, Global.PrepareNotification<HealAction>(), Container);
			this.RemoveObserver(OnPerformHealAction, Global.PerformNotification<HealAction>(), Container);
		}

		private void OnPrepareDamageAction(object sender, object args)
		{
			var action = args as DamageAction;
			enumerator = DamageAction(action);
		}

		private void OnPerformDamageAction(object sender, object args)
		{
			enumerator.MoveNext();
		}

		private void OnPrepareHealAction(object sender, object args)
		{
			var action = args as HealAction;
			enumerator = HealAction(action);
		}

		private void OnPerformHealAction(object sender, object args)
		{
			enumerator.MoveNext();
		}

		IEnumerator DamageAction(DamageAction action)
		{
			foreach (IDestructable target in action.targets)
			{
				var damage = 0;
				var resistance = target as IResistant;
				switch (action.type)
				{
					case DamageType.Physical: //Physical damage is reduced by armor
						var block = resistance != null ? resistance.Armor : 0;
						damage = Math.Max(action.amount - block, 0);
						break;
					case DamageType.Magical: //Magical damage is reduced by magic resist
						var resist = resistance != null ? resistance.MagicResist : 0;
						damage = Math.Max(action.amount - resist, 0);
						break;
					case DamageType.Poison: //Poison can only bring target down to 1 health
						var hitPoints = Math.Max(target.HitPoints - action.amount, 1);
						damage = target.HitPoints - hitPoints;
						break;
					case DamageType.True: //True damage cannot be reduced
						damage = action.amount;
						break;
				}
				target.HitPoints -= damage;
				action.amount = damage;
				yield return null;
			}
		}

		IEnumerator HealAction(HealAction action)
		{
			foreach (IDestructable target in action.targets)
			{
				target.HitPoints = Math.Min(target.HitPoints + action.amount, target.MaxHitPoints);
				yield return null;
			}
		}
	}
}
