using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class CurrentLaneManager : MonoBehaviour
    {
        private const string CORRECT_MESSAGE = "Correct Blinker!";
        private const string FALSE_MESSAGE = "False Blinker!";

        private string _from = string.Empty;

        [Header("Turn Sign Events")]
        [SerializeField] private GameEventNoParam _turnOffSignalCallback = null;

        private TurnSignalManager _turnSignalManager;

        private void Awake()
        {
            _turnSignalManager = GetComponent<TurnSignalManager>();
        }

        public void ChangeFirstLane(string changed)
        {
            _from = changed;
        }

        public void ChangeSecondLane(string changed)
        {
            if (_from == string.Empty)
            {
                _from = string.Empty;
                return;
            }

            CheckLanePass(_from, changed);
        }

        private void CheckLanePass(string before, string after)
        {
            if (_turnSignalManager == null) return;

            if (before == "RR")
            {
                if (after == "RL")
                {
                    CheckTurnSignal(TurnSignType.Right);
                }

                if (after == "LR")
                {
                    CheckTurnSignal(TurnSignType.Left);
                }

                if (after == "LL")
                {
                    CheckTurnSignal(TurnSignType.None);
                }
            }

            if (before == "LR")
            {
                if (after == "RR")
                {
                    CheckTurnSignal(TurnSignType.Right);
                }

                if (after == "LL")
                {
                    CheckTurnSignal(TurnSignType.Left);
                }

                if (after == "RL")
                {
                    CheckTurnSignal(TurnSignType.None);
                }
            }

            if (before == "LL")
            {
                if (after == "RL")
                {
                    CheckTurnSignal(TurnSignType.Right);
                }

                if (after == "LR")
                {
                    CheckTurnSignal(TurnSignType.Left);
                }

                if (after == "RR")
                {
                    CheckTurnSignal(TurnSignType.None);
                }
            }

            if (before == "RL")
            {
                if (after == "LL")
                {
                    CheckTurnSignal(TurnSignType.Right);
                }

                if (after == "RR")
                {
                    CheckTurnSignal(TurnSignType.Left);
                }

                if (after == "LR")
                {
                    CheckTurnSignal(TurnSignType.None);
                }
            }

            _from = string.Empty;
        }

        private void CheckTurnSignal(TurnSignType correctType)
        {
            if (_turnSignalManager == null) return;

            if (_turnSignalManager.CurrentTurnSignal == correctType)
            {
                CorrectBlinker();
                return;
            }

            FalseBlinker();
        }

        private void CorrectBlinker()
        {
            if (GlobalEvents.Instance == null) return;

            // Notification
            GlobalEvents.Instance.SetNotificationCallback.Invoke(CORRECT_MESSAGE, (int)NotificationType.Success);
            GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

            // Points
            GlobalEvents.Instance.AddPointCallback.Invoke(10);
            GlobalEvents.Instance.AddMistakeCallback.Invoke(CORRECT_MESSAGE, 1);
        }

        private void FalseBlinker()
        {
            if (GlobalEvents.Instance == null) return;

            // Notification
            GlobalEvents.Instance.SetNotificationCallback.Invoke(FALSE_MESSAGE, (int)NotificationType.Danger);
            GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

            // Points
            GlobalEvents.Instance.AddPointCallback.Invoke(-10);
            GlobalEvents.Instance.AddMistakeCallback.Invoke(FALSE_MESSAGE, 1);
        }
    }
}
