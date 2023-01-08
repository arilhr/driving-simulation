using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DrivingSimulation
{
    public class UI_PersonaData : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _stopSignCountText = null;
        [SerializeField] private TMP_Text _trafficLightCountText = null;
        [SerializeField] private TMP_Text _maxSpeedCountText = null;
        [SerializeField] private TMP_Text _wrongLaneCountText = null;
        [SerializeField] private TMP_Text _crashCountText = null;
        [SerializeField] private TMP_Text _indicatorCountText = null;

        public void Initialize(PersonaDataset data)
        {
            _stopSignCountText.text = $"{data.WrongStopSign} / {data.CorrectStopSign}";
            _trafficLightCountText.text = $"{data.WrongTrafficLight} / {data.CorrectTrafficLight}";
            _maxSpeedCountText.text = $"{data.ViolateMaxSpeed} / {data.PassMaxSpeed}";
            _wrongLaneCountText.text = $"{data.WrongLane}";
            _crashCountText.text = $"{data.Crash}";
            _indicatorCountText.text = $"{data.WrongIndicator} / {data.CorrectIndicator}";
        }
    }
}
