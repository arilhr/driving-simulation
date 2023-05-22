using Sirenix.OdinInspector;
using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class InitAutomaticChallengeData : MonoBehaviour
    {
        #region Variables

        [BoxGroup("Difficulty")]
        public bool IsAutomaticDifficulty = false;

        [BoxGroup("Difficulty")]
        [DisableIf("IsAutomaticDifficulty")]
        public LevelDifficulty difficulty = null;

        [BoxGroup("Events")]
        public GameEventNoParam OnGameEndCallback = null;

        [BoxGroup("Debug")]
        [ShowInInspector, ReadOnly]
        private ChallengeData challenge = null;


        #endregion

        #region Mono

        private void Awake()
        {
            OnGameEndCallback?.AddListener(Save);
        }

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            OnGameEndCallback?.RemoveListener(Save);
        }

        #endregion

        #region Method

        private void Init()
        {
            if (ChallengeDataManager.Instance == null)
            {
                Debug.Log($"Challenge Data Manager is not found!");
                return;
            }

            if (IsAutomaticDifficulty)
                difficulty = ChallengeDataManager.Instance.GetLevelDifficulty();

            challenge = ChallengeDataManager.Instance.GenerateChallengeData(difficulty);

            if (AutomaticGenerateRoad.Instance == null)
            {
                Debug.Log("Automatic Road Generator in not found!");
                return;
            }

            float distancePerTurn = 100;
            int turns = Mathf.CeilToInt((challenge.Turn / 100f) * (challenge.RoadLength / distancePerTurn));

            AutomaticGenerateRoad.Instance.roadLength = challenge.RoadLength;
            AutomaticGenerateRoad.Instance.roadWidth = challenge.RoadWidth;
            AutomaticGenerateRoad.Instance.turns = turns;

            AutomaticGenerateRoad.Instance.GenerateRoad();
        }

        private void Save()
        {
            if (ChallengeDataManager.Instance == null)
            {
                Debug.Log("Challenge data manager is not found!");
                return;
            }

            ChallengeDataManager.Instance.AddDifficultyData(difficulty, challenge);
        }

        #endregion
    }
}
