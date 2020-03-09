using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Match
    {
        public const int PlayerCount = 2;

        public List<Combatant> players = new List<Combatant>(PlayerCount);
        public int currentPlayerIndex = 0;
        public int nextTurnPlayer = 0;
        public int finishedPlayers = 0;

        public Combatant CurrentPlayer
        {
            get
            {
                return players[currentPlayerIndex];
            }
        }

        public Combatant OpponentPlayer
        {
            get
            {
                return players[1 - currentPlayerIndex];
            }
        }

        public Match(List<Combatant> players)
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                this.players.Add(players[i]);
            }
        }
    }
}
