using Assets.Scripts.Common;
using Assets.Scripts.Common.Animation;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Common.StateMachine;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.GameStates;
using Assets.Scripts.Models;
using Assets.Scripts.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Components
{
    public class ChangeTurnView : MonoBehaviour
    {
        [SerializeField] ChangeTurnButtonView buttonView;
        IContainer game;

        public void ChangeTurnButtonPressed()
        {
            if (CanChangeTurn())
            {
                var system = game.GetAspect<PvEMatchSystem>();
                system.NextTurn();
            }
        }

        bool CanChangeTurn()
        {
            var stateMachine = game.GetAspect<StateMachine>();
            if (!(stateMachine.currentState is PlayerIdleState))
                return false;

            var player = game.GetMatch().CurrentPlayer as Player;
            if (player == null ||
				player.mode != ControlModes.Local)
                return false;

            return true;
		}

		void Awake()
		{
			game = GetComponentInParent<GameViewSystem>().Container;
		}

		void OnEnable()
		{
			this.AddObserver(OnPreparePassTurn, Global.PrepareNotification<NextTurnAction>(), game);
			//this.AddObserver(OnPrepareChangeTurn, Global.PrepareNotification<ChangeTurnAction>(), game);
		}

		void OnDisable()
		{
			this.RemoveObserver(OnPreparePassTurn, Global.PrepareNotification<NextTurnAction>(), game);
			//this.RemoveObserver(OnPrepareChangeTurn, Global.PrepareNotification<ChangeTurnAction>(), game);
		}

		void OnPreparePassTurn(object sender, object args)
		{
			var action = args as NextTurnAction;
			action.Perform.viewer = ChangeTurnViewer;
		}

		void OnPrepareChangeTurn(object sender, object args)
		{
			var action = args as ChangeTurnAction;
			action.Perform.viewer = ChangeTurnViewer;
		}

		IEnumerator ChangeTurnViewer(IContainer game, GameAction action)
		{
			var dataSystem = game.GetAspect<DataSystem>();
			var changeTurnAction = action as NextTurnAction;
			var targetPlayer = dataSystem.match.players[changeTurnAction.targetPlayerIndex];

			var button = FlipButton(targetPlayer);

			while(button.MoveNext()) { yield return null; }
		}

		IEnumerator FlipButton(Combatant targetPlayer)
		{
			var up = Quaternion.Euler(new Vector3(90, 0, 0)); ;
			var down = Quaternion.Euler(new Vector3(270, 0, 0));
			var targetRotation = targetPlayer.mode == ControlModes.Local ? up : down;
			var tweener = buttonView.rotationHandle.RotateTo(targetRotation, 0.5f, EasingEquations.EaseOutBack);
			while (tweener.IsPlaying) { yield return null; }
		}
	}
}
