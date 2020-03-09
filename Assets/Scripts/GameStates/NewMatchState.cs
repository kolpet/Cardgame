using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Common.StateMachine;
using Assets.Scripts.Models;
using Assets.Scripts.Systems;

namespace Assets.Scripts.GameStates
{
    public class NewMatchState : BaseState
    {
        public const string EnterNotification = "NewGameState.EnterNotification";
        public const string ExitNotification = "NewGameState.ExitNotification";

        public override void Enter()
        {
            var player = Container.GetAspect<DataSystem>().player;
            Container.GetAspect<DungeonSystem>().AdvanceDungeon();
            Container.GetAspect<PvEMatchSystem>().StartMatch(player, 0);
            this.PostNotification(EnterNotification);

            Container.ChangeState<PlayerIdleState>();
        }

        public override void Exit()
        {
            this.PostNotification(ExitNotification);
        }
    }
}
