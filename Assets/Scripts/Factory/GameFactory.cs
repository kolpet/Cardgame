using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.StateMachine;
using Assets.Scripts.Enums;
using Assets.Scripts.GameStates;
using Assets.Scripts.Systems;

namespace Assets.Scripts.Factory
{
    public static class GameFactory
    {
        public static Container Create()
        {
            Container game = new Container();

            // Add Default Systems
            game.AddAspect<ActionSystem>();
            game.AddAspect<AttackSystem>();
            game.AddAspect<DataSystem>();

            // Add Other
            game.AddAspect<StateMachine>();
            game.AddAspect<GlobalGameState>();

            game = AddPlayerSystems(game, false);
            game = AddCardSystems(game);
            game = AddResourceSystems(game, ResourceType.Mana);

            return game;    
        }

        public static Container AddPlayerSystems(Container game, bool isPvP)
        {
            game.AddAspect<DestructableSystem>();
            game.AddAspect<PlayerSystem>();
            game.AddAspect<ResourceSystem>();
            game.AddAspect<VictorySystem>();

            if (isPvP)
                game.AddAspect<PvPMatchSystem>();
            else
            {
                game.AddAspect<PvEMatchSystem>();
                game.AddAspect<DungeonSystem>();
                game.AddAspect<LootSystem>();
                game.AddAspect<EncounterSystem>();
            }

            return game;
        }

        public static Container AddCardSystems(Container game)
        {
            game.AddAspect<AbilitySystem>();
            game.AddAspect<TargetSystem>();

            game.AddAspect<CardSystem>();
            game.AddAspect<ConsumableCardSystem>();
            game.AddAspect<EventCardSystem>();
            game.AddAspect<CurseCardSystem>();
            game.AddAspect<EquipCardSystem>();

            return game;
        }

        public static Container AddResourceSystems(Container game, ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Mana:
                    game.AddAspect<ManaSystem>();
                    break;
                case ResourceType.Energy:
                    game.AddAspect<EnergySystem>();
                    break;
            }

            return game;
        }
    }
}
