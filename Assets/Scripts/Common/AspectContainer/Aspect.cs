using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.AspectContainer
{
    public interface IAspect
    {
        IContainer Container { get; set; }
    }

    public class Aspect : IAspect
    {
        public IContainer Container { get; set; }
    }
}
