using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class CarCollideDetection : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (GlobalEvents.Instance == null) return;

            GlobalEvents.Instance.SetNotificationCallback.Invoke("Crash other object!", (int)NotificationType.Danger);
            GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);
            GlobalEvents.Instance.AddPointCallback.Invoke(-10);
        }
    }
}
