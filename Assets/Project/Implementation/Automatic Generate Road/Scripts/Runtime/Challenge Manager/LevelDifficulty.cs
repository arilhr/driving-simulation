using System;
using UnityEngine;

namespace DrivingSimulation
{
    [Serializable]
    public class LevelDifficulty
    {
        public int RoadLength;
        public int RoadWidth;
        public int Turn;
        public int Intersection;
        public int StopSignPercentage;
        public int TrafficLightPercentage;
    }
}