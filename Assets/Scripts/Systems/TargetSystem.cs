using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class TargetSystem : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnValidatePlayCard, Global.ValidateNotification<PlayCardAction>());
        }

        public void Destroy()
        {
            this.RemoveObserver(OnValidatePlayCard, Global.ValidateNotification<PlayCardAction>());
        }

        public void AutoTarget(Card card, ControlModes mode)
        {
            var target = card.GetAspect<Target>();
            if (target == null)
                return;

            var mark = mode == ControlModes.Computer ? target.preferred : target.allowed;
            var candidates = GetMarks(card, mark);
            target.selected = candidates.Count > 0 ? candidates.Random() : null;
        }

        public List<Card> GetMarks(Card source, Mark mark)
        {
            var marks = new List<Card>();
            var combatants = GetPlayers(source, mark);
            foreach(Combatant combatant in combatants)
            {
                var cards = GetCards(source, mark, combatant);
                marks.AddRange(cards);
            }
            return marks;
        }

        List<Combatant> GetPlayers(Card source, Mark mark)
        {
            var dataSystem = Container.GetAspect<DataSystem>();
            var players = new List<Combatant>();
            var pair = new Dictionary<Alliance, Combatant>() {
                { Alliance.Ally, dataSystem.match.players[source.ownerIndex] },
                { Alliance.Enemy, dataSystem.match.players[1 - source.ownerIndex] }
            };
            foreach (Alliance key in pair.Keys)
            {
                if (mark.alliance.Contains(key))
                {
                    players.Add(pair[key]);
                }
            }
            return players;
        }

        List<Card> GetCards(Card source, Mark mark, Combatant combatant)
        {
            var cards = new List<Card>();
            var zones = new Zones[] {
                Zones.Party,
                Zones.Deck,
                Zones.Hand,
                Zones.Graveyard
            };
            foreach (Zones zone in zones)
            {
                if (mark.zones.Contains(zone))
                {
                    cards.AddRange(combatant[zone]);
                }
            }
            return cards;
        }

        public void OnValidatePlayCard(object sender, object args)
        {
            var playCardAction = sender as PlayCardAction;
            var card = playCardAction.card;
            var target = card.GetAspect<Target>();
            if (target == null || (target.required == false && target.selected == null))
                return;

            var validator = args as Validator;
            var candidates = GetMarks(card, target.allowed);
            if (!candidates.Contains(target.selected))
            {
                validator.Invalidate();
            }
        }
    }
}
