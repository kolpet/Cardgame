using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Pooling
{
    public class PoolData
    {
        public GameObject prefab;
        public int maxCount;
        public Queue<Poolable> pool;
    }
}
