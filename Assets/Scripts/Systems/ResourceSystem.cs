using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class ResourceSystem : Aspect, IObserve
    {
        public const string ValueChangedNotification = "ResourceSystem.ValueChangedNotification";

        public void Awake()
        {
            this.AddObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>(), Container);
            this.AddObserver(OnValidatePlayCard, Global.ValidateNotification<PlayCardAction>());
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>(), Container);
            this.RemoveObserver(OnValidatePlayCard, Global.ValidateNotification<PlayCardAction>());
        }

        private void OnPerformPlayCard(object sender, object args)
        {
            var action = args as PlayCardAction;
            var player = Container.GetMatch().CurrentPlayer as Player;
            if (player != null)
            {
                var resource = player.resource;
                resource.Spent += action.card.cost;
                this.PostNotification(ValueChangedNotification, resource);
            }
        }

        private void OnValidatePlayCard(object sender, object args)
        {
            var playCardAction = sender as PlayCardAction;
            if (playCardAction.card.resource == ResourceType.Resourceless)
                return;

            var validator = args as Validator;
            if (Container.GetMatch().players[playCardAction.card.ownerIndex] is Player player && //Check if its a player
                (player.resource.type != playCardAction.card.resource || //Not right resource
                player.resource.Available < playCardAction.card.cost))  //Not enough resource
                validator.Invalidate();
        }
    }
}
