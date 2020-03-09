using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Abilities
{
    public class Target : Aspect
    {
        public bool required;
        public Mark preferred;
        public Mark allowed;
        public Card selected;
    }
}
