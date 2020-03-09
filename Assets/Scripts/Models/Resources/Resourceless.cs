using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Resources
{
    public class Resourceless : Resource
    {
        public override int Available => int.MaxValue;

        public override ResourceType type => ResourceType.Resourceless;
    }
}
