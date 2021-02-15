using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Enums;
using Assets.Scripts.Models.Cards;

namespace Assets.Scripts.Models.Auras
{
    public class BaseAura : Container, IAspect
    {
        public IContainer Container { get; set; }

        public Card Card { get { return Container as Card; } }

        public string ActionName { get; set; }

        public object userInfo { get; set; }
        
        public virtual AuraType Type => AuraType.None;

        public int Duration;
    }
}
