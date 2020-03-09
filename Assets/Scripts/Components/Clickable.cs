using Assets.Scripts.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components
{
    public class Clickable : MonoBehaviour, IPointerClickHandler
    {
        public const string ClickedNotification = "Clickable.ClickedNotification";

        public void OnPointerClick(PointerEventData eventData)
        {
            this.PostNotification(ClickedNotification, eventData);
        }
    }
}
