using Assets.Scripts.Models.Cards;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class MonsterView : BattlefieldCardView
    {
        public Text armor;
        public Text magicResist;

        public Text cardTurnText;

        public Monster monster { get; private set; }

        public override Card card
        {
            get
            {
                return monster;
            }
            set
            {
                var monster = value as Monster;
                this.monster = monster;
                Refresh();
            }
        }

        public override void Refresh()
        {
            if (monster == null)
                return;

            avatar.sprite = isActive ? active : inactive;
            health.text = "Health: " + monster.HitPoints.ToString() + "/" + monster.MaxHitPoints.ToString();
            armor.text = "Armor: " + monster.Armor.ToString();
            magicResist.text = "Magic Resist: " + monster.MagicResist.ToString();
            cardTurnText.text = FormatCardTurnText();
        }

        string FormatCardTurnText()
        {
            var cards = monster.monsterCards;
            string text = "Next actions: " + cards[0].turnsRemaining.ToString();
            for(int i = 1; i < cards.Count; i++)
            {
                text += ", " + cards[i].turnsRemaining.ToString();
            }
            return text;
        }
    }
}
