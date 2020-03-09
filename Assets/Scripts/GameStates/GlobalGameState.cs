using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Common.StateMachine;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameStates
{
    public class GlobalGameState : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnBeginSequence, ActionSystem.beginSequenceNotification);
            this.AddObserver(OnCompleteAllActions, ActionSystem.completeNotification);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnBeginSequence, ActionSystem.beginSequenceNotification);
            this.RemoveObserver(OnCompleteAllActions, ActionSystem.completeNotification);
        }

        private void OnBeginSequence(object sender, object args)
        {
            Container.ChangeState<SequenceState>();
        }

        private void OnCompleteAllActions(object sender, object args)
        {
            if (Container.GetAspect<VictorySystem>().IsGameOver())
                Container.GetAspect<PvEMatchSystem>().GameOver();
            else
                Container.ChangeState<PlayerIdleState>();
        }

    }
}
