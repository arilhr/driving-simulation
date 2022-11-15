using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace SOGameEvents
{
    /// <summary>
    /// Base class for game event
    /// </summary>
    [Serializable]
    public abstract class GameEvent : ScriptableObject
    {
#if UNITY_EDITOR
        protected struct StringExtend
        {
            [HideLabel, Multiline]
            public string extender;
        }

#endif
        #region variable
#if UNITY_EDITOR
        [Title("Change Event Type With ..."), HideLabel]
        [ShowInInspector, BoxGroup("Debug", Order = 100f), DisableInPlayMode]
        [ValueDropdown("GetAllGameEventType",DropdownTitle = "Select Game Event Change")]
        private Type _changeEventType = typeof(GameEvent);

        [BoxGroup("Subscribed Events", Order = 101f)]
        [ShowInInspector, ReadOnly, LabelText("\0")]
        protected List<StringExtend> _subscribedEvents = new List<StringExtend>();
#endif
        #endregion

        #region Properties
#if UNITY_EDITOR
        protected bool FoldoutEventSubscribtions { set; get; } = false;
#endif
        #endregion

        #region Main

        /// <summary>
        /// Remove all listeners from game event
        /// </summary>
        [Button]
        public abstract void RemoveAllListners();
#if UNITY_EDITOR
        /// <summary>
        /// Shows all event subscriptions. (Editor Only)
        /// </summary>
        [BoxGroup("Debug", Order = 100f)]
        [OnInspectorGUI]
        protected abstract void ShowPersistentEventListeners();

        /// <summary>
        /// Change game event confirmation action.
        /// </summary>
        [BoxGroup("Debug", Order = 100f)]
        [Button("Change GE Type"), DisableInPlayMode]
        private void ChangeGameEventType()
        {
            var confirm = EditorUtility.DisplayDialog("Change Game Event Type?",
                "Are you sure you want to change this Game Event Type? All references will be removed from all field.",
                "Proceed", "Cancel");

            if (!confirm) return;
        }

        /// <summary>
        /// Creates type value dropdown to choose.
        /// </summary>
        private IEnumerable<Type> GetAllGameEventType()
        {
            var ts = typeof(GameEvent).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition && typeof(GameEvent).IsAssignableFrom(t));

            return ts;
        }
#endif
        #endregion
    }

    /// <summary>
    /// Base class for game event with 1 parameter
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    public abstract class GameEvent<T> : GameEvent
    {
        #region variable

        /// <summary>
        /// Storing unity event
        /// </summary>
        private UnityEvent<T> _gameEvent = new UnityEvent<T>();

        #endregion

        #region GameEvent
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
        public void AddListener(UnityAction<T> action)
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
        public void RemoveListener(UnityAction<T> action)
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
        /// Invoke this game event(Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        [Button]
        public void Invoke(T p1)
        {
            _gameEvent.Invoke(p1);
        }

        #endregion
    }

    /// <summary>
    /// Base class for game event with 2 parameters
    /// </summary>
    /// <typeparam name="T1">Any type</typeparam>
    /// <typeparam name="T2">Any type</typeparam>
    public abstract class GameEvent<T1, T2> : GameEvent
    {
        #region Variable

        /// <summary>
        /// Storing unity event
        /// </summary>
        private UnityEvent<T1, T2> _gameEvent = new UnityEvent<T1, T2>();

        #endregion

        #region GameEvent
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
        public void AddListener(UnityAction<T1, T2> action)
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
        public void RemoveListener(UnityAction<T1, T2> action)
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
        /// Invoke this game event(Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        /// <param name="p2">2nd parameter</param>
        [Button]
        public void Invoke(T1 p1, T2 p2)
        {
            _gameEvent.Invoke(p1, p2);
        }

        #endregion
    }

    /// <summary>
    /// Base class for game event with 3 parameters
    /// </summary>
    /// <typeparam name="T1">Any type</typeparam>
    /// <typeparam name="T2">Any type</typeparam>
    /// <typeparam name="T3">Any type</typeparam>
    public abstract class GameEvent<T1, T2, T3> : GameEvent
    {
        #region variable

        /// <summary>
        /// Storing unity event
        /// </summary>
        private UnityEvent<T1, T2, T3> _gameEvent = new UnityEvent<T1, T2, T3>();

        #endregion

        #region GameEvent
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
        public void AddListener(UnityAction<T1, T2, T3> action)
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
        public void RemoveListener(UnityAction<T1, T2, T3> action)
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
        /// Invoke this game event(Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        /// <param name="p3">2nd parameter</param>
        /// <param name="p3">3rd parameter</param>
        [Button]
        public void Invoke(T1 p1, T2 p2, T3 p3)
        {
            _gameEvent.Invoke(p1, p2, p3);
        }

        #endregion
    }


    /// <summary>
    /// Base class for game event with 3 parameters
    /// </summary>
    /// <typeparam name="T1">Any type</typeparam>
    /// <typeparam name="T2">Any type</typeparam>
    /// <typeparam name="T3">Any type</typeparam>
    /// <typeparam name="T4">Any type</typeparam>
    public abstract class GameEvent<T1, T2, T3, T4> : GameEvent
    {
        #region variable

        /// <summary>
        /// Storing unity event
        /// </summary>
        private UnityEvent<T1, T2, T3, T4> _gameEvent = new UnityEvent<T1, T2, T3, T4>();

        #endregion

        #region GameEvent
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
        public void AddListener(UnityAction<T1, T2, T3, T4> action)
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
        public void RemoveListener(UnityAction<T1, T2, T3, T4> action)
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
        /// Invoke this game event(Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        /// <param name="p3">2nd parameter</param>
        /// <param name="p3">3rd parameter</param>
        /// <param name="p3">4th parameter</param>
        [Button]
        public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            _gameEvent.Invoke(p1, p2, p3, p4);
        }

        #endregion
    }
}