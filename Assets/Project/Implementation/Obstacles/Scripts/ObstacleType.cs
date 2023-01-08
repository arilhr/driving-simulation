using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    [CreateAssetMenu(fileName = "New Obstacle Type", menuName = "Obstacle/New Obstacle Type")]
    public class ObstacleType : ScriptableObject
    {
        public string key;
        public string successMessage;
        public string failedMessage;
    }
}
