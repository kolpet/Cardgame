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
    public class CombatantView : MonoBehaviour
    {
        public PartyView party;

        public Combatant Combatant { get; protected set; }

        public void SetCombatant(Combatant combatant)
        {
            this.Combatant = combatant;
            for (int i = 0; i < combatant.party.Count; i++)
            {
                var card = combatant.party[i];
                party.characters[i].card = card;
            }
        }

        public GameObject GetMatch(Card card)
        {
            switch (card.zone)
            {
                case Zones.Party:
                    return party.GetMatch(card);
                default:
                    Debug.Log("No implementation for that zone");
                    return null;
            }
        }
    }
}
