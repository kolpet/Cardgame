using Assets.Scripts.Models;

namespace Assets.Scripts.GameActions
{
    public class OverdrawAction : DrawCardsAction
    {
        public OverdrawAction(Player player, int amount) : base(player, amount) { }
    }
}
