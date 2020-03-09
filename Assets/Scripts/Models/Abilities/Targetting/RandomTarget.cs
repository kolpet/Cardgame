using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Systems;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Abilities.Targetting
{
	public class RandomTarget : Aspect, ITargetSelector
	{
		public Mark mark;
		public int count = 1;

		public List<Card> SelectTargets(IContainer game)
		{
			var result = new List<Card>();
			var system = game.GetAspect<TargetSystem>();
			var card = (Container as Ability).Card;
			var marks = system.GetMarks(card, mark);
			if (marks.Count == 0)
				return result;
			for (int i = 0; i < count; ++i)
			{
				result.Add(marks.Random());
			}
			return result;
		}

		public void Load(Dictionary<string, object> data)
		{
			var markData = (Dictionary<string, object>)data["mark"];
			mark = new Mark(markData);
			count = System.Convert.ToInt32(data["count"]);
		}
	}
}
