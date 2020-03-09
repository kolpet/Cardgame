using Assets.Scripts.Common.AssetManagement;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class CardView : MonoBehaviour, IAssetLoader
    {
        public Image cardBack;
        public Image cardFront;
        public Image cardImage;
        public Image manaImage;
        public Text titleText;
        public Text cardText;
        public Text manaText;

        public Card card;

        private GameObject[] faceUpElements;
        private GameObject[] faceDownElements;

        public bool isFaceUp { get; private set; }

        void Awake()
        {
            faceUpElements = new GameObject[]
            {
                cardFront.gameObject,
                cardImage.gameObject,
                manaImage.gameObject,
                titleText.gameObject,
                cardText.gameObject,
                manaText.gameObject
            };
            faceDownElements = new GameObject[]
            {
                cardBack.gameObject
            };
            Flip(isFaceUp);
        }

        public void Flip(bool shouldShow)
        {
            isFaceUp = shouldShow;
            var show = shouldShow ? faceUpElements : faceDownElements;
            var hide = shouldShow ? faceDownElements : faceUpElements;
            Toggle(show, true);
            Toggle(hide, false);
            Refresh();
        }

        void Toggle(GameObject[] elements, bool isActive)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].SetActive(isActive);
            }
        }

        void Refresh()
        {
            if (isFaceUp == false)
                return;

            manaText.text = card.cost.ToString();
            titleText.text = card.name;
            cardText.text = card.description;
        }

        public List<AssetRequest> AssetRequests() =>
            new List<AssetRequest>
            {
                new AssetRequest("image", card.asset)
            };

        public void LoadAsset(string key, UnityEngine.Object asset)
        {
            switch (key)
            {
                case "image":
                    var sprite = asset as Sprite;
                    cardImage.sprite = sprite;
                    break;
            }
        }
    }
}
