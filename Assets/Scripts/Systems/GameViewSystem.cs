using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.StateMachine;
using Assets.Scripts.Enums;
using Assets.Scripts.Factory;
using Assets.Scripts.GameActions;
using Assets.Scripts.GameStates;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class GameViewSystem : MonoBehaviour, IAspect
    {
        IContainer container;
        ActionSystem actionSystem;

        public IContainer Container 
        {
            get
            {
                if(container == null)
                {
                    container = GameFactory.Create();
                    container.AddAspect(this);
                }
                return container;
            }
            set => container = value;
        }

        void Awake()
        {
            Container.Awake();
            actionSystem = Container.GetAspect<ActionSystem>();
            Temp_SetupPvESinglePlayer();
        }

        void Start()
        {
            Container.ChangeState<NewMatchState>();
        }

        void Update()
        {
            actionSystem.Update();
        }

        void Temp_SetupPvPSinglePlayer()
        {
            var match = container.GetMatch();
            (match.players[0] as Player).mode = ControlModes.Local;
            (match.players[1] as Player).mode = ControlModes.Computer;

            foreach(Player p in match.players)
            {
                var deck = new List<Card>();//DeckFactory.CreateDeck("DemoDeck", p.index);
                p[Zones.Deck].AddRange(deck);

                for(int i = 0; i < p.maxDeck; i++)
                {
                    var card = new Card();
                    card.name = "Card " + i.ToString();
                    card.cost = UnityEngine.Random.Range(1, 10);
                    card.description = "Attack +1";
                    p[Zones.Deck].Add(card);
                }

                var hero = new Hero();
                hero.HitPoints = hero.MaxHitPoints;
                hero.ownerIndex = p.index;
                hero.zone = Zones.Party;
                p.party.Add(hero);
            }
        }

        void Temp_SetupPvESinglePlayer()
        {
            Container.GetAspect<DungeonSystem>().StartNewDungeon("DemoDungeon1");
            var player = new Player(0);

            player.mode = ControlModes.Local;
            player.resource.Permanent = 3;

            var deck = DeckFactory.CreateDeck("DemoDeck", player.index);
            player[Zones.Deck].AddRange(deck);

            var hero = new Hero();
            hero.HitPoints = 30;
            hero.MaxHitPoints = 30;
            hero.Armor = 0;
            hero.MagicResist = 0;
            hero.ownerIndex = player.index;
            hero.zone = Zones.Party;
            player.party.Add(hero);

            Container.GetAspect<DataSystem>().player = player;
        }
    }
}
