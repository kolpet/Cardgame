using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Cards;

namespace Assets.Scripts.Systems
{
    public class ConsumableCardSystem : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>());
        }


        public void Destroy()
        {
            this.RemoveObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>());
        }

        private void OnPerformPlayCard(object sender, object args)
        {
            var action = args as PlayCardAction;
            var card = action.card;

            if (card.Type.Contains(CardType.Consumable)) {
                var reaction = new BurnCardsAction(card);
                Container.AddReaction(reaction);
            }
        }
    }
}
