using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Models.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class EffectSystem : Aspect, IObserve
    {
        

        public Dictionary<Card, List<ActiveEffect>> activeEffects = new Dictionary<Card, List<ActiveEffect>>();

        public void Awake()
        {
            this.AddObserver(OnPerformApplyEffectAction, Global.PerformNotification<ApplyEffectAction>(), Container);
            this.AddObserver(OnPerformEffectTriggerAction, Global.PerformNotification<EffectTriggerAction>(), Container);
            this.AddObserver(OnPerformEffectExpireAction, Global.PerformNotification<EffectExpireAction>(), Container);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPerformApplyEffectAction, Global.PerformNotification<ApplyEffectAction>(), Container);
            this.RemoveObserver(OnPerformEffectTriggerAction, Global.PerformNotification<EffectTriggerAction>(), Container);
            this.RemoveObserver(OnPerformEffectExpireAction, Global.PerformNotification<EffectExpireAction>(), Container);
        }

        void OnPerformApplyEffectAction(object sender, object args)
        {
            var action = args as ApplyEffectAction;
            foreach (Card target in action.targets)
            {
                ActiveEffect effect = new ActiveEffect(Container, action.effect);
                effect.card = target;
 
                var type = Type.GetType(GameAction.actionNameSpace + action.effect.TriggerName);
                this.AddObserver(effect.OnEffectTrigger, Global.PerformNotification(type), Container);
                //this.AddObserver(effect.OnEffectExpire, Global.PerformNotification(type), Container);

                if (activeEffects.ContainsKey(target) == false)
                    activeEffects[target] = new List<ActiveEffect>();
                activeEffects[target].Add(effect);
            }
        }

        void OnPerformEffectTriggerAction(object sender, object args)
        {
            var action = args as EffectTriggerAction;
            var type = Type.GetType(GameAction.actionNameSpace + action.ability.ActionName);
            var instance = Activator.CreateInstance(type) as GameAction;
            var loader = instance as IAbilityLoader;
            if (loader != null)
                loader.Load(Container, action.ability);
            Container.AddReaction(instance);
        }

        void OnPerformEffectExpireAction(object sender, object args)
        {
            var action = args as EffectExpireAction;
            var effect = action.effect;
            activeEffects[action.card].Remove(effect);
            var type = Type.GetType(GameAction.actionNameSpace + effect.effect.TriggerName);
            this.RemoveObserver(effect.OnEffectTrigger, Global.PerformNotification(type), Container);
            //this.RemoveObserver(effect.OnEffectExpire, Global.PerformNotification(type), Container);
        }
    }
}
