using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DrivingSimulation
{
    public class UI_MistakeItem : MonoBehaviour
    {
        public TMP_Text TitleText;
        public TMP_Text AmountText;

        public void SetText(string title, int amount)
        {
            TitleText.text = title;
            AmountText.text = $"{amount}x";
        }
    }
}
