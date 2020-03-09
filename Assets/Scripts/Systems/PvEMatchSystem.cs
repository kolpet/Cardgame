using Assets.Scripts.Common.StateMachine;
using Assets.Scripts.Enums;
using Assets.Scripts.Factory;
using Assets.Scripts.GameActions;
using Assets.Scripts.GameStates;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    /* In PvE matches, the player gets to play as many cards as he wishes or is capable of.
     * Playing a card does trigger the change turn, but doesn't actually changes play order to the enemy.
     * Only after skipping can the monster begin play.
     */
    public class PvEMatchSystem : MatchSystem
    {
        public override void NextTurn()
        {
            var match = Container.GetMatch();
            var nextIndex = 1 - match.currentPlayerIndex;
            var action = new NextTurnAction(nextIndex);
            Container.Perform(action);
        }

        //Just send a change turn, but the same player remains
        //For energy purposed and shit
        public override void ChangeTurn()
        {
            var match = Container.GetMatch();
            ChangeTurn(match.currentPlayerIndex);
        }

        public override void StartMatch(Player player, int first)
        {
            var dungeonSystem = Container.GetAspect<DungeonSystem>();
            var enemy = dungeonSystem.GetEncounter(1 - player.index);
            Container.GetAspect<DataSystem>().StartNewMatch(new List<Combatant> { player, enemy });

            var action = new NextTurnAction(first);
            Container.Perform(action);
        }

        public override void GameOver()
        {
            var match = Container.GetMatch();
            var winner = match.CurrentPlayer;
            
            if(winner is Player)
            {
                Container.ChangeState<LootState>();
            }
        }
    }
}