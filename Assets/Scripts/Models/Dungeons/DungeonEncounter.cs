using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Dungeons
{
    public class DungeonEncounter
    {
        public List<string> monsters;
        public int min = -1;
        public int max = int.MaxValue;
        public int priority = 0;

        public DungeonEncounter()
        {
            monsters = new List<string>();
        }

        public virtual void Load(Dictionary<string, object> data)
        {
            if (data.ContainsKey("min"))
                min = Convert.ToInt32(data["min"]);
            if (data.ContainsKey("max"))
                max = Convert.ToInt32(data["max"]);
            if (data.ContainsKey("priority"))
                priority = Convert.ToInt32(data["priority"]);

            var ids = (List<object>)data["ids"];
            foreach (var id in ids)
                monsters.Add((string)id);
        }
    }
}
