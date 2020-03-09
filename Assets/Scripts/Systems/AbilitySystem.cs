using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class AbilitySystem : Aspect, IObserve
    {
        const string actionNameSpace = "Assets.Scripts.GameActions.";

        public void Awake()
        {
            this.AddObserver(OnPerformPlayCardAction, Global.PerformNotification<PlayCardAction>(), Container);
            this.AddObserver(OnPerformAbilityAction, Global.PerformNotification<AbilityAction>(), Container);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPerformPlayCardAction, Global.PerformNotification<PlayCardAction>(), Container);
            this.RemoveObserver(OnPerformAbilityAction, Global.PerformNotification<AbilityAction>(), Container);
        }

        void OnPerformPlayCardAction(object sender, object args)
        {
            var action = args as PlayCardAction;
            var abilities = action.card.GetAspects<Ability>();
            if(abilities != null)
            {
                foreach (var ability in abilities)
                {
                    var reaction = new AbilityAction(ability);
                    Container.AddReaction(reaction);
                }
            }
        }

        void OnPerformAbilityAction(object sender, object args)
        {
            var action = args as AbilityAction;
            var type = Type.GetType(actionNameSpace + action.ability.ActionName);
            var instance = Activator.CreateInstance(type) as GameAction;
            var loader = instance as IAbilityLoader;
            if (loader != null)
                loader.Load(Container, action.ability);
            Container.AddReaction(instance);
        }
    }
}
