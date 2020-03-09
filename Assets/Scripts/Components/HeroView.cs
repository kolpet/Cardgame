using Assets.Scripts.Models.Cards;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class HeroView : BattlefieldCardView
    {
        public Text armor;
        public Text magicResist;

        public Hero hero { get; private set; }

        public override Card card
        {
            get
            {
                return hero;
            }
            set
            {
                var hero = value as Hero;
                this.hero = hero;
                Refresh();
            }
        }

        public override void Refresh()
        {
            if (hero == null)
                return;

            avatar.sprite = isActive ? active : inactive;
            health.text = "Health: " + hero.HitPoints.ToString() + "/" + hero.MaxHitPoints.ToString();
            armor.text = "Armor: " + hero.Armor.ToString();
            magicResist.text = "Magic Resist: " + hero.MagicResist.ToString();
        }
    }
}
