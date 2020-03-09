using Assets.Scripts.Common;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameActions;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class MonsterSystem : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), Container);
            this.AddObserver(OnPerformDrawCards, Global.PerformNotification<DrawCardsAction>(), Container);
            this.AddObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>(), Container);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), Container);
            this.RemoveObserver(OnPerformDrawCards, Global.PerformNotification<DrawCardsAction>(), Container);
            this.RemoveObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>(), Container);
        }

        private void OnPerformChangeTurn(object arg1, object arg2)
        {
            throw new NotImplementedException();
        }

        private void OnPerformDrawCards(object arg1, object arg2)
        {
            throw new NotImplementedException();
        }

        private void OnPerformPlayCard(object arg1, object arg2)
        {
            throw new NotImplementedException();
        }
    }
}
