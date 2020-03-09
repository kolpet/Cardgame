using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.GameActions;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Abilities;
using Assets.Scripts.Models.Cards;
using System.Collections;

namespace Assets.Scripts.Systems
{
    public class EncounterSystem : Aspect
    {
        IEnumerator playCards;
        public void TakeTurn()
        {
            if(playCards == null)
                playCards = PlayCards();

            if (playCards.MoveNext())
                return;

            playCards = null;
            Container.GetAspect<PvEMatchSystem>().NextTurn();
        }

        IEnumerator PlayCards()
        {
            var encounter = Container.GetMatch().CurrentPlayer as Encounter;
            
            for(int i = 0; i < encounter.party.Count; i++)
            {
                var monster = encounter.party[i] as Monster;
                var cards = monster.monsterCards;

                foreach(var card in cards)
                {
                    card.turnsRemaining--;
                    if(card.turnsRemaining <= 0)
                    {
                        var abilities = card.GetAspects<Ability>();
                        foreach (var ability in abilities)
                        {
                            var action = new AbilityAction(ability);
                            Container.Perform(action);

                            card.turnsRemaining = card.frequency;
                            yield return true;
                        }
                    }
                }
            }
        }
    }
}
