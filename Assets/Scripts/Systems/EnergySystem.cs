using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Resources;

namespace Assets.Scripts.Systems
{
    public class EnergySystem : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), Container);
            this.AddObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>(), Container);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), Container);
            this.RemoveObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>(), Container);
        }

        private void OnPerformChangeTurn(object sender, object args)
        {
            var energy = GetEnergy();
            if (energy == null)
                return;

            energy.Spent = 0;
            energy.Temporary = 0;

            this.PostNotification(ResourceSystem.ValueChangedNotification, energy);
        }

        private void OnPerformPlayCard(object sender, object args)
        {
            var energy = GetEnergy();
            if (energy == null)
                return;

            if (energy.Spent > 0)
                energy.Spent--;

            this.PostNotification(ResourceSystem.ValueChangedNotification, energy);
        }

        private Energy GetEnergy()
        {
            var player = Container.GetMatch().CurrentPlayer as Player;
            if (player == null) //Not Player
                return null;

            var energy = player.resource as Energy;
            return energy;
        }
    }
}
