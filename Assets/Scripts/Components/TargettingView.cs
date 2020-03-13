using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class TargettingView : MonoBehaviour
    {
        public Image crosshairImage;
        public float maxSpinSpeed = 1f;
        public float spinAccelerate = 1f;

        float spinSpeed;
        Quaternion originalRotation;

        public void Awake()
        {
            crosshairImage.raycastTarget = false;
            originalRotation = transform.rotation;
        }

        public IEnumerator Targetting()
        {
            spinSpeed = 0;
            transform.rotation = originalRotation;
            while (true)
            {
                spinSpeed = Math.Min(spinSpeed + spinAccelerate * Time.deltaTime, maxSpinSpeed);
                transform.Rotate(0, 0, spinSpeed * Time.deltaTime, Space.Self);
                yield return null;
            }
        }
    }
}
