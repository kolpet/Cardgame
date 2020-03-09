using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Enums;
using Assets.Scripts.Factory;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Dungeons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class DungeonSystem : Aspect
    {
        public const string NewLevelNotification = "DungeonSystem.NewLevelNotification";

        public Dungeon dungeon;

        public void StartNewDungeon(string id)
        {
            dungeon = DungeonFactory.CreateDungeon(id);
        }

        public void AdvanceDungeon()
        {
            dungeon.currentLevel++;
        }

        public Encounter GetEncounter(int ownerIndex)
        {
            var valid = new List<DungeonEncounter>();
            int priority = 0;
            foreach(var dungeonMonster in dungeon.monsters)
            {
                if(dungeonMonster.min <= dungeon.currentLevel &&
                   dungeonMonster.max >= dungeon.currentLevel &&
                   dungeonMonster.priority >= priority)
                {
                    if (dungeonMonster.priority > priority)
                    {
                        priority = dungeonMonster.priority;
                        valid.Clear();
                    }
                    valid.Add(dungeonMonster);
                }
            }

            var choosen = valid.Random();
            var encounter = DungeonFactory.CreateEncounter(choosen.monsters, ownerIndex);

            return encounter;
        }
    }
}
