using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface ITargetSelector : IAspect
    {
        List<Card> SelectTargets(IContainer game);

        void Load(Dictionary<string, object> data);
    }
}
