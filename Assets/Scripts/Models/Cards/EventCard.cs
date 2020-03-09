using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Cards
{
    public class EventCard : Card
    {
        public override CardType Type => CardType.Event;
    }
}
