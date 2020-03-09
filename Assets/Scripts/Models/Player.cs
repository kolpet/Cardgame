using Assets.Scripts.Enums;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Models.Resources;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    public class Player : Combatant
    {
        public int maxDeck = 30;
        public int maxHand = 4;

        public bool finished = false;

        public Resource resource = new Mana();

        public List<Card> deck = new List<Card>(30);
        public List<Card> hand = new List<Card>(7);
        public List<Card> graveyard = new List<Card>();

        public int fatique = 0;

        public override List<Card> this[Zones zone]
        {
            get
            {
                switch (zone)
                {
                    case Zones.Party:
                        return party;
                    case Zones.Deck:
                        return deck;
                    case Zones.Hand:
                        return hand;
                    case Zones.Graveyard:
                        return graveyard;
                    default:
                        return null;
                }
            }
        }

        public Player(int index) : base(index) { }
    }
}
