using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Cards;
using System.Collections.Generic;

namespace Assets.Scripts.Systems
{
    public class CardSystem : Aspect
    {
        public List<Card> playable = new List<Card>();

        public void Refresh(ControlModes mode)
        {
            var match = Container.GetMatch();
            var targetSystem = Container.GetAspect<TargetSystem>();
            playable.Clear();
            foreach(Card card in match.CurrentPlayer[Zones.Hand])
            {
                targetSystem.AutoTarget(card, mode);
                var playAction = new PlayCardAction(card);
                if (playAction.Validate())
                    playable.Add(card);
            }
        }

        public void ChangeZone(Card card, Zones zone, Player toPlayer = null)
        {
            var fromPlayer = Container.GetMatch().players[card.ownerIndex] as Player;
            toPlayer = toPlayer ?? fromPlayer;
            fromPlayer[card.zone].Remove(card);
            toPlayer[zone].Add(card);
            card.zone = zone;
            card.ownerIndex = toPlayer.index;
        }

        public void BurnCard(Card card)
        {
            var player = Container.GetMatch().players[card.ownerIndex] as Player;
            player[card.zone].Remove(card);
        }
    }
}