using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface IResource
    {
        int Permanent { get; set; }

        int Spent { get; set; }

        int Temporary { get; set; }

        int Available { get; }
    }
}
