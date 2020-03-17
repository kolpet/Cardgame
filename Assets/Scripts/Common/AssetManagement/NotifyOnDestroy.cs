using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.AssetManagement
{
    public class NotifyOnDestroy : MonoBehaviour
    {
        public event Action<string, NotifyOnDestroy> Destroyed;
        public string Path { get; set; }

        public void OnDestroy()
        {
            Destroyed?.Invoke(Path, this);
        }
    }
}
