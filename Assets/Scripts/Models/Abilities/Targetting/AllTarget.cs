using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Systems;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Abilities.Targetting
{
	public class AllTarget : Aspect, ITargetSelector
	{
		public Mark mark;

		public List<Card> SelectTargets(IContainer game)
		{
			var result = new List<Card>();
			var system = game.GetAspect<TargetSystem>();
			var card = (Container as Ability).Card;
			var marks = system.GetMarks(card, mark);
			result.AddRange(marks);
			return result;
		}

		public void Load(Dictionary<string, object> data)
		{
			var markData = (Dictionary<string, object>)data["mark"];
			mark = new Mark(markData);
		}
	}
}