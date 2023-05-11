using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class Player : MonoBehaviour
    {
        [Header("Camera")]
        public GameObject objToFollow;

        private void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            SetCamera();
        }

        void SetCamera()
        {
            if (CameraManager.Instance == null)
            {
                Debug.Log($"Camera manager is not found!");
                return;
            }

            CameraManager.Instance.vcam.Follow = objToFollow.transform;
            CameraManager.Instance.vcam.LookAt = objToFollow.transform;
        }
    }
}
