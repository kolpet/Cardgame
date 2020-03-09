using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class DestructableSystem : Aspect, IObserve
    {
		public void Awake()
		{
			this.AddObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>(), Container);
			this.AddObserver(OnPerformHealAction, Global.PerformNotification<HealAction>(), Container);
		}

		public void Destroy()
		{
			this.RemoveObserver(OnPerformDamageAction, Global.PerformNotification<DamageAction>(), Container);
			this.RemoveObserver(OnPerformHealAction, Global.PerformNotification<HealAction>(), Container);
		}

		private void OnPerformDamageAction(object sender, object args)
		{
			var action = args as DamageAction;
			foreach(IDestructable target in action.targets)
			{
				var resistance = target as IResistant;
				switch (action.type)
				{
					case DamageType.Physical: //Physical damage is reduced by armor
						var block = resistance != null ? resistance.Armor : 0;
						target.HitPoints -= Math.Max(action.amount - block, 0);
						break;
					case DamageType.Magical: //Magical damage is reduced by magic resist
						var resist = resistance != null ? resistance.MagicResist : 0;
						target.HitPoints -= Math.Max(action.amount - resist, 0);
						break;
					case DamageType.Poison: //Poison can only bring target down to 1 health
						target.HitPoints = Math.Max(target.HitPoints - action.amount, 1);
						break;
					case DamageType.True: //True damage cannot be reduced
						target.HitPoints -= action.amount;
						break;
				}
			}
		}

		private void OnPerformHealAction(object sender, object args)
		{
			var action = args as HealAction;
			foreach(IDestructable target in action.targets)
			{
				target.HitPoints = Math.Min(target.HitPoints + action.amount, target.MaxHitPoints);
			}
		}
	}
}
