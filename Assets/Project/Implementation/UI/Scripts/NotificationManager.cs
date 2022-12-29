using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using SOGameEvents;

namespace DrivingSimulation
{
    public enum NotificationType { Default = 0, Success = 1, Danger = 2 }

    public class NotificationManager : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _parentCanvas = null;
        [SerializeField]
        private TMP_Text _notificationText = null;
        [SerializeField]
        private Coroutine _outCoroutine = null;

        [Header("Color")]
        [SerializeField]
        private Color _defaultColor = Color.white;
        [SerializeField]
        private Color _dangerColor = Color.red;
        [SerializeField]
        private Color _successColor = Color.green;

        [Header("Events")]
        [SerializeField]
        private GameEventStringInt _setNotificationCallback = null;
        [SerializeField]
        private GameEventThreeFloat _startNotificationCallback = null;

        private void Awake()
        {
            _setNotificationCallback.AddListener(SetActiveNotification);
            _startNotificationCallback.AddListener(StartNotification);
        }

        private void OnDisable()
        {
            _setNotificationCallback.RemoveListener(SetActiveNotification);
            _startNotificationCallback.RemoveListener(StartNotification);
        }

        private void SetActiveNotification(string message, int type = 0)
        {
            _notificationText.text = message;
            switch (type)
            {
                case (int)NotificationType.Default:
                    _notificationText.color = _defaultColor;
                    break;
                case (int)NotificationType.Danger:
                    _notificationText.color = _dangerColor;
                    break;
                case (int)NotificationType.Success:
                    _notificationText.color = _successColor;
                    break;
                default:
                    break;
            }
        }

        private void StartNotification(float fadeInTime, float delayTime, float fadeOutTime)
        {
            // Stop current coroutine
            if (_outCoroutine != null)
                StopCoroutine(_outCoroutine);

            // Fade in
            FadeIn(fadeInTime);

            // Start fade out coroutine
            _outCoroutine = StartCoroutine(FadeOutWithDelay(delayTime, fadeOutTime));
        }

        private void FadeIn(float fadeInTime)
        {
            // Animate Canvas Opacity
            _parentCanvas.alpha = 0f;

            _parentCanvas.DOFade(1f, fadeInTime);

            // Animate Text Scale
            _notificationText.transform.localScale = Vector3.zero;
            _notificationText.transform.DOScale(1f, fadeInTime).SetEase(Ease.OutBounce);
        }

        private void FadeOut(float time)
        {
            _parentCanvas.alpha = 1f;
            _parentCanvas.DOFade(0f, time);
        }

        private IEnumerator FadeOutWithDelay(float delayTime, float fadeTime)
        {
            yield return new WaitForSeconds(delayTime);

            FadeOut(fadeTime);
        }
    }
}
