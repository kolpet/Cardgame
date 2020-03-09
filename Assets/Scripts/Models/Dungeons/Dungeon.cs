using Assets.Scripts.Common.AspectContainer;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Dungeons
{
    public class Dungeon : Container
    {
        public string id;
        public string name;
        public int currentLevel;
        public List<DungeonEncounter> monsters;

        public Dungeon()
        {
            monsters = new List<DungeonEncounter>();
        }

        public virtual void Load(Dictionary<string, object> data)
        {
            id = (string)data["id"];

            var results = (List<object>)data["monsters"];
            foreach(object entry in results)
            {
                var monsterData = (Dictionary<string, object>)entry;
                var monster = new DungeonEncounter();
                monster.Load(monsterData);
                monsters.Add(monster);
            }
        }
    }
}
