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
	public static class DeckFactory
	{
		public static List<Card> CreateDeck(string fileName, int ownerIndex)
		{
			var file = Resources.Load<TextAsset>(fileName);
			var contents = MiniJSON.Json.Deserialize(file.text) as Dictionary<string, object>;
			Resources.UnloadAsset(file);

			var array = (List<object>)contents["deck"];
			var result = new List<Card>();
			foreach (object item in array)
			{
				var id = (string)item;
				var card = CardFactory.CreateCard(id, ownerIndex);
				result.Add(card);
			}
			return result;
		}
	}
}
