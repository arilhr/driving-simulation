using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrivingSimulation
{
    public class TrafficLightManager : MonoBehaviour
    {
        private enum LightType { None, Red, Yellow, Green }

        private const string RED_NAME = "redlight";
        private const string YELLOW_NAME = "yellowlight";
        private const string GREEN_NAME = "greenlight";
        private const string PLAYER_LAYER_NAME = "Player";

        private Light _redLight = null;
        private Light _yellowLight = null;
        private Light _greenLight = null;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            List<Light> lights = gameObject.GetComponentsInChildren<Light>().ToList();

            _redLight = lights.Find(x => x.gameObject.name == RED_NAME);
            _yellowLight = lights.Find(x => x.gameObject.name == YELLOW_NAME);
            _greenLight = lights.Find(x => x.gameObject.name == GREEN_NAME);
        }

        private LightType GetCurrentLight()
        {
            if (_redLight.enabled)
            {
                return LightType.Red;
            }

            if (_yellowLight.enabled)
            {
                return LightType.Yellow;
            }

            if (_greenLight.enabled)
            {
                return LightType.Green;
            }

            return LightType.None;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer(PLAYER_LAYER_NAME)) return;

            Vector3 rotation = transform.localRotation.eulerAngles;
            Quaternion offset = Quaternion.Euler(rotation.x, rotation.y + 90f, rotation.z);
            Vector3 forwardDir = offset * transform.forward;

            Vector3 directionToOther = other.transform.root.position - transform.position;

            float angle = Vector3.Angle(forwardDir, directionToOther);

            if (angle < 90f)
            {
                LightType currentType = GetCurrentLight();
                if (currentType == LightType.Red || currentType == LightType.Yellow)
                {
                    GlobalEvents.Instance.SetNotificationCallback.Invoke("You passing the red light!", (int)NotificationType.Danger);
                    GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

                    if (GlobalEvents.Instance.AddPointCallback != null)
                        GlobalEvents.Instance.AddPointCallback.Invoke(-20);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector3 rotation = transform.localRotation.eulerAngles;
            Quaternion offset = Quaternion.Euler(rotation.x, rotation.y + 90f, rotation.z);

            Gizmos.DrawLine(transform.position, transform.position + offset * transform.forward);
        }
    }
}
