using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class DataSystem : Aspect
    {
        public Player player;
        public Match match;

        public void StartNewMatch(List<Combatant> list)
        {
            match = new Match(list);
        }
    }

    public static class DataSystemExtension
    {
        public static Match GetMatch(this IContainer game)
        {
            var dataSystem = game.GetAspect<DataSystem>();
            return dataSystem.match;
        }
    }
}
