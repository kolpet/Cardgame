using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Common.StateMachine;
using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameStates
{
    public class PlayerIdleState : BaseState
    {
        public const string EnterNotification = "PlayerIdleState.EnterNotification";
        public const string ExitNotification = "PlayerIdleState.ExitNotification";

        public override void Enter()
        {
            var mode = Container.GetMatch().CurrentPlayer.mode;
            Container.GetAspect<AttackSystem>().Refresh();
            if(Container.GetMatch().CurrentPlayer is Player)
                Container.GetAspect<CardSystem>().Refresh(mode);
            else
                Container.GetAspect<EncounterSystem>().TakeTurn();
            if (mode == ControlModes.Computer) ;
            this.PostNotification(EnterNotification);
        }

        public override void Exit()
        {
            this.PostNotification(ExitNotification);
        }
    }
}
