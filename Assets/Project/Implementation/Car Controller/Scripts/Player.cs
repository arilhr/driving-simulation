using UnityEngine;
using SOGameEvents;

namespace DrivingSimulation
{
    public class Player : Singleton<Player>
    {
        [Header("Camera")]
        public GameObject objToFollow;

        [Header("Events")]
        [SerializeField]
        private GameEventNoParam _initializeGame = null;

        protected override void Awake()
        {
            base.Awake();

            _initializeGame.AddListener(Initialize);
        }

        private void OnDestroy()
        {
            _initializeGame.RemoveListener(Initialize);
        }

        void Initialize()
        {
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
            }
        }

        //void SetCamera()
        //{
        //    if (CameraManager.Instance == null)
        //    {
        //        Debug.Log($"Camera manager is not found!");
        //        return;
        //    }

        //    CameraManager.Instance.vcam.Follow = objToFollow.transform;
        //    CameraManager.Instance.vcam.LookAt = objToFollow.transform;
        //}
    }
}
