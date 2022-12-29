using Sirenix.OdinInspector;
using SOGameEvents;
using UnityEngine;

namespace DrivingSimulation
{
    public class PointManager : MonoBehaviour
    {
        #region Variables

        [BoxGroup("Properties", Order = 0)]
        [SerializeField]
        private int _startPoint = 100;

        [BoxGroup("Properties", Order = 0)]
        [SerializeField, ReadOnly]
        private int _point = 0;

        [BoxGroup("Events", Order = 5)]
        [SerializeField]
        private GameEventInt _onPointChanged = null;

        [BoxGroup("Events", Order = 5)]
        [SerializeField]
        private GameEventInt _onPointDecreased = null;

        [BoxGroup("Events", Order = 5)]
        [SerializeField]
        private GameEventInt _onPointIncreased = null;

        [BoxGroup("Events", Order = 5)]
        [SerializeField]
        private GameEventInt _addPointCallback = null;

        #endregion

        #region Properties

        public int Point { get { return _point; } private set { _point = value; _onPointChanged.Invoke(_point); } }

        #endregion

        #region Mono

        private void Awake()
        {
            Point = _startPoint;

            // event init
            _addPointCallback.AddListener(AddPoint);
        }

        private void OnDestroy()
        {
            _addPointCallback.RemoveListener(AddPoint);
        }

        #endregion

        #region Methods

        private void AddPoint(int value)
        {
            if (Point - value <= 0)
            {
                Point = 0;
                return;
            }

            if (value < 0) _onPointDecreased.Invoke(Mathf.Abs(value));
            if (value > 0) _onPointIncreased.Invoke(Mathf.Abs(value));

            Point += value;
        }

        #endregion
    }
}
