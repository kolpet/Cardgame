using Assets.Scripts.Enums;
using Assets.Scripts.Models.Cards;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    public abstract class Combatant
    {
        public int index;
        public ControlModes mode = ControlModes.Computer;

        public List<Card> party = new List<Card>();

        public abstract List<Card> this[Zones zone] { get; }

        public Combatant(int index)
        {
            this.index = index;
        }
    }
}
