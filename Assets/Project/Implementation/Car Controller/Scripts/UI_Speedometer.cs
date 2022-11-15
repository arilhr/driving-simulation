using SOGameEvents;
using TMPro;
using UnityEngine;

namespace DrivingSimulation
{
    public class UI_Speedometer : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _speedText = null;

        [SerializeField]
        private GameEventFloat _onUpdateSpeed = null;

        private void Awake()
        {
            if (_onUpdateSpeed != null)
                _onUpdateSpeed.AddListener(UpdateText);
        }

        private void OnDestroy()
        {
            if (_onUpdateSpeed != null)
                _onUpdateSpeed.RemoveListener(UpdateText);
        }

        private void UpdateText(float speed)
        {
            _speedText.text = Mathf.FloorToInt(speed).ToString();
        }
    }
}
