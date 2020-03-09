using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Models.Abilities;

namespace Assets.Scripts.Interfaces
{
    public interface IAbilityLoader
    {
        void Load(IContainer game, Ability ability);
    }
}
