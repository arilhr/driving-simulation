using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DrivingSimulation
{
    public class LevelDifficulty
    {
        public int RoadLength;
        public int RoadWidth;
        public int Turn;
    }

    public class GeneratedDifficulty
    {
        public float RoadLength;
        public float RoadWidth;
        public float Turn;
    }


    public class ChallengeDataManager : Singleton<ChallengeDataManager>
    {
        [Header("Difficulty Data List")]
        public DifficultyData difficulties;

        [Header("Current Difficulty")]
        [SerializeField] private int laneDifficulty = 0;
        [SerializeField] private int intersectionDifficulty = 0;

        public LevelDifficulty GetLevelDifficulty()
        {
            LevelDifficulty result = new LevelDifficulty();

            // Lane Difficulty
            int remainingLaneDiffPoint = laneDifficulty;
            while (remainingLaneDiffPoint > 0)
            {
                // 0: road length, 1: road width, 2: turns
                List<int> challengePool = new List<int>{ 0, 1, 2 };
                if (result.RoadLength >= difficulties.RoadLengths.Count - 1)
                    challengePool.Remove(0);
                if (result.RoadWidth >= difficulties.RoadWidths.Count - 1)
                    challengePool.Remove(1);
                if (result.Turn >= difficulties.Turns.Count - 1)
                    challengePool.Remove(2);

                if (challengePool.Count <= 0)
                    break;

                int selectedLaneDiff = Random.Range(0, 3);
                int pointToAdded = Random.Range(1, remainingLaneDiffPoint);
                if (selectedLaneDiff == 0)
                    result.RoadLength += pointToAdded;
                if (selectedLaneDiff == 1)
                    result.RoadWidth += pointToAdded;
                if (selectedLaneDiff == 2)
                    result.Turn += pointToAdded;

                remainingLaneDiffPoint -= pointToAdded;
            }

            return result;
        }

        public GeneratedDifficulty GenerateDifficulty()
        {
            LevelDifficulty level = GetLevelDifficulty();

            return GenerateDifficulty(level);
        }

        public GeneratedDifficulty GenerateDifficulty(LevelDifficulty diffToGenerate)
        {
            if (difficulties == null)
            {
                Debug.Log("Difficulties data is null!");
                return null;
            }

            GeneratedDifficulty result = new GeneratedDifficulty();

            // Road Length
            if (difficulties.RoadLengths.Count <= 0)
            {
                Debug.Log("Road length difficulty in null!");
                return null;
            }

            float roadLengthResult = Random.Range(difficulties.RoadLengths[diffToGenerate.RoadLength].Min, difficulties.RoadLengths[diffToGenerate.RoadLength].Max);
            result.RoadLength = roadLengthResult;

            // Road widths
            if (difficulties.RoadWidths.Count <= 0)
            {
                Debug.Log("Road widths difficulty in null!");
                return null;
            }

            float roadWidthResult = Random.Range(difficulties.RoadWidths[diffToGenerate.RoadWidth].Min, difficulties.RoadWidths[diffToGenerate.RoadWidth].Max);
            result.RoadWidth = roadWidthResult;

            // Turns
            if (difficulties.Turns.Count <= 0)
            {
                Debug.Log("Turns difficulty in null!");
                return null;
            }

            float turnsResult = Random.Range(difficulties.Turns[diffToGenerate.Turn].Min, difficulties.Turns[diffToGenerate.Turn].Max);
            result.Turn = turnsResult;

            return result;
        }

        public void CountDifficulty(PersonaDataValue newest, PersonaDataValue average)
        {

        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ChallengeDataManager))]
    public class ChallengeDataManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ChallengeDataManager manager = (ChallengeDataManager)target;

            if (GUILayout.Button("Generate Challenge Data"))
            {
                GeneratedDifficulty result = manager.GenerateDifficulty();
                
                if (result == null) return;

                string resJson = JsonConvert.SerializeObject(result);

                Debug.Log(resJson);
            }
        }
    }
#endif
}
