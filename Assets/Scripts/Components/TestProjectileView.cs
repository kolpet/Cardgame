using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class TestProjectileView : MonoBehaviour
    {
        public float maxSpeed = 3f;
        public float launchSpeed = 3f;
        public float forcePower = 1f;

        public IEnumerator FlyToTarget(Transform origin, Transform target)
        {
            transform.position = origin.position;
            Vector3 velocity = Vector3.forward * launchSpeed;
            Vector3 force;
            while(Vector3.Distance(transform.position, target.position) > velocity.magnitude)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, forcePower * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
