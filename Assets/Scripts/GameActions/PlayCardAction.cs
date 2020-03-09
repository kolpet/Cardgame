using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameActions
{
    public class PlayCardAction : GameAction
    {
        public Card card;

        public PlayCardAction(Card card) => this.card = card;
    }
}
