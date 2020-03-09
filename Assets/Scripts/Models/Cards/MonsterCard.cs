using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Cards
{
    public class MonsterCard : Card
    {
        public int turnsRemaining;
        public int frequency;
        public override CardType Type => CardType.Monster;

        public override void Load(Dictionary<string, object> data)
        {
            id = (string)data["id"];
            frequency = Convert.ToInt32(data["frequency"]);
            turnsRemaining = frequency;
        }
    }
}
