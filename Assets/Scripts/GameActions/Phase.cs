using Assets.Scripts.Common.AspectContainer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameActions
{
    public class Phase
    {
        public readonly GameAction owner;
        public readonly Action<IContainer> handler;
        public Func<IContainer, GameAction, IEnumerator> viewer;

        public Phase(GameAction owner, Action<IContainer> handler)
        {
            this.owner = owner;
            this.handler = handler;
        }

        public IEnumerator Flow(IContainer game)
        {
            bool hitKeyFrame = false;

            if(viewer != null)
            {
                var sequence = viewer(game, owner);
                while (sequence.MoveNext())
                {
                    var isKeyFrame = (sequence.Current is bool) ? (bool)sequence.Current : false;
                    if (isKeyFrame)
                    {
                        hitKeyFrame = true;
                        handler(game);
                    }
                    yield return null;
                }
            }

            if (!hitKeyFrame)
            {
                handler(game);
            }
        }
    }
}
