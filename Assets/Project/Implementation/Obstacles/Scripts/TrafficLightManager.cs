using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrivingSimulation
{
    public class TrafficLightManager : MonoBehaviour
    {
        public enum LightType
        {
            None, Green, Red, Yellow
        }

        private const string RED_NAME = "redlight";
        private const string YELLOW_NAME = "yellowlight";
        private const string GREEN_NAME = "greenlight";

        private LightType _currentType = LightType.None;

        private Light _redLight = null;
        private Light _yellowLight = null;
        private Light _greenLight = null;
        
        public void Initialize()
        {
            List<Light> lights = gameObject.GetComponentsInChildren<Light>().ToList();

            _redLight = lights.Find(x => x.gameObject.name == RED_NAME);
            _yellowLight = lights.Find(x => x.gameObject.name == YELLOW_NAME);
            _greenLight = lights.Find(x => x.gameObject.name == GREEN_NAME);
        }

        private void Update()
        {
            CheckCurrentLight();
        }

        private void CheckCurrentLight()
        {
            if (_redLight.enabled)
            {
                _currentType = LightType.Red;
                return;
            }

            if (_yellowLight.enabled)
            {
                _currentType = LightType.Yellow;
                return;
            }

            if (_greenLight.enabled)
            {
                _currentType = LightType.Green;
                return;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}
