using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.GameActions;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Effects
{
    public class ActiveEffect
    {
        public IContainer game;
        public Card card;
        public Effect effect;

        public ActiveEffect(IContainer game, Effect effect)
        {
            this.game = game;
            this.effect = effect;
        }

        public void OnEffectTrigger(object sender, object args)
        {
            var reaction = new EffectTriggerAction(effect);
            game.AddReaction(reaction);
        }

        public void OnEffectExpire(object sender, object args)
        {
            var reaction = new EffectExpireAction(card, this);
            game.AddReaction(reaction);
        }
    }
}
