using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Factory
{
    public static class MonsterFactory
    {
		public static Dictionary<string, Dictionary<string, object>> Monsters
		{
			get
			{
				if (monsters == null)
				{
					monsters = LoadDemoMonster();
				}
				return monsters;
			}
		}

		private static Dictionary<string, Dictionary<string, object>> monsters = null;

		private static Dictionary<string, Dictionary<string, object>> LoadDemoMonster()
		{
			var file = Resources.Load<TextAsset>("DemoMonsters");
			var dict = MiniJSON.Json.Deserialize(file.text) as Dictionary<string, object>;
			Resources.UnloadAsset(file);

			var array = (List<object>)dict["monsters"];
			var result = new Dictionary<string, Dictionary<string, object>>();
			foreach(object entry in array)
			{
				var cardData = (Dictionary<string, object>)entry;
				var id = (string)cardData["id"];
				result.Add(id, cardData);
			}
			return result;
		}

		public static Monster CreateMonster(string id, int ownerIndex)
		{
			var monsterData = Monsters[id];
			Monster monster = CreateMonster(monsterData, ownerIndex);

			return monster;
		} 

		private static Monster CreateMonster(Dictionary<string, object> data, int ownerIndex)
		{
			var monster = new Monster();
			monster.Load(data);

			var cards = (List<object>)data["cards"];
			foreach(object entry in cards)
			{
				var cardData = (Dictionary<string, object>)entry;
				var card = CreateMonsterCard(cardData, ownerIndex);

				monster.monsterCards.Add(card);
			}
			return monster;
		}

		private static MonsterCard CreateMonsterCard(Dictionary<string, object> data, int ownerIndex)
		{
			var card = new MonsterCard();
			card.Load(data);
			card.ownerIndex = ownerIndex;
			AddTarget(card, data);
			AddAbilities(card, data);
			return card;
		}

		private static void AddTarget(Card card, Dictionary<string, object> data)
		{
			if (data.ContainsKey("target") == false)
				return;
			var targetData = (Dictionary<string, object>)data["target"];
			var target = card.AddAspect<Target>();
			var allowedData = (Dictionary<string, object>)targetData["allowed"];
			target.allowed = new Mark(allowedData);
			var preferredData = allowedData;
			if (targetData.ContainsKey("preferred"))
				preferredData = (Dictionary<string, object>)targetData["preferred"];
			target.preferred = new Mark(preferredData);
		}

		private static void AddAbilities(Card card, Dictionary<string, object> data)
		{
			if (data.ContainsKey("abilities") == false)
				return;
			var abilities = (List<object>)data["abilities"];
			foreach (object entry in abilities)
			{
				var abilityData = (Dictionary<string, object>)entry;
				Ability ability = AddAbility(card, abilityData);
				AddSelector(ability, abilityData);
			}
		}

		private static Ability AddAbility(Card card, Dictionary<string, object> data)
		{
			var key = card.GetAspects<Ability>().Count.ToString();
			var ability = card.AddAspect<Ability>(key);
			ability.ActionName = (string)data["action"];
			if (data.ContainsKey("info"))
				ability.userInfo = data["info"];
			return ability;
		}

		private static void AddSelector(Ability ability, Dictionary<string, object> data)
		{
			if (data.ContainsKey("targetSelector") == false)
				return;
			var selectorData = (Dictionary<string, object>)data["targetSelector"];
			var typeName = (string)selectorData["type"];
			var type = Type.GetType("Assets.Scripts.Models.Abilities.Targetting." + typeName);
			var instance = Activator.CreateInstance(type) as ITargetSelector;
			instance.Load(selectorData);
			ability.AddAspect<ITargetSelector>(instance);
		}
	}
}
