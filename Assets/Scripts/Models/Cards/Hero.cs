﻿using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Cards
{
    public class Hero : Card, IResistant, IDestructable
    {
        // IArmored
        public int Armor { get; set; }

        public int MagicResist { get; set; }

        //IDestructable

        public int HitPoints { get; set; }

        public int MaxHitPoints { get; set; }

        public int TemporaryHitPoints { get; set; }
    }
}
