using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Models.Dungeons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Factory
{
    public static class DungeonFactory
    {
		public static Dictionary<string, Dictionary<string, object>> Dungeons
		{
			get
			{
				if (dungeons == null)
				{
					dungeons = LoadDemoDungeon();
				}
				return dungeons;
			}
		}

		private static Dictionary<string, Dictionary<string, object>> dungeons = null;

		private static Dictionary<string, Dictionary<string, object>> LoadDemoDungeon()
		{
			var file = Resources.Load<TextAsset>("DemoDungeons");
			var dict = MiniJSON.Json.Deserialize(file.text) as Dictionary<string, object>;
			Resources.UnloadAsset(file);

			var array = (List<object>)dict["dungeons"];
			var result = new Dictionary<string, Dictionary<string, object>>();
			foreach(object entry in array)
			{
				var cardData = (Dictionary<string, object>)entry;
				var id = (string)cardData["id"];
				result.Add(id, cardData);
			}
			return result;
		}

		public static Dungeon CreateDungeon(string id)
		{
			var dungeonData = Dungeons[id];
			Dungeon dungeon = new Dungeon();
			dungeon.currentLevel = 0;
			dungeon.Load(dungeonData);

			return dungeon;
		}

		public static Encounter CreateEncounter(List<string> ids, int ownerIndex)
		{
			var enemy = new Encounter(ownerIndex);

			foreach (string id in ids)
			{
				var monster = MonsterFactory.CreateMonster(id, enemy.index);
				monster.ownerIndex = enemy.index;
				monster.zone = Zones.Party;
				enemy.party.Add(monster);
			}

			return enemy;
		}
	}
}
