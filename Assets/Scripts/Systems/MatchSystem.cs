using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
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
    public abstract class MatchSystem : Aspect, IObserve
    {
        public virtual void Awake()
        {
            this.AddObserver(OnPerformPassTurn, Global.PerformNotification<NextTurnAction>(), Container);
            this.AddObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), Container);
        }

        public virtual void Destroy()
        {
            this.RemoveObserver(OnPerformPassTurn, Global.PerformNotification<NextTurnAction>(), Container);
            this.RemoveObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), Container);
        }

        public abstract void StartMatch(Player player, int first);

        public abstract void NextTurn();

        public abstract void ChangeTurn();

        public abstract void GameOver();

        public void ChangeTurn(int index)
        {
            var action = new ChangeTurnAction(index);
            Container.Perform(action);
        }

        void OnPerformPassTurn(object sender, object args)
        {
            var action = args as NextTurnAction;
            var match = Container.GetMatch();
            match.currentPlayerIndex = action.targetPlayerIndex;
        }

        void OnPerformChangeTurn(object sender, object args)
        {
            var action = args as ChangeTurnAction;
            var match = Container.GetMatch();
            match.currentPlayerIndex = action.targetPlayerIndex;
        }
    }
}
