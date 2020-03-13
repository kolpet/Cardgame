using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Models;

namespace Assets.Scripts.GameActions
{
    public class GameAction
    {
        public const string actionNameSpace = "Assets.Scripts.GameActions.";

        public readonly int id;

        public Combatant Player { get; set; }

        public int Priority { get; set; }

        public int OrderOfPlay { get; set; }

        public bool IsCancelled { get; set; }

        public Phase Prepare { get; set; }

        public Phase Perform { get; set; }

        public Phase Cancel { get; set; }

        public GameAction()
        {
            id = Global.GenerateID(GetType());
            Prepare = new Phase(this, OnPrepareKeyFrame);
            Perform = new Phase(this, OnPerformKeyFrame);
            Cancel = new Phase(this, OnCancelKeyFrame);
        }

        public virtual void CancelAction()
        {
            IsCancelled = true;
        }

        protected virtual void OnPrepareKeyFrame(IContainer game)
        {
            var notificationName = Global.PrepareNotification(GetType());
            game.PostNotification(notificationName, this);
        }

        protected virtual void OnPerformKeyFrame(IContainer game)
        {
            var notificationName = Global.PerformNotification(GetType());
            game.PostNotification(notificationName, this);
        }

        protected virtual void OnCancelKeyFrame(IContainer game)
        {
            var notificationName = Global.CancelNotification(GetType());
            game.PostNotification(notificationName, this);
        }
    }
}