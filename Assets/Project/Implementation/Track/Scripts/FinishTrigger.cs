using Sirenix.OdinInspector;
using SOGameEvents;
using UnityEngine;

namespace DrivingSimulation
{
    public class FinishTrigger : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _playerLayer;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventNoParam _gameSuccessCallback = null;

        private void OnTriggerEnter(Collider other)
        {
            if (!((_playerLayer.value & (1 << other.gameObject.layer)) > 0)) return;

            _gameSuccessCallback?.Invoke();
        }
    }
}
