using Assets.Scripts.Enums;

namespace Assets.Scripts.Models.Resources
{
    public class Energy : Resource
    {
        public override int Available
        {
            get
            {
                return Permanent + Temporary - Spent;
            }
        }

        public override ResourceType type => ResourceType.Energy;
    }
}
