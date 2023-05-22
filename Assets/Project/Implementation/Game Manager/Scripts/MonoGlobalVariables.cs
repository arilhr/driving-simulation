using System;
using UnityEngine;

namespace DrivingSimulation
{
    public class MonoGlobalVariables : MonoBehaviour
    {
        public static MonoGlobalVariables Instance;

        [Header("Road Material")]
        public Material roadMaterial;
        public Material undersideMaterial;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            
        }
    }
}
