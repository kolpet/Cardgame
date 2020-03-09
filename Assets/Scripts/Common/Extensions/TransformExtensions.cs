using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Extensions
{
    public static class TransformExtensions
    {
		public static void ResetParent(this Transform obj, Transform target)
		{
			obj.SetParent(target, false);
			obj.localPosition = Vector3.zero;
			obj.localEulerAngles = Vector3.zero;
			obj.localScale = Vector3.one;
		}

		public static T GetOrAddComponent<T>(this Transform obj) where T : Component
		{
			T retValue = obj.GetComponent<T>();
			if (retValue == null)
				retValue = obj.gameObject.AddComponent<T>();
			return retValue;
		}
	}
}
