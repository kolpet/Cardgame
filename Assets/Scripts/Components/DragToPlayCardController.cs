using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.StateMachine;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.GameStates;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using Assets.Scripts.Systems;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(InputHandler))]
    public class DragToPlayCardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public TargettingView targettingView;

        public float untargetedPlayDistance = 2f;

        //InputHandler inputHandler;

        IContainer game;
        StateMachine gameStateMachine;

        Container container;
        StateMachine stateMachine;
        CardView activeCardView;
        Card target;
        int pointerId;

        void Awake()
        {
            //inputHandler = GetComponentInParent<InputHandler>();

            game = GetComponentInParent<GameViewSystem>().Container;
            gameStateMachine = game.GetAspect<StateMachine>();

            container = new Container();
            stateMachine = container.AddAspect<StateMachine>();
            AddState<WaitingForInputState>();
            AddState<ShowTargetState>();
            AddState<TargetState>();
            AddState<FinishInputState>();
            AddState<CompleteState>();
            AddState<CancellingState>();
            AddState<ResetState>();
            stateMachine.ChangeState<WaitingForInputState>();
        }

        void AddState<T>() where T : BaseControllerState, new() => container.AddAspect(new T()).owner = this;

        #region Drag Handlers

        public void OnBeginDrag(PointerEventData eventData)
        {
            var handler = stateMachine.currentState as IBeginDragHandler;
            if (handler != null)
                handler.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var handler = stateMachine.currentState as IDragHandler;
            if (handler != null)
                handler.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var handler = stateMachine.currentState as IEndDragHandler;
            if (handler != null)
                handler.OnEndDrag(eventData);
        }

        #endregion

        private abstract class BaseControllerState : BaseState
        {
            public DragToPlayCardController owner;
        }

        private class WaitingForInputState : BaseControllerState, IBeginDragHandler
        {
            public void OnBeginDrag(PointerEventData eventData)
            {
                if (!(owner.gameStateMachine.currentState is PlayerIdleState))
                    return;

                owner.pointerId = eventData.pointerId;
                var press = eventData.rawPointerPress;
                var view = (press != null) ? press.GetComponentInParent<CardView>() : null;
                if (view == null ||
                    view.card.zone != Zones.Hand ||
                    view.card.ownerIndex != owner.game.GetMatch().currentPlayerIndex)
                    return;

                owner.activeCardView = view;
                owner.gameStateMachine.ChangeState<PlayerInputState>();

                var target = view.card.GetAspect<Target>();
                if (target != null)
                {
                    owner.stateMachine.ChangeState<ShowTargetState>();
                }
                else
                {
                    owner.stateMachine.ChangeState<FinishInputState>();
                }
            }
        }

        private class ShowTargetState : BaseControllerState
        {
            public override void Enter()
            {
                base.Enter();
                owner.StartCoroutine(HideProcess());
            }

            IEnumerator HideProcess()
            {
                var handView = owner.activeCardView.GetComponentInParent<HandView>();
                yield return owner.StartCoroutine(handView.LayoutCards(true));
                owner.stateMachine.ChangeState<TargetState>();
            }
        }

        private class TargetState : BaseControllerState, IEndDragHandler
        {
            Coroutine coroutine;

            public override void Enter()
            {
                base.Enter();
                coroutine = owner.StartCoroutine(Follow());
            }

            public override void Exit()
            {
                base.Exit();
                owner.StopCoroutine(coroutine);
                owner.targettingView.gameObject.SetActive(false);
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                var hover = eventData.pointerCurrentRaycast.gameObject;

                var view = (hover != null) ? hover.GetComponentInParent<BattlefieldCardView>() : null;
                var target = owner.activeCardView.card.GetAspect<Target>();
                if(view != null)
                {
                    target.selected = view.card;
                    owner.stateMachine.ChangeState<CompleteState>();
                }
                else
                {
                    owner.stateMachine.ChangeState<CancellingState>();
                }
            }

            private IEnumerator Follow()
            {
                Vector3 position;
                IEnumerator targetting = owner.targettingView.Targetting();
                owner.targettingView.gameObject.SetActive(true);
                while (true)
                {
                    if (owner.pointerId < 0)
                        position = Input.mousePosition;
                    else
                        position = Input.GetTouch(0).position;

                    position = Camera.main.ScreenToWorldPoint(position);
                    position.y = 0;
                    owner.targettingView.transform.position = position;
                    targetting.MoveNext();
                    yield return null;
                }
            }
        }

        private class FinishInputState : BaseControllerState, IDragHandler, IEndDragHandler
        {
            IEnumerator coroutine;
            Vector3 position;
            Vector3 startPosition;

            public override void Enter()
            {
                base.Enter();
                coroutine = Follow();
            }

            public void OnDrag(PointerEventData eventData)
            {
                position = eventData.position;
                coroutine.MoveNext();
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                var hover = eventData.pointerCurrentRaycast.gameObject;
                if(Vector3.Distance(startPosition, eventData.lastPress.transform.position) > owner.untargetedPlayDistance)
                    owner.stateMachine.ChangeState<CompleteState>();
                else
                    owner.stateMachine.ChangeState<CancellingState>();
            }

            private IEnumerator Follow()
            {
                Vector3 worldPos;
                startPosition = Camera.main.ScreenToWorldPoint(position);
                startPosition.y = 0;
                while (true)
                {
                    worldPos = Camera.main.ScreenToWorldPoint(position);
                    worldPos.y = 0;
                    owner.activeCardView.transform.position = worldPos;
                    yield return null;
                }
            }
        }

        private class CompleteState : BaseControllerState
        {
            public override void Enter()
            {
                base.Enter();
                if(owner.activeCardView != null)
                {
                    var action = new PlayCardAction(owner.activeCardView.card);
                    owner.game.Perform(action);
                    owner.stateMachine.ChangeState<ResetState>();
                }

                if (!owner.game.GetAspect<ActionSystem>().IsActive)
                    owner.gameStateMachine.ChangeState<PlayerIdleState>();

                owner.activeCardView = null;
                owner.target = null;
                owner.stateMachine.ChangeState<ResetState>();
            }
        }

        private class CancellingState : BaseControllerState
        {
            public override void Enter()
            {
                base.Enter();
                owner.StartCoroutine(HideProcess());
            }

            IEnumerator HideProcess()
            {
                var handView = owner.activeCardView.GetComponentInParent<HandView>();
                yield return owner.StartCoroutine(handView.LayoutCards(true));
                owner.stateMachine.ChangeState<ResetState>();
            }
        }

        private class ResetState : BaseControllerState
        {
            public override void Enter()
            {
                base.Enter();
                owner.stateMachine.ChangeState<WaitingForInputState>();
                if (!owner.game.GetAspect<ActionSystem>().IsActive)
                    owner.game.ChangeState<PlayerIdleState>();
            }
        }
    }
}
