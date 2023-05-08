using Cinemachine;
using UnityEngine;

namespace DrivingSimulation
{
    public class CameraManager : Singleton<CameraManager>
    {
        [Header("References")]
        public CinemachineVirtualCamera vcam;
    }
}
