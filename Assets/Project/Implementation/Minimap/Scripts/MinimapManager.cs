using SOGameEvents;
using UnityEngine;

namespace DrivingSimulation
{
    public class MinimapManager : MonoBehaviour
    {
        [SerializeField]
        private NavMeshPathToLineRenderer _navigationPathRenderer = null;

        [Header("Events")]
        [SerializeField]
        private GameEventObject _onSetFinishLine = null;

        private void Awake()
        {
            _onSetFinishLine.AddListener(OnSetFinishLine);
        }

        private void OnDestroy()
        {
            _onSetFinishLine.RemoveListener(OnSetFinishLine);
        }

        private void OnSetFinishLine(object finishObj)
        {
            GameObject finishGameObject = (GameObject)finishObj;
            _navigationPathRenderer.SetEndPoint(finishGameObject.transform);
        }
    }
}
