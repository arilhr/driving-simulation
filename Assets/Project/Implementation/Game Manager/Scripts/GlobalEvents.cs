using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class GlobalEvents : MonoBehaviour
    {
        public static GlobalEvents Instance = null;

        [Header("Point Manager Events")]
        public GameEventInt AddPointCallback = null;

        [Header("Notification UI Events")]
        public GameEventStringInt SetNotificationCallback = null;
        public GameEventThreeFloat StartNoticationCallback = null;

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
