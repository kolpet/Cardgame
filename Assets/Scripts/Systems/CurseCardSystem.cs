using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Cards;

namespace Assets.Scripts.Systems
{
    public class CurseCardSystem : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnPrepareNextTurn, Global.PrepareNotification<NextTurnAction>());
            this.AddObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>());
        }


        public void Destroy()
        {
            this.RemoveObserver(OnPrepareNextTurn, Global.PrepareNotification<NextTurnAction>());
            this.AddObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>());
        }

        private void OnPrepareNextTurn(object sender, object args)
        {
            //var action = args as NextTurnAction;
            var player = Container.GetMatch().CurrentPlayer as Player;
            if (player == null)
                return;

            foreach (var card in player.hand)
            {
                if (card.Type == CardType.Curse)
                {
                    var reaction = new PlayCardAction(card);
                    Container.AddReaction(reaction);
                }
            }
        }

        private void OnPerformPlayCard(object sender, object args)
        {
            var action = args as PlayCardAction;
            var card = action.card as CurseCard;
            if (card == null)
                return;

            var reaction = new BurnCardsAction(card);
            Container.AddReaction(reaction);
        }
    }
}