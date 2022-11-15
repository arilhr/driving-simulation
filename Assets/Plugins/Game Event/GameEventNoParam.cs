using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace SOGameEvents
{
    /// <summary>
    /// Base class for game event without parameter
    /// </summary>
    [CreateAssetMenu(
        fileName = "New No Param Event",
        menuName = "SOGameEvents/Game Events/No Param")]
    public class GameEventNoParam : GameEvent
    {
#if UNITY_EDITOR
        protected override void ShowPersistentEventListeners()
        {
            FoldoutEventSubscribtions = EditorGUILayout.BeginFoldoutHeaderGroup(FoldoutEventSubscribtions,
                $"Event Actions ({_gameEvent.GetHashCode()})");

            if (FoldoutEventSubscribtions)
            {
                var listenEventCount = _gameEvent.GetPersistentEventCount();

                if (listenEventCount == 0)
                {
                    EditorGUILayout.HelpBox("There is no event subscriptions.", MessageType.Info);
                }
                else
                {
                    for (int i = 0; i < listenEventCount; i++)
                    {
                        var eventName = _gameEvent.GetPersistentMethodName(i);
                        EditorGUILayout.LabelField(eventName);
                    }
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }
#endif
        #region variable

        /// <summary>
        /// Storing unity event
        /// </summary>
        private UnityEvent _gameEvent = new UnityEvent();

        #endregion

        #region GameEvent

        public override void RemoveAllListners()
        {
#if UNITY_EDITOR
            _subscribedEvents.Clear();
#endif
            _gameEvent.RemoveAllListeners();
        }

        #endregion

        #region Main

        /// <summary>
        /// Add listener to game event
        /// </summary>
        /// <param name="action">Action that will be subscribed</param>
        public void AddListener(UnityAction action)
        {
#if UNITY_EDITOR
            _subscribedEvents.Add(new StringExtend
            {
                extender = $"Method: {action.Method}\nFrom: {action.Method.DeclaringType}",
            });
#endif
            _gameEvent.AddListener(action);
        }

        /// <summary>
        /// Remove listener from game event
        /// </summary>
        /// <param name="action">Action that will be unsubscribed</param>
        public void RemoveListener(UnityAction action)
        {
#if UNITY_EDITOR
            _subscribedEvents.Remove(new StringExtend
            {
                extender = $"Method: {action.Method}\nFrom: {action.Method.DeclaringType}",
            });
#endif
            _gameEvent.RemoveListener(action);
        }

        /// <summary>
        /// Invoke this game event (Send event to all listeners)
        /// </summary>
        [Button]
        public void Invoke()
        {
            _gameEvent.Invoke();
        }

        #endregion
    }
}