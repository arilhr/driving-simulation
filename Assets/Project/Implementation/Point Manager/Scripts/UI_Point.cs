using DG.Tweening;
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

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventInt _onPointIncreased = null;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventInt _onPointDecreased = null;

        private Color _defaultTextColor;
        private float animationTime = 1f;

        #endregion

        #region Mono

        private void Awake()
        {
            _defaultTextColor = _pointText.color;

            _onPointChanged.AddListener(UpdateTextPoint);
            _onPointIncreased.AddListener(OnPointIncreased);
            _onPointDecreased.AddListener(OnPointDecreased);
        }

        private void OnDestroy()
        {
            _onPointChanged.RemoveListener(UpdateTextPoint);
            _onPointIncreased.RemoveListener(OnPointIncreased);
            _onPointDecreased.RemoveListener(OnPointDecreased);
        }

        #endregion

        #region Method

        private void UpdateTextPoint(int value)
        {
            _pointText.text = value.ToString();
        }

        private void OnPointIncreased(int point)
        {
            var colorSequence = DOTween.Sequence();

            colorSequence.Append(_pointText.DOColor(Color.green, animationTime / 2));
            colorSequence.Append(_pointText.DOColor(_defaultTextColor, animationTime / 2));

            _pointText.transform.DOScale(1f, animationTime).SetEase(Ease.OutBounce);
        }

        private void OnPointDecreased(int point)
        {
            var colorSequence = DOTween.Sequence();

            colorSequence.Append(_pointText.DOColor(Color.red, animationTime / 2));
            colorSequence.Append(_pointText.DOColor(_defaultTextColor, animationTime / 2));

            _pointText.transform.DOScale(1f, animationTime).SetEase(Ease.OutBounce);
        }

        #endregion
    }
}
