using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Cards;

namespace Assets.Scripts.Systems
{
    public class EventCardSystem : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnPrepareNextTurn, Global.PrepareNotification<NextTurnAction>());
        }


        public void Destroy()
        {
            this.RemoveObserver(OnPrepareNextTurn, Global.PrepareNotification<NextTurnAction>());
        }

        private void OnPrepareNextTurn(object sender, object args)
        {
            var action = args as NextTurnAction;
            var player = Container.GetMatch().CurrentPlayer as Player;
            if (player == null)
                return;

            foreach(var card in player.hand)
            {
                if(card is EventCard)
                {
                    var reaction = new PlayCardAction(card);
                    Container.AddReaction(reaction);
                }
            }
        }
    }
}
