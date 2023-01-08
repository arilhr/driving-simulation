using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DrivingSimulation
{
    public class UI_ChallengeData : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _longRoadText = null;
        [SerializeField] private TMP_Text _roadSizeText = null;
        [SerializeField] private TMP_Text _totalTotalTurnText = null;
        [SerializeField] private TMP_Text _totalIntersectText = null;
        [SerializeField] private TMP_Text _totalStopSignText = null;
        [SerializeField] private TMP_Text _totalTrafficLightText = null;
        [SerializeField] private TMP_Text _totalMaxSpeedText = null;

        public void Initialize(ChallengeGeneratedData data)
        {
            _longRoadText.text = $"{data.LongRoad}";
            _roadSizeText.text = $"{data.RoadSize}";
            _totalTotalTurnText.text = $"{data.TotalTurn}";
            _totalIntersectText.text = $"{data.TotalIntersect}";
            _totalStopSignText.text = $"{data.TotalStopSign}";
            _totalTrafficLightText.text = $"{data.TotalTrafficLight}";
            _totalMaxSpeedText.text = $"{data.TotalMaxSpeedLimit}";
        }
    }
}
