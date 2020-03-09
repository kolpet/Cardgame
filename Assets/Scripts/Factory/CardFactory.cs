using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Abilities.Targetting;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Factory
{
	public static class CardFactory
	{
		// Maps from a Card ID, to the Card's Data
		public static Dictionary<string, Dictionary<string, object>> Cards
		{
			get
			{
				if (_cards == null)
				{
					_cards = LoadDemoCollection();
				}
				return _cards;
			}
		}
		private static Dictionary<string, Dictionary<string, object>> _cards = null;

		private static Dictionary<string, Dictionary<string, object>> LoadDemoCollection()
		{
			var file = Resources.Load<TextAsset>("DemoCards");
			var dict = MiniJSON.Json.Deserialize(file.text) as Dictionary<string, object>;
			Resources.UnloadAsset(file);

			var array = (List<object>)dict["cards"];
			var result = new Dictionary<string, Dictionary<string, object>>();
			foreach (object entry in array)
			{
				var cardData = (Dictionary<string, object>)entry;
				var id = (string)cardData["id"];
				result.Add(id, cardData);
			}
			return result;
		}

		public static Card CreateCard(string id, int ownerIndex)
		{
			var cardData = Cards[id];
			Card card = CreateCard(cardData, ownerIndex);
			AddTarget(card, cardData);
			AddAbilities(card, cardData);
			AddMechanics(card, cardData);
			return card;
		}

		private static Card CreateCard(Dictionary<string, object> data, int ownerIndex)
		{
			var cardType = (string)data["type"];
			var type = Type.GetType("Assets.Scripts.Models.Cards." + cardType);
			var instance = Activator.CreateInstance(type) as Card;
			instance.Load(data);
			instance.ownerIndex = ownerIndex;
			return instance;
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

		private static void AddMechanics(Card card, Dictionary<string, object> data)
		{
			if (data.ContainsKey("taunt"))
			{
				//card.AddAspect<Taunt>();
			}
		}

		public static Card CreateFatigueCard(int ownerIndex, int fatigue)
		{
			var card = new CurseCard();
			card.ownerIndex = ownerIndex;
			card.name = "Fatigue";
			card.description = "Suffer " + fatigue + " damage";
			card.resource = ResourceType.Resourceless;
			card.cost = 0;

			var abilityData = new Dictionary<string, object> {
				{ "action", "DamageAction" },
				{ "info", new Dictionary<string, object>
				{
					{ "type", "True" },
					{ "amount", fatigue }
				}},
				{ "targetSelector", new Dictionary<string, object>
				{
					{ "type", "AllTarget" },
					{ "mark", new Dictionary<string, object>
					{
						{ "alliance", "Ally" },
						{ "zone", "Party" }
					}}
				}}
			};
			Ability ability = AddAbility(card, abilityData);
			AddSelector(ability, abilityData);

			return card;
		}
	}
}
