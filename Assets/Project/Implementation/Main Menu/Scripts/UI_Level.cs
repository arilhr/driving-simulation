using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrivingSimulation
{
    public class UI_Level : MonoBehaviour
    {
        [SerializeField]
        private Button _levelButton = null;
        [SerializeField]
        private TMP_Text _levelText = null;

        public Button LevelButton
        {
            get { return _levelButton; }
        }

        public void SetText(string text)
        {
            _levelText.text = text;
        }
    }
}
