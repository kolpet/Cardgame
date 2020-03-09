using Assets.Scripts.Common.AspectContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.StateMachine
{
    public interface IState : IAspect
    {
        void Enter();

        void Exit();

        bool CanTransition(IState other);
    }

    public abstract class BaseState : Aspect, IState
    {
        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual bool CanTransition(IState other) => true;
    }
}
