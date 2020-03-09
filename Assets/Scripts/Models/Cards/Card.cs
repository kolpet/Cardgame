using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Cards
{
    public class Card : Container
    {
        public string id;
        public string name;
        public string description;
        public string asset;
        public ResourceType resource;
        public int cost;
        public int orderOfPlay = int.MaxValue;
        public int ownerIndex;
        public Zones zone = Zones.Deck;

        public virtual CardType Type => CardType.None;

        public virtual void Load(Dictionary<string, object> data)
        {
            id = (string)data["id"];
            name = (string)data["name"];
            description = (string)data["text"];
            asset = (string)data["asset"];
            resource = (ResourceType)Enum.Parse(typeof(ResourceType), (string)data["resource"]);
            cost = Convert.ToInt32(data["cost"]);
        }
    }
}
