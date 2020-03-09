using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameActions;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Systems
{
    public class ActionSystem : Aspect
    {
        public const string beginSequenceNotification = "ActionSystem.beginSequenceNotification";
        public const string endSequenceNotification = "ActionSystem.endSequenceNotification";
        public const string deathReaperNotification = "ActionSystem.deathReaperNotification";
        public const string completeNotification = "ActionSystem.completeNotification";

        Queue<GameAction> actionQueue = new Queue<GameAction>();
        GameAction rootAction;
        IEnumerator rootSequence;
        List<GameAction> openReactions;

        public bool IsActive { get { return rootSequence != null; } }

        public void Perform(GameAction action)
        {
            if (IsActive)
                actionQueue.Enqueue(action);
            else
            {
                rootAction = action;
                rootSequence = Sequence(action);
            }
        }

        public void Update()
        {
            if (rootSequence == null)
                return;

            if (rootSequence.MoveNext() == false)
            {
                rootAction = null;
                rootSequence = null;
                openReactions = null;
                this.PostNotification(completeNotification);
            }

            if (actionQueue.Count > 0)
                Perform(actionQueue.Dequeue());
        }

        public void AddReaction(GameAction action)
        {
            if (openReactions != null)
                openReactions.Add(action);
        }

        IEnumerator Sequence(GameAction action)
        {
            this.PostNotification(beginSequenceNotification, action);

            if (action.Validate() == false)
                action.CancelAction();

            var phase = MainPhase(action.Prepare);
            while(phase.MoveNext()) { yield return null; }

            phase = MainPhase(action.Perform);
            while (phase.MoveNext()) { yield return null; }

            phase = MainPhase(action.Cancel);
            while (phase.MoveNext()) { yield return null; }

            if(rootAction == action)
            {
                phase = EventPhase(deathReaperNotification, action, true);
                while (phase.MoveNext()) { yield return null; }
            }

            this.PostNotification(endSequenceNotification, action);
        }

        IEnumerator MainPhase(Phase phase)
        {
            bool isActionCancelled = phase.owner.IsCancelled;
            bool isCancelPhase = phase.owner.Cancel == phase;

            if (isActionCancelled ^ isCancelPhase)
                yield break;

            var reactions = openReactions = new List<GameAction>();
            var flow = phase.Flow(Container);
            while(flow.MoveNext()) { yield return null; }

            flow = ReactPhase(reactions);
            while(flow.MoveNext()) { yield return null; }
        }

        IEnumerator ReactPhase(List<GameAction> reactions)
        {
            reactions.Sort(SortActions);
            foreach(GameAction reaction in reactions)
            {
                IEnumerator subFlow = Sequence(reaction);
                while (subFlow.MoveNext())
                {
                    yield return null;
                }
            }
        }

        IEnumerator EventPhase(string notification, GameAction action, bool repeats = false)
        {
            List<GameAction> reactions;
            do
            {
                reactions = openReactions = new List<GameAction>();
                this.PostNotification(notification, action);

                var phase = ReactPhase(reactions);
                while(phase.MoveNext()) { yield return null; }
            } while (repeats == true && reactions.Count > 0);
        }

        private int SortActions(GameAction x, GameAction y)
        {
            if(x.Priority != y.Priority)
            {
                return y.Priority.CompareTo(x.Priority);
            }
            else
            {
                return x.OrderOfPlay.CompareTo(y.OrderOfPlay);
            }
        }
    }

    public static class ActionSystemExtensions
    {
        public static void Perform(this IContainer game, GameAction action)
        {
            var actionSystem = game.GetAspect<ActionSystem>();
            actionSystem.Perform(action);
        }

        public static void AddReaction(this IContainer game, GameAction action)
        {
            var actionSystem = game.GetAspect<ActionSystem>();
            actionSystem.AddReaction(action);
        }
    }
}