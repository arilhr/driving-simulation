using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    [CreateAssetMenu(fileName = "Difficulty Data", menuName = "Difficulty Data/New Difficulty Data", order = 0)]
    public class DifficultyData : ScriptableObject
    {
        [Space(5f)]
        [BoxGroup("Difficulty")]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = true)]
        public List<MinMaxValue<float>> RoadLengths = new List<MinMaxValue<float>>();
        [Space(5f)]
        [BoxGroup("Difficulty")]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = true)]
        public List<MinMaxValue<float>> RoadWidths = new List<MinMaxValue<float>>();
        [Space(5f)]
        [BoxGroup("Difficulty")]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = true)]
        public List<MinMaxValue<float>> Turns = new List<MinMaxValue<float>>();

        public int TotalLaneDifficulty()
        {
            return RoadLengths.Count + RoadWidths.Count + Turns.Count;
        }
    }
}
