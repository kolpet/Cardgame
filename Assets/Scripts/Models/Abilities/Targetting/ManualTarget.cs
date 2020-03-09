using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Cards;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Abilities.Targetting
{
    public class ManualTarget : Aspect, ITargetSelector
    {
		public List<Card> SelectTargets (IContainer game) {
			var card = (Container as Ability).Card;
			var target = card.GetAspect<Target>();
			var result = new List<Card>();
			result.Add(target.selected);
			return result;
		}

		public void Load(Dictionary<string, object> data)
		{

		}
	}
}
