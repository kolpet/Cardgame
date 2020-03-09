using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Resources;

namespace Assets.Scripts.Systems
{
    public class ManaSystem : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnPerformNextTurn, Global.PerformNotification<NextTurnAction>(), Container);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPerformNextTurn, Global.PerformNotification<NextTurnAction>(), Container);
        }

        private void OnPerformNextTurn(object sender, object args)
        {
            var player = Container.GetMatch().CurrentPlayer as Player;
            if (player == null) //Not Player
                return;

            var mana = player.resource as Mana;
            if (mana == null) //Not Mana user
                return;

            mana.Spent = 0;
            mana.Temporary = 0;

            this.PostNotification(ResourceSystem.ValueChangedNotification, mana);
        }
    }
}
