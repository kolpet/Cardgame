using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class PlayerView : CombatantView
    {
        public DeckView deck;
        public GraveyardView graveyard;
        public HandView hand;
        public GameObject cardPrefab;

        public Player player => Combatant as Player;
    }
}
