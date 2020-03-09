using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class PartyView : MonoBehaviour
    {
        public List<BattlefieldCardView> characters = new List<BattlefieldCardView>();

        private void Awake()
        {
            
        }

        public GameObject GetMatch(Card card)
        {
            for (int i = characters.Count - 1; i >= 0; --i)
            {
                if (characters[i].card == card)
                    return characters[i].gameObject;
            }
            return null;
        }
    }
}
