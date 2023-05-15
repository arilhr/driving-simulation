using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class BlinkerChecker : MonoBehaviour
    {
        private const string CORRECT_MESSAGE = "Correct Blinker!";
        private const string FALSE_MESSAGE = "False Blinker!";

        private Transform posBefore = null;

        public Transform PosBefore
        {
            set { posBefore = value; }
        }

        public void CheckBlinker(Transform posAfter)
        {
            if (posBefore == null) return;

            GameObject player = Player.Instance.gameObject;

            if (player == null) return;

            Vector3 directionToOther = posAfter.position - posBefore.position;

            Vector3 cross = Vector3.Cross(posBefore.forward, directionToOther).normalized;

            if (!player.TryGetComponent(out TurnSignalManager turnSignalManager))
                return;

            // Turn left
            if (cross.y > 0f)
            {
                if (turnSignalManager.CurrentTurnSignal == TurnSignType.Left)
                {
                    Success();
                    return;
                }
            }

            // Turn Right
            if (cross.y < 0f)
            {
                if (turnSignalManager.CurrentTurnSignal == TurnSignType.Right)
                {
                    Success();
                    return;
                }
            }

            // Forward
            if (cross.y == 0f)
            {
                if (turnSignalManager.CurrentTurnSignal == TurnSignType.None || turnSignalManager.CurrentTurnSignal == TurnSignType.Hazard)
                {
                    Success();
                    return;
                }
            }

            Failed();
        }

        public void Success()
        {
            if (GlobalEvents.Instance != null)
            {
                // Notification
                GlobalEvents.Instance.SetNotificationCallback.Invoke(CORRECT_MESSAGE, (int)NotificationType.Success);
                GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

                // Points
                GlobalEvents.Instance.AddPointCallback.Invoke(10);
            }

            if (InGamePersonaDatasetManager.Instance != null)
            {
                InGamePersonaDatasetManager.Instance.CorrectIndicator();
            }
        }

        public void Failed()
        {
            if (GlobalEvents.Instance != null)
            {
                // Notification
                GlobalEvents.Instance.SetNotificationCallback.Invoke(FALSE_MESSAGE, (int)NotificationType.Danger);
                GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

                // Points
                GlobalEvents.Instance.AddPointCallback.Invoke(-10);
                GlobalEvents.Instance.AddMistakeCallback.Invoke(FALSE_MESSAGE, 1);
            }

            if (InGamePersonaDatasetManager.Instance != null)
            {
                InGamePersonaDatasetManager.Instance.WrongIndicator();
            }
        }
    }
}
