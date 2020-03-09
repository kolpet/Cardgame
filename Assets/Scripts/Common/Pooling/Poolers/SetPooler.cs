using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Pooling.Poolers
{
    public class SetPooler : BasePooler
    {
        public HashSet<Poolable> Collection = new HashSet<Poolable>();

        public override void Enqueue(Poolable item)
        {
            base.Enqueue(item);
            if (Collection.Contains(item))
                Collection.Remove(item);
        }

        public override Poolable Dequeue()
        {
            Poolable item = base.Dequeue();
            Collection.Add(item);
            return item;
        }

        public override void EnqueueAll()
        {
            foreach (Poolable item in Collection)
                base.Enqueue(item);
            Collection.Clear();
        }
    }
}
