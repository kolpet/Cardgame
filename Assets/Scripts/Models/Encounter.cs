using Assets.Scripts.Enums;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Encounter : Combatant
    {
        public override List<Card> this[Zones zone]
        {
            get
            {
                switch (zone)
                {
                    case Zones.Party:
                        return party;
                    default:
                        return null;
                }
            }
        }

        public Encounter(int index) : base(index) { }
    }
}
