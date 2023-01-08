using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DrivingSimulation
{
    public class CarCollideDetection : MonoBehaviour
    {
        private const string CAR_CRASH_KEY = "Car Crash!";

        [SerializeField]
        private LayerMask _obstacleLayer;

        private float _delayBetweenCrash = 0.2f;

        private bool _isActivate = true;

        private void Awake()
        {
            _isActivate = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isActivate) return;
            if ((_obstacleLayer & (1 << collision.gameObject.layer)) <= 0) return;

            if (GlobalEvents.Instance != null)
            {
                GlobalEvents.Instance.SetNotificationCallback.Invoke("Crash other object!", (int)NotificationType.Danger);
                GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);
                GlobalEvents.Instance.AddPointCallback.Invoke(-10);

                GlobalEvents.Instance.AddMistakeCallback.Invoke(CAR_CRASH_KEY, 1);
            }

            if (InGamePersonaDatasetManager.Instance != null)
            {
                InGamePersonaDatasetManager.Instance.Crash();
            }

            StartCoroutine(DelayActivate());
        }

        private IEnumerator DelayActivate()
        {
            _isActivate = false;

            yield return new WaitForSeconds(_delayBetweenCrash);

            _isActivate = true;
        }
    }
}
