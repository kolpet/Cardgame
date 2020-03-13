using Assets.Scripts.Models.Cards;
using Assets.Scripts.Models.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameActions
{
    public class EffectExpireAction : GameAction
    {
        public Card card;
        public ActiveEffect effect;

        public EffectExpireAction(Card card, ActiveEffect effect)
        {
            this.card = card;
            this.effect = effect;
        }
    }
}
