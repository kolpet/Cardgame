using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Models.Resources;
using Assets.Scripts.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class ResourceView : MonoBehaviour
    {
        public Image image;
        public Sprite available; 
        public Sprite unavailable;
        public Text availableText;

        private void OnEnable()
        {
            this.AddObserver(OnValueChangedNotification, ResourceSystem.ValueChangedNotification);
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnValueChangedNotification, ResourceSystem.ValueChangedNotification);
        }

        private void OnValueChangedNotification(object sender, object args)
        {
            var resource = args as Resource;
            availableText.text = resource.Available.ToString();
            if(resource.Available > 0)
            {
                image.sprite = available;
            }
            else
            {
                image.sprite = unavailable;
            }
        }
    }
}