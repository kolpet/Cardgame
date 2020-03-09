using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Pooling.Poolers
{
    public abstract class BasePooler : MonoBehaviour
    {
        public Action<Poolable> willEnqueue;
        public Action<Poolable> didDequeue;

        public string key = string.Empty;
        public GameObject prefab = null;
        public int prepopulate = 0;
        public int maxCount = int.MaxValue;
        public bool autoRegister = true;
        public bool autoClear = true;

        public bool isRegistered { get; private set; }

        protected virtual void Awake()
        {
            if (autoRegister)
                Register();
        }

        protected virtual void OnDestroy()
        {
            EnqueueAll();
            if (autoClear)
                UnRegister();
        }

        protected virtual void OnApplicationQuit()
        {
            EnqueueAll();
        }

        public void Register()
        {
            if (string.IsNullOrEmpty(key))
                key = prefab.name;
            GameObjectPoolController.AddEntry(key, prefab, prepopulate, maxCount);
            isRegistered = true;
        }

        public void UnRegister()
        {
            GameObjectPoolController.ClearEntry(key);
            isRegistered = false;
        }

        public virtual void Enqueue(Poolable item)
        {
            willEnqueue?.Invoke(item);
            GameObjectPoolController.Enqueue(item);
        }

        public virtual void EnqueueObject(GameObject obj)
        {
            Poolable item = obj.GetComponent<Poolable>();
            if (item != null)
                Enqueue(item);
        }

        public virtual void EnqueueScript(MonoBehaviour script)
        {
            Poolable item = script.GetComponent<Poolable>();
            if (item != null)
                Enqueue(item);
        }

        public virtual Poolable Dequeue()
        {
            Poolable item = GameObjectPoolController.Dequeue(key);
            didDequeue?.Invoke(item);
            return item;
        }

        public virtual U DequeueScript<U>() where U : MonoBehaviour
        {
            Poolable item = Dequeue();
            return item.GetComponent<U>();
        }

        public abstract void EnqueueAll();
    }
}
