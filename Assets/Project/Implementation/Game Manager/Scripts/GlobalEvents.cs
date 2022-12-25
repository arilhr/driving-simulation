using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class GlobalEvents : MonoBehaviour
    {
        public static GlobalEvents Instance = null;

        [Header("Global Game Events")]
        public GameEventInt AddPointCallback = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
