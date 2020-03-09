using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class PopupView : MonoBehaviour
    {
        public Text text;

        public void ChangeColor(Color color) => text.color = color;

        public void SetText(string txt) => text.text = txt;
    }
}
