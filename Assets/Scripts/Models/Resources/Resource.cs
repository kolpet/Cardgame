using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Resources
{
    public abstract class Resource : IResource
    {
        public abstract ResourceType type { get; }

        public int Permanent { get; set; }

        public int Spent { get; set; }

        public int Temporary { get; set; }

        public abstract int Available { get; }
    }
}
