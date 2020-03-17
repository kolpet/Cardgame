using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Cards
{
    public class PassiveCard : Card, IPlayable
    {
        public override CardType Type => CardType.Passive;
    }
}
