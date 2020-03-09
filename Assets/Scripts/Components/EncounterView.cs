using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Components
{
    public class EncounterView : CombatantView
    {
        public Encounter player => Combatant as Encounter;
    }
}
