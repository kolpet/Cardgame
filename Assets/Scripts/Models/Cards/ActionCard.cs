using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Cards
{
    public class ActionCard : Card, IPlayable
    {
        public virtual CardType Type => CardType.Action;
    }
}
