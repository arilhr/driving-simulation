using Sirenix.OdinInspector;
using SOGameEvents;
using TMPro;
using UnityEngine;

namespace DrivingSimulation
{
    public class UI_Point : MonoBehaviour
    {
        #region Variables

        [BoxGroup("References")]
        [SerializeField]
        private TMP_Text _pointText = null;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventInt _onPointChanged = null;

        #endregion

        #region Mono

        private void Awake()
        {
            _onPointChanged.AddListener(UpdateTextPoint);
        }

        private void OnDestroy()
        {
            _onPointChanged.RemoveListener(UpdateTextPoint);
        }

        #endregion

        #region Method

        private void UpdateTextPoint(int value)
        {
            _pointText.text = value.ToString();
        }

        #endregion
    }
}
