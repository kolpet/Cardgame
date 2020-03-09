using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Abilities
{
    public class Ability : Container, IAspect
    {
        public IContainer Container { get; set; }

        public Card Card { get { return Container as Card; } }

        public string ActionName { get; set; }

        public object userInfo { get; set; }
    }
}
