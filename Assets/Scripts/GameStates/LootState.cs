using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Common.StateMachine;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameStates
{
    public class LootState : BaseState
    {
        public const string EnterNotification = "LootState.EnterNotification";
        public const string ExitNotification = "LootState.ExitNotification";

        public override void Enter()
        {
            Container.GetAspect<LootSystem>().Refresh();
            this.PostNotification(EnterNotification);
        }

        public override void Exit()
        {
            this.PostNotification(ExitNotification);
        }
    }
}
