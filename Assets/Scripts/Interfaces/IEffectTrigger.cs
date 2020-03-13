using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.GameActions;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface IEffectTrigger : IAspect
    {
        bool IsTriggered(IContainer game, GameAction action);

        void Load(Dictionary<string, object> data);
    }
}
