using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using System.Collections.Generic;

namespace Assets.Scripts.Systems
{
    public class EquipCardSystem : Aspect, IObserve
    {
        List<Card> equips = new List<Card>();

        public void Awake()
        {
            this.AddObserver(OnPrepareNextTurn, Global.PrepareNotification<NextTurnAction>());
            this.AddObserver(OnPerformNextTurn, Global.PerformNotification<NextTurnAction>());
        }


        public void Destroy()
        {
            this.RemoveObserver(OnPrepareNextTurn, Global.PrepareNotification<NextTurnAction>());
            this.RemoveObserver(OnPerformNextTurn, Global.PerformNotification<NextTurnAction>());
        }

        private void OnPrepareNextTurn(object sender, object args)
        {
            equips.Clear();

            //var action = args as NextTurnAction;
            var player = Container.GetMatch().CurrentPlayer as Player;
            if (player == null)
                return;
            foreach (var card in player.hand)
                if (card.Type == CardType.Equip)
                    equips.Add(card);
        }

        private void OnPerformNextTurn(object sender, object args)
        {
            foreach (var card in equips)
            {
                if (card == null)
                    return;

                var abilities = card.GetAspects<Ability>();
                if (abilities != null)
                {
                    foreach (var ability in abilities)
                    {
                        var reaction = new AbilityAction(ability);
                        Container.AddReaction(reaction);
                    }
                }
            }
        }
    }
}
