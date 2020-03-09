using Assets.Scripts.GameActions;
using Assets.Scripts.Models;

namespace Assets.Scripts.Systems
{
    /* In PvP Matches, every card played changes the play to the enemy player
     * Similar to LoR, the players play in an attacker/defender pattern, each having the advantage
     * of attacking first every second round
     */
    public class PvPMatchSystem : MatchSystem
    {
        public override void NextTurn()
        {
            var match = Container.GetMatch();
            var player = match.players[match.currentPlayerIndex] as Player;
            if (player.finished)
                return;

            player.finished = true;
            match.finishedPlayers++;
            if (match.finishedPlayers == match.players.Count)
            {
                var nextIndex = match.nextTurnPlayer;
                match.nextTurnPlayer = 1 - match.nextTurnPlayer;
                var action = new NextTurnAction(nextIndex);
                Container.Perform(action);
            }
            else
            {
                ChangeTurn();
            }
        }

        public override void ChangeTurn()
        {
            var match = Container.GetMatch();
            var nextIndex = 1 - match.currentPlayerIndex;
            ChangeTurn(nextIndex);
        }

        public override void GameOver()
        {
            throw new System.NotImplementedException();
        }

        public override void StartMatch(Player player, int first)
        {
            throw new System.NotImplementedException();
        }
    }
}
