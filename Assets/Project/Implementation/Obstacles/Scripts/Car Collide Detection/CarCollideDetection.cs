using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class CarCollideDetection : MonoBehaviour
    {
        private const string CAR_CRASH_KEY = "Car Crash!";

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag != "Obstacle") return;
            if (GlobalEvents.Instance == null) return;

            GlobalEvents.Instance.SetNotificationCallback.Invoke("Crash other object!", (int)NotificationType.Danger);
            GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);
            GlobalEvents.Instance.AddPointCallback.Invoke(-10);

            GlobalEvents.Instance.AddMistakeCallback.Invoke(CAR_CRASH_KEY, 1);
        }
    }
}
